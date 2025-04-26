//using UnityEngine;
//using System.Collections;

//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Animation")]
//    public float walkSpeed = 3f;
//    public float runSpeed = 6f;
//    public float jumpForce = 10f;
//    public float dashSpeed = 12f;
//    public float dashDuration = 0.2f;
//    public float positionRecordInterval = 0.1f;
//    public float climbSpeed = 3f; // Climbing speed
//    public float topLadderYPosition = 10f; // Set the Y position where the top of the ladder is
//    public float bottomLadderYPosition = 0f; // Set the Y position where the bottom of the ladder is

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isDashing;
//    private ShadowFollower shadowFollower;
//    private static NewGameManager gameManager;
//    private CharacterAnimator characterAnimator;
//    private bool isDead = false;
//    private float lastDirection = 1f; // Tracks last movement direction

//    private bool isClimbing = false;
//    private bool canClimb = false;
//    private float climbAnimSpeed = 1f; // Store normal climbing animation speed

//    [Header("Waterfall")]
//    // Waterfall sound variables
//    public Transform waterfall; // Assign the waterfall GameObject's transform in Inspector
//    public float waterfallRange = 5f; // Max distance where sound starts fading in/out

//    [Header("Particle Effects")]
//    public ParticleSystem fallParticle;
//    public ParticleSystem bloodParticle;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.bodyType = RigidbodyType2D.Dynamic;
//        rb.gravityScale = 2f;
//        gameManager = NewGameManager.Instance;
//        characterAnimator = GetComponent<CharacterAnimator>();

//        shadowFollower = FindFirstObjectByType<ShadowFollower>();
//        StartCoroutine(RecordPosition());
//    }

//    void Update()
//    {
//        if (isClimbing)
//        {
//            Climb();
//        }
//        else
//        {
//            // Normal movement when not climbing
//            if (!isDashing && !isClimbing)
//            {
//                Move();
//            }

//            // Jumping
//            if (Input.GetKeyDown(KeyCode.Space) && !isClimbing && isGrounded)
//            {
//                Jump();
//            }

//            // Handle Dash
//            if (Input.GetKey(KeyCode.LeftShift))
//            {
//                StartCoroutine(Dash());
//            }

//            // Start climbing if near ladder and "W" is pressed
//            if (canClimb && Input.GetKey(KeyCode.W))
//            {
//                StartClimbing();
//            }
//            // Stop climbing if "W" is released
//            else if (isClimbing && !Input.GetKey(KeyCode.W))
//            {
//                PauseClimbing();
//            }
//        }
//        UpdateWaterfallSound();
//    }

//    void Move()
//    {
//        float moveInput = Input.GetAxisRaw("Horizontal");
//        float speed = Input.GetKey(KeyCode.LeftControl) ? walkSpeed : runSpeed;

//        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

//        // Store last movement direction for dash and flipping
//        if (moveInput != 0)
//        {
//            lastDirection = Mathf.Sign(moveInput);
//            transform.localScale = new Vector3(lastDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
//        }

//        // Trigger running animation if moving horizontally
//        if (moveInput != 0 && isGrounded)
//        {
//            characterAnimator.SetIsRunning(true);
//        }
//        else
//        {
//            characterAnimator.SetIsRunning(false);
//        }
//    }

//    void Jump()
//    {
//        if (isClimbing)
//        {
//            // If jumping while climbing, stop climbing and apply jump force
//            StopClimbing();
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//            characterAnimator.SetIsJumping(true);
//        }
//        else if (isGrounded)
//        {
//            // Normal jump when grounded
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//            characterAnimator.SetIsJumping(true);
//            isGrounded = false;
//        }
//    }

//    IEnumerator Dash()
//    {
//        isDashing = true;
//        characterAnimator.PlayDashAnimation();

//        rb.linearVelocity = new Vector2(lastDirection * dashSpeed, rb.linearVelocity.y);
//        yield return new WaitForSeconds(dashDuration);

//        isDashing = false;
//    }

//    IEnumerator RecordPosition()
//    {
//        while (true)
//        {
//            if (shadowFollower != null)
//            {
//                bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0 && isGrounded;
//                bool jumped = !isGrounded && rb.linearVelocity.y > 0.1f;
//                bool climbing = isClimbing;

