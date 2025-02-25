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

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private ShadowFollower shadowFollower;
    private static GameManager gameManager;
    private CharacterAnimator characterAnimator;
    private bool isDead = false;
    private float lastDirection = 1f; // Tracks last movement direction

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
        //CheckGrounded();
        if (isDead || Time.timeScale == 0) return;

        if (!isDashing)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }   
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftControl) ? walkSpeed : runSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Store last movement direction for dash
        if (moveInput != 0)
        {
            lastDirection = Mathf.Sign(moveInput);
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

    //void Jump()
    //{
    //    // Trigger jump animation
    //    characterAnimator.SetIsJumping(true);
    //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    //    isGrounded = false;
    //}

    void Jump()
    {
        if (isGrounded) // Only allow jumping if grounded
        {
            characterAnimator.SetIsJumping(true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Use velocity instead of linearVelocity for accurate physics
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




    //IEnumerator RecordPosition()
    //{
    //    while (true)
    //    {
    //        if (shadowFollower != null)
    //        {
    //            bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0 && isGrounded;
    //            bool jumped = !isGrounded && rb.linearVelocity.y > 0.1f; // Ensures actual upward movement

    //            shadowFollower.StorePosition(rb.position, rb.linearVelocity, jumped, isDashing, isRunning);
    //        }
    //        yield return new WaitForSeconds(positionRecordInterval);
    //    }
    //}

    IEnumerator RecordPosition()
    {
        while (true)
        {
            if (shadowFollower != null)
            {
                bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0 && isGrounded;
                bool jumped = !isGrounded && rb.linearVelocity.y > 0.1f;

                shadowFollower.StorePosition(rb.position, rb.linearVelocity, jumped, isDashing, isRunning);
            }
            yield return new WaitForSeconds(positionRecordInterval);
        }
    }




    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //        characterAnimator.SetIsJumping(false); // Reset jump animation on landing
    //    }
    //    else if (collision.gameObject.CompareTag("Obstacle"))
    //    {
    //        Die();
    //    }
    //}

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
        Vector2 feetPosition = new Vector2(transform.position.x, transform.position.y - 0.3f); // Adjust if needed
        float checkRadius = 0.2f;
        isGrounded = Physics2D.OverlapCircle(feetPosition, checkRadius, LayerMask.GetMask("Ground"));

        Debug.DrawRay(transform.position, Vector2.down * 0.5f, isGrounded ? Color.green : Color.red);
    }

}

