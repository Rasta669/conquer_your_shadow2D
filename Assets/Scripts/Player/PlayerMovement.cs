using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float positionRecordInterval = 0.1f;
    public float climbSpeed = 3f; // Climbing speed
    public float topLadderYPosition = 10f; // Set the Y position where the top of the ladder is
    public float bottomLadderYPosition = 0f; // Set the Y position where the bottom of the ladder is

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private ShadowFollower shadowFollower;
    private static GameManager gameManager;
    private CharacterAnimator characterAnimator;
    private bool isDead = false;
    private float lastDirection = 1f; // Tracks last movement direction

    private bool isClimbing = false;
    private bool canClimb = false;
    private float climbAnimSpeed = 1f; // Store normal climbing animation speed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        gameManager = GameManager.Instance;
        characterAnimator = GetComponent<CharacterAnimator>();

        shadowFollower = FindFirstObjectByType<ShadowFollower>();
        StartCoroutine(RecordPosition());
    }

    void Update()
    {
        if (isClimbing)
        {
            Climb();
        }
        else
        {
            // Normal movement when not climbing
            if (!isDashing && !isClimbing)
            {
                Move();
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) && !isClimbing && isGrounded) // Normal jump on the ground
            {
                Jump();
            }

            // Handle Dash
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }

            // Start climbing if near ladder and "W" is pressed
            if (canClimb && Input.GetKey(KeyCode.W))
            {
                StartClimbing();
            }
            // Stop climbing if "W" is released
            else if (canClimb && !Input.GetKey(KeyCode.W))
            {
                StopClimbing();
            }
        }
    }


    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftControl) ? walkSpeed : runSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Store last movement direction for dash and flipping
        if (moveInput != 0)
        {
            lastDirection = Mathf.Sign(moveInput);
            transform.localScale = new Vector3(lastDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Trigger running animation if moving horizontally
        if (moveInput != 0 && isGrounded)
        {
            characterAnimator.SetIsRunning(true);
        }
        else
        {
            characterAnimator.SetIsRunning(false);
        }
    }


    void Jump()
    {
        if (isClimbing)
        {
            // If jumping while climbing, stop climbing and apply jump force
            StopClimbing(); // Stop climbing immediately
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply the jump force
            characterAnimator.SetIsJumping(true); // Play jump animation
        }
        else if (isGrounded)
        {
            // Normal jump when grounded
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply the jump force
            characterAnimator.SetIsJumping(true); // Play jump animation
            isGrounded = false;
        }
    }
    IEnumerator Dash()
    {
        isDashing = true;
        characterAnimator.PlayDashAnimation(); // Call the Dash animation

        rb.linearVelocity = new Vector2(lastDirection * dashSpeed, rb.linearVelocity.y);
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    IEnumerator RecordPosition()
    {
        while (true)
        {
            if (shadowFollower != null)
            {
                bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0 && isGrounded;
                bool jumped = !isGrounded && rb.linearVelocity.y > 0.1f;

                // Store player's position, movement, and facing direction
                shadowFollower.StorePosition(rb.position, rb.linearVelocity, jumped, isDashing, isRunning, lastDirection);
            }
            yield return new WaitForSeconds(positionRecordInterval);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            characterAnimator.SetIsJumping(false); // Reset jump animation on landing
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero; // Stop movement
        characterAnimator.PlayDeathAnimation(); // Trigger death animation
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(1.0f); // Wait for death animation
        Time.timeScale = 1f; // Ensure UI updates properly
        gameManager.GameOver();
    }

    void CheckGrounded()
    {
        float rayLength = 0.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        isGrounded = hit.collider != null;

        Debug.DrawRay(transform.position, Vector2.down * rayLength, isGrounded ? Color.green : Color.red);
        Debug.Log($"Grounded: {isGrounded} | Hit: {(hit.collider != null ? hit.collider.gameObject.name : "None")}");
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0; // Disable gravity while climbing
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Stop vertical movement

        // Start the climbing animation and resume it by setting speed to normal
        characterAnimator.SetIsClimbing(true);
        characterAnimator.SetAnimatorSpeed(climbAnimSpeed); // Resume normal animation speed
    }

    void PauseClimbing()
    {
        // Pause the climbing animation by setting the speed to 0
        characterAnimator.SetAnimatorSpeed(0); // This pauses the climbing animation
    }

   

    void Climb()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (verticalInput != 0)
        {
            // Move the player vertically on the ladder
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
            characterAnimator.SetIsClimbing(true); // Keep climbing animation
        }
        else
        {
            // Pause the climbing animation (still on the ladder, just not moving)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Stop vertical movement
            characterAnimator.SetIsClimbing(true); // Keep the climbing animation active
        }

        // Check if the player has reached the top or bottom of the ladder
        if (transform.position.y >= topLadderYPosition) // Adjust this based on the ladder top position
        {
            StopClimbing();
        }

        if (transform.position.y <= bottomLadderYPosition) // Adjust this based on the ladder bottom position
        {
            StopClimbing();
        }
    }


    void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2f; // Restore gravity when not climbing
        characterAnimator.SetIsClimbing(false); // Stop climbing animation
        characterAnimator.SetAnimatorSpeed(1); // Reset animation speed to normal if not paused
    }

    void StartClimbingDown()
    {
        // Handle climbing down, similar to normal climbing logic but with downward movement
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -climbSpeed);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimb = true; // Allow climbing
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimb = false;
            isClimbing = false;
            rb.gravityScale = 2f; // Restore gravity when leaving ladder
            characterAnimator.SetIsClimbing(false);
        }
    }


}
