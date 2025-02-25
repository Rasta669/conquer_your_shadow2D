//using UnityEngine;
//using System.Collections;

//public class ShadowFollower : MonoBehaviour
//{
//    public float delayTime = 1f;
//    public float interpolation_factor = 10f;
//    private Rigidbody2D rb;

//    private const int maxStoredPositions = 100; // Prevents memory overflow
//    private MovementData[] movementHistory = new MovementData[maxStoredPositions];
//    private int currentIndex = 0, totalStored = 0;

//    private bool isReplaying = false;
//    private static GameManager gameManager; // Cache singleton reference
//    private CharacterAnimator characterAnimator; // Reference to the player's animator
//    private bool isDead = false; // To check if the player is dead

//    private class MovementData
//    {
//        public Vector2 position;
//        public Vector2 velocity;
//        public bool jumped;
//        public bool dashed;
//        public bool isRunning;
//    }


//    //void Awake()
//    //{
//    //    rb = GetComponent<Rigidbody2D>();
//    //    rb.bodyType = RigidbodyType2D.Kinematic;
//    //    gameManager = GameManager.Instance; // Cache reference to prevent GC
//    //    characterAnimator = GetComponent<CharacterAnimator>(); // Find the CharacterAnimator in the scene

//    //    for (int i = 0; i < maxStoredPositions; i++)
//    //        movementHistory[i] = new MovementData(); // Preallocate objects
//    //}

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.bodyType = RigidbodyType2D.Kinematic;
//        gameManager = GameManager.Instance; // Cache reference to prevent GC
//        characterAnimator = GetComponent<CharacterAnimator>(); // Find the CharacterAnimator in the scene

//        for (int i = 0; i < maxStoredPositions; i++)
//            movementHistory[i] = new MovementData(); // Preallocate objects

//        //gameObject.SetActive(false); // Disable the shadow initially
//        //Debug.Log("Shadow is initially disabled.");
//    }

//    //void OnEnable()
//    //{
//    //    StartCoroutine(EnableShadowAfterDelay()); // Start coroutine to enable shadow after delay
//    //    Debug.Log("Starting coroutine to enable shadow after delay.");
//    //}

//    private IEnumerator EnableShadowAfterDelay()
//    {
//        Debug.Log($"Waiting for {delayTime} seconds to enable shadow...");
//        yield return new WaitForSeconds(delayTime);
//        gameObject.SetActive(true); // Enable the shadow after the delay
//        Debug.Log("Shadow is now enabled.");
//    }


//    public void StorePosition(Vector2 position, Vector2 velocity, bool jumped, bool dashed, bool isRunning)
//    {
//        Debug.Log("StorePosition called: " + position + ", Jumped: " + jumped + ", Dashed: " + dashed + ", Running: " + isRunning);

//        MovementData data = movementHistory[currentIndex];
//        data.position = position;
//        data.velocity = velocity;
//        data.jumped = jumped;
//        data.dashed = dashed;
//        data.isRunning = isRunning;

//        currentIndex = (currentIndex + 1) % maxStoredPositions;
//        totalStored = Mathf.Min(totalStored + 1, maxStoredPositions);

//        if (!isReplaying)
//        {
//            Debug.Log("Starting ReplayMovement Coroutine...");
//            StopCoroutine("ReplayMovement");
//            StartCoroutine("ReplayMovement");
//        }
//    }





//    //private IEnumerator ReplayMovement()
//    //{
//    //    isReplaying = true;
//    //    yield return new WaitForSeconds(delayTime);

//    //    int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions;
//    //    bool wasGrounded = true; // Track previous grounded state

//    //    while (totalStored > 0)
//    //    {
//    //        MovementData data = movementHistory[replayIndex];

//    //        rb.MovePosition(Vector2.Lerp(rb.position, data.position, Time.deltaTime * interpolation_factor));
//    //        rb.linearVelocity = (data.position - rb.position) * 10f; // Smooth movement



//    //        if (characterAnimator != null)
//    //        {
//    //            Debug.Log($"Replaying: Jumped: {data.jumped}, Running: {data.isRunning}, Dashed: {data.dashed}");

//    //            // Set running animation first
//    //            characterAnimator.SetIsRunning(data.isRunning);

//    //            // Only trigger jump if actually jumping AND was grounded before
//    //            if (data.jumped && wasGrounded)
//    //            {
//    //                characterAnimator.SetIsJumping(true);
//    //            }

//    //            // Reset jumping state if not jumping
//    //            if (!data.jumped)
//    //            {
//    //                characterAnimator.SetIsJumping(false); // Reset jump animation
//    //            }

