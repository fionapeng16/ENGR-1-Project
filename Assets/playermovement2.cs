using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using UnityEngine.InputSystem;

public class playermovement2 : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    [SerializeField] float speed=1f;
    [SerializeField] float jumpHeight=3f;
    float direction=0;
    bool isGrounded=false;
    int timesJumped=0;
    bool isFacingRight=true;
    private Vector3 spawnPoint;    public AudioClip jumpSound;
    public AudioClip runSound;
    public AudioClip deathSound;
    private AudioSource audioSource; // For jump & death
    private AudioSource runAudioSource; // Separate source for running

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        // Get or Add AudioSources
        audioSource = GetComponent<AudioSource>();
        runAudioSource = gameObject.AddComponent<AudioSource>(); // Add second AudioSource
        runAudioSource.clip = runSound;
        runAudioSource.loop = true; // Running sound should loop
    }

    // Update is called once per frame
    void Update()
    {
        Move(direction);
        if ((isFacingRight && direction==-1) || (!isFacingRight && direction==1)) {
            flip();
        }
    }

    void FixedUpdate()
    {
        // Get current velocity
        Vector2 velocity = rb.linearVelocity;

        // Move the player normally
        velocity.x = direction * speed;

        // Apply the velocity to the Rigidbody
        rb.linearVelocity = velocity;

        // Clamp the player's position within the bounds
        float clampedX = Mathf.Clamp(rb.position.x, -5.66f, 38.56f);
        rb.position = new Vector2(clampedX, rb.position.y);
    }
    void OnMove(InputValue value) {
        float v = value.Get<float>();
        direction=v;
    }

    void Move(float dir)
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        // Play running sound only when moving and grounded
        if (dir != 0 && isGrounded)
        {
            if (!runAudioSource.isPlaying)
            {
                runAudioSource.Play();
            }
        }
        else
        {
            // Stop the running sound when the player stops moving
            runAudioSource.Stop();
        }
    }

    void OnJump() {
        if (isGrounded || timesJumped==1) {
            Jump();
            audioSource.PlayOneShot(jumpSound);
        }
                  
        else {
            timesJumped=0;
        }
        
    }

    void Jump() {
        timesJumped++;
        rb.linearVelocity=new Vector2(rb.linearVelocity.x, jumpHeight);
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

void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Enemy") || collision.CompareTag("DeathZone"))
    {
        Respawn();
    }
}

private void Respawn()
{
    audioSource.PlayOneShot(deathSound);
    transform.position = spawnPoint; // Reset to the starting position
    rb.linearVelocity = Vector2.zero; // Stop movement
}

}
