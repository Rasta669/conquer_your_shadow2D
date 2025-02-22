using UnityEngine;
using System.Collections;

public class ShadowFollower : MonoBehaviour
{
    public float delayTime = 1f;
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
        gameManager = GameManager.Instance; // Cache reference to prevent GC
        characterAnimator = GetComponent<CharacterAnimator>(); // Find the CharacterAnimator in the scene

        for (int i = 0; i < maxStoredPositions; i++)
            movementHistory[i] = new MovementData(); // Preallocate objects
    }

 
    public void StorePosition(Vector2 position, Vector2 velocity, bool jumped, bool dashed, bool isRunning)
    {
        Debug.Log("StorePosition called: " + position + ", Jumped: " + jumped + ", Dashed: " + dashed + ", Running: " + isRunning);

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
            Debug.Log("Starting ReplayMovement Coroutine...");
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

            rb.MovePosition(Vector2.Lerp(rb.position, data.position, 0.5f)); // Smooth interpolation
            rb.linearVelocity = data.velocity;

            if (characterAnimator != null)
            {
                Debug.Log($"Replaying: Jumped: {data.jumped}, Running: {data.isRunning}, Dashed: {data.dashed}");

                // Set running animation first
                characterAnimator.SetIsRunning(data.isRunning);

                // Only trigger jump if actually jumping AND was grounded before
                if (data.jumped && wasGrounded)
                {
                    StartCoroutine(DelayedJump());
                }

                // Reset jumping state if not jumping
                if (!data.jumped)
                {
                    characterAnimator.SetIsJumping(false); // Reset jump animation
                }

                if (data.dashed)
                {
                    characterAnimator.PlayDashAnimation();
                }
            }

            IEnumerator DelayedJump()
            {
                yield return new WaitForSeconds(0.01f);
                characterAnimator.SetIsJumping(true);
            }

            // Update grounded state for next frame
            wasGrounded = !data.jumped;

            replayIndex = (replayIndex + 1) % maxStoredPositions;
            totalStored--;

            yield return new WaitForSeconds(0.1f);
        }

        isReplaying = false;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered: " + other.gameObject.name);  // Debugging collision detection
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over! Shadow caught the player."); // Debugging shadow-player collision
            if (!isDead)
            {
                isDead = true;
                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
                gameManager.GameOver(); // Call the Game Over method
            }
        }
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! Shadow hit an obstacle.");
            if (!isDead)
            {
                isDead = true;
                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
                gameManager.GameOver(); // Call the Game Over method
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision entered: " + collision.gameObject.name);  // Debugging collision detection
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! Shadow hit an obstacle.");
            if (!isDead)
            {
                isDead = true;
                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
                gameManager.GameOver(); // Call the Game Over method
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over! Shadow caught the player."); // Debugging shadow-player collision
            if (!isDead)
            {
                isDead = true;
                characterAnimator.PlayDeathAnimation(); // Trigger the death animation
                gameManager.GameOver(); // Call the Game Over method
            }
        }
    }
}