//    //            if (data.dashed)
//    //            {
//    //                characterAnimator.PlayDashAnimation();
//    //            }
//    //        }

//    //        //IEnumerator DelayedJump()
//    //        //{
//    //        //    yield return new WaitForSeconds(0.01f);
//    //        //    characterAnimator.SetIsJumping(true);
//    //        //}

//    //        // Update grounded state for next frame
//    //        wasGrounded = !data.jumped;

//    //        replayIndex = (replayIndex + 1) % maxStoredPositions;
//    //        totalStored--;

//    //        yield return new WaitForSeconds(0.1f);
//    //    }

//    //    isReplaying = false;
//    //}

//    //private IEnumerator ReplayMovement()
//    //{
//    //    isReplaying = true;
//    //    yield return new WaitForSeconds(delayTime);

//    //    int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions;
//    //    bool wasGrounded = true; // Track previous grounded state

//    //    while (totalStored > 0)
//    //    {
//    //        MovementData data = movementHistory[replayIndex];

//    //        // Align shadow's position to player position
//    //        rb.position = data.position; // Directly set the position to match player

//    //        // Smooth horizontal movement
//    //        rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);

//    //        // Handle jumping
//    //        if (data.jumped)
//    //        {
//    //            rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.velocity.y); // Apply jump velocity
//    //        }
//    //        else if (wasGrounded)
//    //        {
//    //            // Prevent unintended upward movement when not jumping
//    //            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, float.MinValue, 0f));
//    //        }

//    //        // Handle animations
//    //        if (characterAnimator != null)
//    //        {
//    //            characterAnimator.SetIsRunning(data.isRunning);

//    //            if (data.jumped && wasGrounded)
//    //            {
//    //                characterAnimator.SetIsJumping(true);
//    //            }

//    //            if (!data.jumped)
//    //            {
//    //                characterAnimator.SetIsJumping(false);
//    //            }

//    //            if (data.dashed)
//    //            {
//    //                characterAnimator.PlayDashAnimation();
//    //            }
//    //        }

//    //        wasGrounded = !data.jumped;

//    //        replayIndex = (replayIndex + 1) % maxStoredPositions;
//    //        totalStored--;

//    //        yield return new WaitForSeconds(0.1f);
//    //    }

//    //    isReplaying = false;
//    //}



//    private IEnumerator ReplayMovement()
//    {
//        isReplaying = true;
//        yield return new WaitForSeconds(delayTime);

//        int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions;
//        bool wasGrounded = true; // Track previous grounded state

//        while (totalStored > 0)
//        {
//            MovementData data = movementHistory[replayIndex];

//            // Align shadow's position to player position
//            rb.position = data.position; // Directly set the position to match player

//            // Smooth horizontal movement
//            rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);

//            // Handle jumping
//            if (data.jumped)
//            {
//                rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.velocity.y); // Apply jump velocity
//            }
//            else if (wasGrounded)
//            {
//                // Prevent unintended upward movement when not jumping
//                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, float.MinValue, 0f));
//            }

//            // Handle animations with smoother transitions
//            if (characterAnimator != null)
//            {
//                characterAnimator.SetIsRunning(data.isRunning);

//                // Smoothly set jumping based on vertical velocity
//                if (data.jumped && wasGrounded)
//                {
//                    characterAnimator.SetIsJumping(true);
//                }
//                else if (rb.linearVelocity.y <= 0) // When falling or grounded
//                {
//                    characterAnimator.SetIsJumping(false);
//                }

//                if (data.dashed)
//                {
//                    characterAnimator.PlayDashAnimation();
//                }
//            }

//            wasGrounded = !data.jumped;

//            replayIndex = (replayIndex + 1) % maxStoredPositions;
//            totalStored--;

//            yield return new WaitForSeconds(0.1f);
//        }

//        isReplaying = false;
//    }




//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        Debug.Log("Trigger entered: " + other.gameObject.name);  // Debugging collision detection
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("Game Over! Shadow caught the player."); // Debugging shadow-player collision
//            if (!isDead)
//            {
//                isDead = true;
//                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
//                gameManager.GameOver(); // Call the Game Over method
//            }
//        }
//        if (other.CompareTag("Obstacle"))
//        {
//            Debug.Log("Game Over! Shadow hit an obstacle.");
//            if (!isDead)
//            {
//                isDead = true;
//                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
//                gameManager.GameOver(); // Call the Game Over method
//            }
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        Debug.Log("Collision entered: " + collision.gameObject.name);  // Debugging collision detection
//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            Debug.Log("Game Over! Shadow hit an obstacle.");
//            if (!isDead)
//            {
//                isDead = true;
//                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
//                gameManager.GameOver(); // Call the Game Over method
//            }
//        }

