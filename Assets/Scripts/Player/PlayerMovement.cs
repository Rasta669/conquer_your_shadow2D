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

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.bodyType = RigidbodyType2D.Dynamic; // Ensure gravity affects the player
//        rb.gravityScale = 2f; // Adjust gravity for better feel

//        shadowFollower = FindFirstObjectByType<ShadowFollower>(); // Updated API
//        StartCoroutine(RecordPosition());
//    }

//    void Update()
//    {
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
//    }

//    void Jump()
//    {
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply force for jumping
//        isGrounded = false;
//    }

//    IEnumerator Dash()
//    {
//        isDashing = true;
//        float direction = Mathf.Sign(rb.linearVelocity.x);
//        rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y); // Set high speed for dash

//        yield return new WaitForSeconds(dashDuration);

//        isDashing = false;
//    }

//    IEnumerator RecordPosition()
//    {
//        while (true)
//        {
//            if (shadowFollower != null)
//            {
//                shadowFollower.StorePosition(rb.position);
//            }
//            yield return new WaitForSeconds(positionRecordInterval);
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//        }
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
    private static GameManager gameManager; // Cache singleton reference

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        gameManager = GameManager.Instance; // Cache reference to prevent GC

        shadowFollower = FindFirstObjectByType<ShadowFollower>();
        StartCoroutine(RecordPosition());
    }

    void Update()
    {
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
    }

    void Jump()
    {
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
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! Player hit an obstacle.");
            gameManager.GameOver(); // Use cached reference
        }
    }
}