//                shadowFollower.StorePosition(rb.position, rb.linearVelocity, jumped, isDashing, isRunning, lastDirection, climbing);
//            }
//            yield return new WaitForSeconds(positionRecordInterval);
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            fallParticle.Play();
//            isGrounded = true;
//            characterAnimator.SetIsJumping(false);
//        }
//        else if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Shadow"))
//        {
//            Die();
//        }
//    }

//    public bool IsGrounded()
//    {
//        return isGrounded;
//    }

//    public bool IsDead()
//    {
//        return isDead;
//    }

//    private void Die()
//    {
//        if (isDead) return;

//        isDead = true;
//        rb.linearVelocity = Vector2.zero;
//        bloodParticle.Play();
//        characterAnimator.PlayDeathAnimation();
//        StartCoroutine(HandleGameOver());
//    }

//    private IEnumerator HandleGameOver()
//    {
//        yield return new WaitForSeconds(1.0f);
//        Time.timeScale = 1f;
//        gameManager.GameOver();
//    }

//    void CheckGrounded()
//    {
//        float rayLength = 0.3f;
//        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

//        isGrounded = hit.collider != null;

//        Debug.DrawRay(transform.position, Vector2.down * rayLength, isGrounded ? Color.green : Color.red);
//        Debug.Log($"Grounded: {isGrounded} | Hit: {(hit.collider != null ? hit.collider.gameObject.name : "None")}");
//    }

//    void StartClimbing()
//    {
//        isClimbing = true;
//        rb.gravityScale = 0;
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
//        characterAnimator.SetIsClimbing(true);
//        characterAnimator.SetAnimatorSpeed(climbAnimSpeed);
//    }

//    void PauseClimbing()
//    {
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
//        characterAnimator.SetAnimatorSpeed(0);
//    }

//    void Climb()
//    {
//        float verticalInput = Input.GetAxisRaw("Vertical");

//        if (verticalInput > 0) // Holding "W"
//        {
//            // Smoothly move up the ladder
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbSpeed);
//            characterAnimator.SetIsClimbing(true);
//            characterAnimator.SetAnimatorSpeed(climbAnimSpeed * verticalInput); // Adjust animation speed based on input

//            // Loop back to bottom when reaching the top
//            if (transform.position.y >= topLadderYPosition)
//            {
//                transform.position = new Vector3(transform.position.x, bottomLadderYPosition, transform.position.z);
//            }
//        }
//        else if (verticalInput < 0) // Holding "S" to move down
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
//            characterAnimator.SetIsClimbing(true);
//            characterAnimator.SetAnimatorSpeed(climbAnimSpeed * -verticalInput);

//            // Loop back to top when reaching the bottom
//            if (transform.position.y <= bottomLadderYPosition)
//            {
//                transform.position = new Vector3(transform.position.x, bottomLadderYPosition, transform.position.z);
//            }
//        }
//        else
//        {
//            // Pause movement and animation when no vertical input
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
//            characterAnimator.SetIsClimbing(true);
//            characterAnimator.SetAnimatorSpeed(0);
//        }
//    }

//    void StopClimbing()
//    {
//        isClimbing = false;
//        rb.gravityScale = 2f;
//        characterAnimator.SetIsClimbing(false);
//        characterAnimator.SetAnimatorSpeed(1);
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Ladder"))
//        {
//            canClimb = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Ladder"))
//        {
//            canClimb = false;
//            isClimbing = false;
//            rb.gravityScale = 2f;
//            characterAnimator.SetIsClimbing(false);
//        }
//    }

//    private void UpdateWaterfallSound()
//    {
//        if (waterfall == null || AudioManager.Instance == null || AudioManager.Instance.waterSound == null)
//            return;

//        float distance = Mathf.Abs(transform.position.x - waterfall.position.x);
//        float volume = Mathf.Clamp01(1 - (distance / waterfallRange));

//        AudioManager.Instance.waterSound.volume = volume;

