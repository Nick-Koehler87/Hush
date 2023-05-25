using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    

    [Header("Settings")]
    public float maxSpeed = 10f;
    public float speed = 5;
    public float maxAirSpeed = 10f;
    public float airMultiplier = .1f;
    public float jumpForce = 5;
    public float groundDrag = 10f;
    public float airDrag = 5f;
    public float playerHight;

    private Transform transform;
    Rigidbody2D rb;
    private Vector2 pos = new Vector2(0, 0);
    float horizontalInput = 0;
    bool grounded = false;
    bool readyToJump = true;


    public LayerMask whatIsGround;
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        settings();
    }

    private void settings() 
    {
        pos = new Vector2(transform.position.x, transform.position.y);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, playerHight, whatIsGround);
        grounded = (hit.collider != null);
        //print(grounded);
        if (grounded)
        {
            //print("drag");
            rb.drag = groundDrag;

            //prevents jump from being activated multiple times ie, must fall before jump again. it deals with the inactuarcy of player hight in the raycast
            if (rb.velocity.y <= .01){
                readyToJump = true;
            }
        }
        else 
        {
            //print("no drag");
            rb.drag = 0;
        }

        if (Input.GetKey("space") && grounded && readyToJump) {
            //print("space");
            jump();
        }
        walk();
        
    }

    private void walk()
    {
        //horizontalInput == -1 for left, 1 for right
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //will need to update for jump
        Vector2 moveDirection = new Vector2(horizontalInput, 0f);
        if (horizontalInput != 0 && grounded){
            rb.drag = 0;
        }
        //if in air go slower
        if (grounded) {
            rb.AddForce(moveDirection * speed, ForceMode2D.Force);
        } else {
            rb.AddForce(moveDirection * speed * airMultiplier, ForceMode2D.Force);
        }
        //speed control handels topspeed and friction
        SpeedControl();
    }

    private void SpeedControl(){
        //if not pressing left or right, add friction
        if (horizontalInput == 0) {
            //rb.velocity = new Vector2(rb.velocity.x / friction, rb.velocity.y);
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);

        //if in air go slower
        if (!grounded) {
            rb.velocity = new Vector2(rb.velocity.x / airDrag, rb.velocity.y);
        }
    }

    private void jump(){
        //print(jumpForce);
        Vector2 jumpForceVector = new Vector2(0f, jumpForce);
        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
        grounded = false;
        readyToJump = false;
    }
    void OnCollision(Collision collision)
    {
        print("colission");
    }
}
