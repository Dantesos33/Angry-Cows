using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using Unity.VisualScripting;

public class CowAI : MonoBehaviour
{
    Transform target;
    Animator anim;
    Seeker seeker;
    Rigidbody2D rb;
    Path path;
    Audio_Manager audioManager;

    [SerializeField] float speed = 6000f;
    [SerializeField] float sightDistance = 6f;
    [SerializeField] LayerMask obstacleMask;

    float nextWaypointDistance = 3f;
    int currentWaypoint = 0;

    bool isChasing = false;
    bool isMoving;
    // bool reachedEndofPath = false;

    


    void Start()
    {
        target = FindAnyObjectByType<Movement>().transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio_Manager>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && isChasing)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {

        if(path == null)
        {
            return;
        }

        /* if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        } */

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 faceDirection = (target.position - transform.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (isChasing)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rb.AddForce(force);
            audioManager.PlayMusic(audioManager.cowChase);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
    }

    bool IsPlayerInSight()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        // Draw the ray for debugging
        Debug.DrawLine(transform.position, (Vector2)transform.position + (direction * sightDistance), Color.red);

        // Check if there are any obstacles between the cow and the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, sightDistance, obstacleMask);

        // If the raycast hits something, check if it's the player
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name); // Log the name of the object hit
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            
        }

        return false; // Player is not in sight
    }

    void Update()
    {
        if (IsPlayerInSight())
        {
            isChasing = true;
        }

        Animate();

    }

    void Animate()
    {
        if (rb.velocity.magnitude >= 0.01f || rb.velocity.magnitude <= -0.01f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            anim.SetFloat("X", rb.velocity.x);
            anim.SetFloat("Y", rb.velocity.y);
        }

        anim.SetBool("isMoving", isMoving);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            isChasing = false;
            LoadCurrentScene();
        }
        
    }

        void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
