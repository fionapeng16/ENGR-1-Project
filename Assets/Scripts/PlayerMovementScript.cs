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
    bool isGrounded=false;
    int timesJumped=0;
    bool isFacingRight=true;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        rb.velocity=new Vector2(dir*speed, rb.velocity.y);
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
        rb.velocity=new Vector2(rb.velocity.x, jumpHeight);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
    }

    void OnCollisionStay2D(Collision2D collision ) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded=false; 
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
}
