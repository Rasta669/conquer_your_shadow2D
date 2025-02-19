////using UnityEngine;
////using System.Collections;

////public class PlayerMovement : MonoBehaviour
////{
////    public float walkSpeed = 3f;
////    public float runSpeed = 6f;
////    public float jumpForce = 10f;
////    public float dashSpeed = 12f;
////    public float dashDuration = 0.2f;
////    public float positionRecordInterval = 0.1f;

////    private Rigidbody2D rb;
////    private bool isGrounded;
////    private bool isDashing;
////    private ShadowFollower shadowFollower;
////    private static GameManager gameManager;
////    private CharacterAnimator characterAnimator;
////    private bool isDead = false;

////    void Start()
////    {
////        rb = GetComponent<Rigidbody2D>();
////        rb.bodyType = RigidbodyType2D.Dynamic;
////        rb.gravityScale = 2f;
////        gameManager = GameManager.Instance;
////        characterAnimator = GetComponent<CharacterAnimator>();

////        shadowFollower = FindFirstObjectByType<ShadowFollower>();
////        StartCoroutine(RecordPosition());
////    }

////    void Update()
////    {
////        if (isDead || Time.timeScale == 0) return;

////        if (!isDashing)
////        {
////            Move();
////        }

////        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
////        {
////            Jump();
////        }

////        if (Input.GetKeyDown(KeyCode.LeftShift))
////        {
////            StartCoroutine(Dash());
////        }
////    }

////    void Move()
////    {
////        float moveInput = Input.GetAxisRaw("Horizontal");
////        float speed = Input.GetKey(KeyCode.LeftControl) ? walkSpeed : runSpeed;

////        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

////        // Trigger running animation if moving horizontally
////        if (moveInput != 0 && isGrounded)
////        {
////            characterAnimator.SetIsRunning(true);
////        }
////        else
////        {
////            characterAnimator.SetIsRunning(false);
////        }
////    }

////    void Jump()
////    {
////        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
////        isGrounded = false;

////        // Trigger jump animation
////        characterAnimator.SetIsJumping(true);
////    }

////    IEnumerator Dash()
////    {
////        isDashing = true;
////        float direction = Mathf.Sign(rb.linearVelocity.x);
////        rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);

////        yield return new WaitForSeconds(dashDuration);

////        isDashing = false;
////    }

////    IEnumerator RecordPosition()
////    {
////        while (true)
////        {
////            if (shadowFollower != null)
////            {
////                shadowFollower.StorePosition(rb.position, rb.linearVelocity, !isGrounded, isDashing);
////            }
////            yield return new WaitForSeconds(positionRecordInterval);
////        }
////    }

////    private void OnCollisionEnter2D(Collision2D collision)
////    {
////        if (collision.gameObject.CompareTag("Ground"))
////        {
////            isGrounded = true;
////            characterAnimator.SetIsJumping(false); // Reset jump animation on landing
////        }
////        else if (collision.gameObject.CompareTag("Obstacle"))
////        {
////            Die();
////        }
////    }

////    public bool IsGrounded()
////    {
////        return isGrounded;
////    }

////    public bool IsDead()
////    {
////        return isDead;
////    }

////    private void Die()
////    {
////        if (isDead) return;

////        isDead = true;
////        rb.linearVelocity = Vector2.zero; // Stop movement
////        characterAnimator.PlayDeathAnimation();
////        StartCoroutine(HandleGameOver());
////    }

////    private IEnumerator HandleGameOver()
////    {
////        yield return new WaitForSeconds(1.0f); // Adjust time to match your death animation duration
////        gameManager.GameOver();
////    }
////}


//using UnityEngine;
//using System.Collections;

//public class PlayerMovement : MonoBehaviour
//{
//    public float walkSpeed = 3f;
//    public float runSpeed = 6f;
//    public float jumpForce = 10f;
//    public float dashSpeed = 12f;
//    public float dashDuration = 0.2f;
//    public float positionRecordInterval = 0.1f;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isDashing;
//    private ShadowFollower shadowFollower;
//    private static GameManager gameManager;
//    private CharacterAnimator characterAnimator;
//    private bool isDead = false;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.bodyType = RigidbodyType2D.Dynamic;
//        rb.gravityScale = 2f;
//        gameManager = GameManager.Instance;
//        characterAnimator = GetComponent<CharacterAnimator>();

//        shadowFollower = FindFirstObjectByType<ShadowFollower>();
//        StartCoroutine(RecordPosition());
//    }

//    void Update()
//    {
//        if (isDead || Time.timeScale == 0) return;

//        if (!isDashing)
//        {
//            Move();
//        }

//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            Jump();
//        }

//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            StartCoroutine(Dash());
//        }
//    }

//    void Move()
//    {
//        float moveInput = Input.GetAxisRaw("Horizontal");
//        float speed = Input.GetKey(KeyCode.LeftControl) ? walkSpeed : runSpeed;

//        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

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
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//        isGrounded = false;

//        // Trigger jump animation
//        characterAnimator.SetIsJumping(true);
//    }

//    IEnumerator Dash()
//    {
//        isDashing = true;
//        float direction = Mathf.Sign(rb.linearVelocity.x);
//        rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);

//        yield return new WaitForSeconds(dashDuration);

//        isDashing = false;
//    }

//    IEnumerator RecordPosition()
//    {
//        while (true)
//        {
//            if (shadowFollower != null)
//            {
//                shadowFollower.StorePosition(rb.position, rb.linearVelocity, !isGrounded, isDashing);
//            }
//            yield return new WaitForSeconds(positionRecordInterval);
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//            characterAnimator.SetIsJumping(false); // Reset jump animation on landing
//        }
//        else if (collision.gameObject.CompareTag("Obstacle"))
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
//        rb.linearVelocity = Vector2.zero; // Stop movement
//        characterAnimator.PlayDeathAnimation(); // Trigger death animation
//        StartCoroutine(HandleGameOver());
//    }

//    private IEnumerator HandleGameOver()
//    {
//        yield return new WaitForSeconds(1.0f); // Adjust time to match your death animation duration
//        gameManager.GameOver();
//    }
//}


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
        // Trigger jump animation regardless of horizontal movement
        characterAnimator.SetIsJumping(true);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float direction = Mathf.Sign(rb.linearVelocity.x);
        rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    IEnumerator RecordPosition()
    {
        while (true)
        {
            if (shadowFollower != null)
            {
                shadowFollower.StorePosition(rb.position, rb.linearVelocity, !isGrounded, isDashing);
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
        yield return new WaitForSeconds(1.0f); // Adjust time to match your death animation duration
        gameManager.GameOver();
    }
}

