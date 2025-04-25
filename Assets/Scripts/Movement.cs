using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rb;
    Animator anim;
    bool isMoving;
    Vector2 movDirection;
    float moveX, moveY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // for inputs
        GetInput();

        // for Animations 
        Animate();
    }

    void FixedUpdate()
    {
        // for physics calculations
        rb.velocity = new Vector2(movDirection.x * moveSpeed, movDirection.y * moveSpeed);
    }

    void GetInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        movDirection = new Vector2(moveX, moveY).normalized;
    }

    void Animate()
    {
        if(movDirection.magnitude >= 0.1f || movDirection.magnitude <= -0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            anim.SetFloat("X", moveX);
            anim.SetFloat("Y", moveY);
        }

        anim.SetBool("isMoving", isMoving);
    }
}