//        if (collision.gameObject.CompareTag("Player"))
//        {
//            Debug.Log("Game Over! Shadow caught the player."); // Debugging shadow-player collision
//            if (!isDead)
//            {
//                isDead = true;
//                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
//                gameManager.GameOver(); // Call the Game Over method
//            }
//        }
//    }

//    void CheckGrounded()
//    {
//        Vector2 feetPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
//        float checkRadius = 0.2f;
//        bool grounded = Physics2D.OverlapCircle(feetPosition, checkRadius, LayerMask.GetMask("Ground"));

//        if (grounded)
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // ✅ Stop gravity effect
//            characterAnimator.SetIsJumping(false);
//        }

//        Debug.DrawRay(transform.position, Vector2.down * 0.5f, grounded ? Color.green : Color.red);
//    }


//}


using UnityEngine;
using System.Collections;

public class ShadowFollower : MonoBehaviour
{
    public float delayTime = 1f;
    public float interpolation_factor = 10f;
    private Rigidbody2D rb;

    private const int maxStoredPositions = 100; // Prevents memory overflow
    private MovementData[] movementHistory = new MovementData[maxStoredPositions];
    private int currentIndex = 0, totalStored = 0;

    private bool isReplaying = false;
    private static GameManager gameManager; // Cache singleton reference
    private CharacterAnimator characterAnimator; // Reference to the player's animator
    private bool isDead = false; // To check if the player is dead

    private class MovementData
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool jumped;
        public bool dashed;
        public bool isRunning;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameManager = GameManager.Instance;
        characterAnimator = GetComponent<CharacterAnimator>();

        for (int i = 0; i < maxStoredPositions; i++)
            movementHistory[i] = new MovementData();

        // Disable only visual representation instead of the whole object
        GetComponent<SpriteRenderer>().enabled = false;

        StartCoroutine(EnableShadowAfterDelay());
    }

    private IEnumerator EnableShadowAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        // Re-enable the sprite instead of activating the GameObject
        GetComponent<SpriteRenderer>().enabled = true;
    }


    public void StorePosition(Vector2 position, Vector2 velocity, bool jumped, bool dashed, bool isRunning)
    {
        MovementData data = movementHistory[currentIndex];
        data.position = position;
        data.velocity = velocity;
        data.jumped = jumped;
        data.dashed = dashed;
        data.isRunning = isRunning;

        currentIndex = (currentIndex + 1) % maxStoredPositions;
        totalStored = Mathf.Min(totalStored + 1, maxStoredPositions);

        if (!isReplaying)
        {
            StopCoroutine("ReplayMovement");
            StartCoroutine("ReplayMovement");
        }
    }

    private IEnumerator ReplayMovement()
    {
        isReplaying = true;
        yield return new WaitForSeconds(delayTime);

        int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions;
        bool wasGrounded = true; // Track previous grounded state

        while (totalStored > 0)
        {
            MovementData data = movementHistory[replayIndex];
            rb.position = data.position;
            rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);

            if (data.jumped)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.velocity.y);
            }
            else if (wasGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, float.MinValue, 0f));
            }

            if (characterAnimator != null)
            {
                characterAnimator.SetIsRunning(data.isRunning);
                if (data.jumped && wasGrounded)
                {
                    characterAnimator.SetIsJumping(true);
                }
                else if (rb.linearVelocity.y <= 0)
                {
                    characterAnimator.SetIsJumping(false);
                }
                if (data.dashed)
                {
                    characterAnimator.PlayDashAnimation();
                }
            }

            wasGrounded = !data.jumped;
            replayIndex = (replayIndex + 1) % maxStoredPositions;
            totalStored--;

            yield return new WaitForSeconds(0.1f);
        }
        isReplaying = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Obstacle"))
        {
            if (!isDead)
            {
                isDead = true;
                characterAnimator.PlayDeathAnimation();
                gameManager.GameOver();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Player"))
        {
            if (!isDead)
            {
                isDead = true;
                characterAnimator.PlayDeathAnimation();
                gameManager.GameOver();
            }
        }
    }

    void CheckGrounded()
    {
        Vector2 feetPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        float checkRadius = 0.2f;
        bool grounded = Physics2D.OverlapCircle(feetPosition, checkRadius, LayerMask.GetMask("Ground"));

        if (grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            characterAnimator.SetIsJumping(false);
        }

        Debug.DrawRay(transform.position, Vector2.down * 0.5f, grounded ? Color.green : Color.red);
    }
}
