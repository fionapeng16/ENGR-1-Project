// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Net.WebSockets;
// using UnityEngine.InputSystem;

// public class PlayerMovementScript : MonoBehaviour
// {
//     // Start is called before the first frame update
//     Rigidbody2D rb;
//     [SerializeField] float speed=1f;
//     [SerializeField] float jumpHeight=3f;
//     float direction=0;
//     bool isGrounded=true;
//     int timesJumped=0;
//     bool isFacingRight=true;
//     bool isDead = false;
//     private Vector3 spawnPoint;
//     Animator animator;
//     int timer = 0;
//     void Start()
//     {
//         rb=GetComponent<Rigidbody2D>();
//         spawnPoint = transform.position;
//         animator = GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {        
//         if (isDead && timer==1000) {
//             transform.position = spawnPoint; // Reset to the starting position
//             rb.linearVelocity = Vector2.zero; // Stop movement
//             isDead = false;
//             animator.SetBool("isDead", false);
//             timer = 0;
//         }
//         else if (isDead) {
//             timer++;
//             rb.linearVelocity=new Vector2(0, 0);
//             return;
//         }

//         Move(direction);
//         if ((isFacingRight && direction==-1) || (!isFacingRight && direction==1)) {
//             flip();
//         }
//     }
//     void OnMove(InputValue value) {
//         float v = value.Get<float>();
//         direction=v;
//     }

//     void Move(float dir) {
//         animator.SetBool("isRunning", dir!=0);
//         rb.linearVelocity=new Vector2(dir*speed, rb.linearVelocity.y);
//     }

//     void OnJump() {
//         if (isGrounded || timesJumped==1) {
//             Jump();
//         }
//         else {
//             timesJumped=0;
//         }
        
//     }

//     void Jump() {
//         timesJumped++;
//         isGrounded = false;
//         animator.SetBool("isJumping", !isGrounded);
//         animator.SetBool("isDoubleJumping", timesJumped==2);
//         rb.linearVelocity=new Vector2(rb.linearVelocity.x, jumpHeight);
//     }
//     void OnCollisionEnter2D(Collision2D collision) {
//        if (collision.gameObject.CompareTag("Ground")) {
//           animator.SetBool("isJumping", false);
//           animator.SetBool("isDoubleJumping", false);
//           timesJumped=0;
//        }
//     }

//     void OnCollisionStay2D(Collision2D collision ) {
//         if (collision.gameObject.CompareTag("Ground")) {
//             for (int i=0;i<collision.contactCount;i++) {
//             if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) <45f) {
//                 isGrounded=true;
//             }
//           }
//         }
//     }

//     void OnCollisionExit2D(Collision2D collision) {
//         if (collision.gameObject.CompareTag("Ground")) {
//             isGrounded=false;
//         }
//     }

//     private void flip() {
//         Vector3 newLocalScale=transform.localScale;
//         newLocalScale.x*=-1f;
//         transform.localScale=newLocalScale;
//         isFacingRight=!isFacingRight;
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Enemy"))
//         {
//             isDead = true;
//             animator.SetBool("isDead", true);
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    // Movement variables
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpHeight = 3f;
    float direction = 0;
    bool isFacingRight = true;

    // Grounded and jumping variables
    bool isGrounded = false;
    int timesJumped = 0;

    // Death and respawn variables
    bool isDead = false;
    private Vector3 spawnPoint;

    // Audio variables
    public AudioClip jumpSound;
    public AudioClip runSound;
    public AudioClip deathSound;
    private AudioSource audioSource; // For jump & death
    private AudioSource runAudioSource; // Separate source for running

    // Animator for controlling animations
    Animator animator;

    // Clamping variables for X position (set this in the Unity editor per level)
    public float minX = -6.96f;
    public float maxX = 38.56f;
    int timer = 0;

    void Start()
    {
        // Get or Add AudioSources
        audioSource = GetComponent<AudioSource>();
        runAudioSource = gameObject.AddComponent<AudioSource>(); // Add second AudioSource
        runAudioSource.clip = runSound;
        runAudioSource.loop = true; // Running sound should loop
        timer = 0;
        // Initial spawn point and animator setup
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Handling death and respawn logic
        if (isDead)
        {
            HandleDeath();
        }
        else
        {
            // Normal movement and flipping logic
            Move(direction);
            FlipIfNeeded();
        }
    }

    void FixedUpdate()
    {
        // Move the player and clamp position within bounds
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.linearVelocity;
        velocity.x = direction * speed;
        rb.linearVelocity = velocity;

        // Clamp the player's position using the adjustable min/maxX
        ClampPosition();
    }

    void OnMove(InputValue value)
    {
        // Read movement input
        direction = value.Get<float>();
    }

    void Move(float dir)
    {
        // Move the player horizontally
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
        animator.SetBool("isRunning", dir!=0);

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
            runAudioSource.Stop();
        }
    }

    void OnJump()
    {
        // Handle jumping logic and play jump sound
        if (isGrounded || timesJumped == 1)
        {
            PerformJump();
            audioSource.PlayOneShot(jumpSound); // Play the jump sound here
        }
        else
        {
            return;
        }
    }

    void PerformJump()
    {
        // Perform the jump and update state
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        timesJumped++; // Increment the jump count after a jump

        // Update animations for jumping
        animator.SetBool("isJumping", true);
        animator.SetBool("isDoubleJumping", timesJumped == 2);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collisions with ground and enemies
        if (collision.gameObject.CompareTag("Ground"))
        {
            HandleGroundCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("DeathZone"))
        {
            HandleDeath();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f)
                {
                    isGrounded = true; // Player is still grounded
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Player is no longer grounded
        }
    }

    void HandleGroundCollision(Collision2D collision)
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isDoubleJumping", false);
        timesJumped=0;
        // Check if grounded based on collision angles
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f)
            {
                isGrounded = true;
            }
        }
    }

    void ClampPosition()
    {
        // Clamp player position within specified bounds (using the public minX and maxX)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float clampedX = Mathf.Clamp(rb.position.x, minX, maxX);
        rb.position = new Vector2(clampedX, rb.position.y);
    }

    void FlipIfNeeded()
    {
        // Flip player sprite based on movement direction
        if ((isFacingRight && direction < 0) || (!isFacingRight && direction > 0))
        {
            Flip();
        }
    }

    void Flip()
    {
        // Flip player sprite horizontally
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void HandleDeath()
    {
        // Ensure the death animation is triggered once when the cat dies
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            // Play the death sound
            if (audioSource && deathSound)
            {
                audioSource.PlayOneShot(deathSound);
            }
            StartCoroutine(DeathCoroutine());
        }
    }

    IEnumerator DeathCoroutine()
    {
        // Wait for a short amount of time (e.g., 1 second) before respawning
        yield return new WaitForSeconds(1f); // Adjust this time based on how long you want the death animation to last

        // After the wait, respawn the player
        transform.position = spawnPoint;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero; // Stop movement
        isDead = false;
        animator.SetBool("isDead", false); // Reset death animation
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle triggers for enemy and death zones
        if (collision.CompareTag("Enemy") || collision.CompareTag("DeathZone"))
        {
            HandleDeath();
        }
    }

    /*void Respawn()
    {
        // Reset player position to spawn point
        if (audioSource && deathSound)
        {
            audioSource.PlayOneShot(deathSound);
        }

        transform.position = spawnPoint;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }*/
}
