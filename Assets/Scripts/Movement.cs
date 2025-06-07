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
    public VirtualJoystick joystick;
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
        rb.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);

    }

    void GetInput()
    {
        // Get movement from Keyboard input
        float keyboardX = Input.GetAxis("Horizontal"); // A/D or Arrow keys
        float keyboardY = Input.GetAxis("Vertical"); // W/S or Arrow keys

        // Get movement from Joystick input
        float joystickX = joystick.Horizontal();
        float joystickY = joystick.Vertical();

        moveX = joystickX != 0 ? joystickX : keyboardX;  // Use joystick if it's moving, otherwise use keyboard
        moveY = joystickY != 0 ? joystickY : keyboardY;  // Use joystick if it's moving, otherwise use keyboard


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
