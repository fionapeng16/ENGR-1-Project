using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    [SerializeField] float speed=1f;
    [SerializeField] float jumpHeight=3f;
    float direction=0;
    bool isGrounded=true;
    int timesJumped=0;
    bool isFacingRight=true;
    bool isDead = false;
    private Vector3 spawnPoint;
    Animator animator;
    int timer = 0;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (isDead && timer==1000) {
            transform.position = spawnPoint; // Reset to the starting position
            rb.linearVelocity = Vector2.zero; // Stop movement
            isDead = false;
            animator.SetBool("isDead", false);
            timer = 0;
        }
        else if (isDead) {
            timer++;
            rb.linearVelocity=new Vector2(0, 0);
            return;
        }

        Move(direction);
        if ((isFacingRight && direction==-1) || (!isFacingRight && direction==1)) {
            flip();
        }
    }
    void OnMove(InputValue value) {
        float v = value.Get<float>();
        direction=v;
    }

    void Move(float dir) {
        animator.SetBool("isRunning", dir!=0);
        rb.linearVelocity=new Vector2(dir*speed, rb.linearVelocity.y);
    }

    void OnJump() {
        if (isGrounded || timesJumped==1) {
            Jump();
        }
        else {
            timesJumped=0;
        }
        
    }

    void Jump() {
        timesJumped++;
        isGrounded = false;
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isDoubleJumping", timesJumped==2);
        rb.linearVelocity=new Vector2(rb.linearVelocity.x, jumpHeight);
    }
    void OnCollisionEnter2D(Collision2D collision) {
       if (collision.gameObject.CompareTag("Ground")) {
          animator.SetBool("isJumping", false);
          animator.SetBool("isDoubleJumping", false);
          timesJumped=0;
       }
    }

    void OnCollisionStay2D(Collision2D collision ) {
        if (collision.gameObject.CompareTag("Ground")) {
            for (int i=0;i<collision.contactCount;i++) {
            if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) <45f) {
                isGrounded=true;
            }
          }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded=false;
        }
    }

    private void flip() {
        Vector3 newLocalScale=transform.localScale;
        newLocalScale.x*=-1f;
        transform.localScale=newLocalScale;
        isFacingRight=!isFacingRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            isDead = true;
            animator.SetBool("isDead", true);
        }
    }
}