//        if (!AudioManager.Instance.waterSound.isPlaying && volume > 0)
//        {
//            AudioManager.Instance.waterSound.Play();
//        }
//        else if (volume == 0)
//        {
//            AudioManager.Instance.waterSound.Stop();
//        }
//    }
//}

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animation")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float positionRecordInterval = 0.1f;
    public float climbSpeed = 3f;
    public float topLadderYPosition = 10f;
    public float bottomLadderYPosition = 0f;

    [Header("Health System")]
    public int maxHealth = 3; // Maximum health (3 hearts)
    private int currentHealth; // Current health

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private ShadowFollower shadowFollower;
    private static NewGameManager gameManager;
    private CharacterAnimator characterAnimator;
    private bool isDead = false;
    private float lastDirection = 1f;

    private bool isClimbing = false;
    private bool canClimb = false;
    private float climbAnimSpeed = 1f;

    [Header("Waterfall")]
    public Transform waterfall;
    public float waterfallRange = 5f;

    [Header("Particle Effects")]
    public ParticleSystem fallParticle;
    public ParticleSystem bloodParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        gameManager = NewGameManager.Instance;
        characterAnimator = GetComponent<CharacterAnimator>();

        shadowFollower = FindFirstObjectByType<ShadowFollower>();
        currentHealth = maxHealth; // Initialize health
        UpdateHealthUI(); // Update UI on start
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
            if (!isDashing && !isClimbing)
            {
                Move();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isClimbing && isGrounded)
            {
                Jump();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }

            if (canClimb && Input.GetKey(KeyCode.W))
            {
                StartClimbing();
            }
            else if (isClimbing && !Input.GetKey(KeyCode.W))
            {
                PauseClimbing();
            }
        }
        UpdateWaterfallSound();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftControl) ? walkSpeed : runSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            lastDirection = Mathf.Sign(moveInput);
            transform.localScale = new Vector3(lastDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

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
            StopClimbing();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            characterAnimator.SetIsJumping(true);
        }
        else if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            characterAnimator.SetIsJumping(true);
            isGrounded = false;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        characterAnimator.PlayDashAnimation();

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
                bool climbing = isClimbing;

                shadowFollower.StorePosition(rb.position, rb.linearVelocity, jumped, isDashing, isRunning, lastDirection, climbing);
            }
            yield return new WaitForSeconds(positionRecordInterval);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            fallParticle.Play();
            isGrounded = true;
            characterAnimator.SetIsJumping(false);
        }
        else if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Shadow"))
        {
            TakeDamage();
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

    private void TakeDamage()
    {
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - 1); // Reduce health
        UpdateHealthUI(); // Update UI
        bloodParticle.Play(); // Play damage effect

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Optional: Add a brief invulnerability period or visual feedback
            characterAnimator.PlayHurtAnimation(); // Assuming you have a hurt animation
        }
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        characterAnimator.PlayDeathAnimation();
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 1f;
        gameManager.GameOver();
    }

    private void UpdateHealthUI()
    {
        if (gameManager != null)
        {
            gameManager.UpdatePlayerHealth(currentHealth); // Notify GameManager to update UI
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthUI();
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
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        characterAnimator.SetIsClimbing(true);
        characterAnimator.SetAnimatorSpeed(climbAnimSpeed);
    }

    void PauseClimbing()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        characterAnimator.SetAnimatorSpeed(0);
    }

    void Climb()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (verticalInput > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbSpeed);
            characterAnimator.SetIsClimbing(true);
            characterAnimator.SetAnimatorSpeed(climbAnimSpeed * verticalInput);

            if (transform.position.y >= topLadderYPosition)
            {
                transform.position = new Vector3(transform.position.x, bottomLadderYPosition, transform.position.z);
            }
        }
        else if (verticalInput < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
            characterAnimator.SetIsClimbing(true);
            characterAnimator.SetAnimatorSpeed(climbAnimSpeed * -verticalInput);

            if (transform.position.y <= bottomLadderYPosition)
            {
                transform.position = new Vector3(transform.position.x, bottomLadderYPosition, transform.position.z);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            characterAnimator.SetIsClimbing(true);
            characterAnimator.SetAnimatorSpeed(0);
        }
    }

    void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2f;
        characterAnimator.SetIsClimbing(false);
        characterAnimator.SetAnimatorSpeed(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimb = false;
            isClimbing = false;
            rb.gravityScale = 2f;
            characterAnimator.SetIsClimbing(false);
        }
    }

    private void UpdateWaterfallSound()
    {
        if (waterfall == null || AudioManager.Instance == null || AudioManager.Instance.waterSound == null)
            return;

        float distance = Mathf.Abs(transform.position.x - waterfall.position.x);
        float volume = Mathf.Clamp01(1 - (distance / waterfallRange));

        AudioManager.Instance.waterSound.volume = volume;

        if (!AudioManager.Instance.waterSound.isPlaying && volume > 0)
        {
            AudioManager.Instance.waterSound.Play();
        }
        else if (volume == 0)
        {
            AudioManager.Instance.waterSound.Stop();
        }
    }
}