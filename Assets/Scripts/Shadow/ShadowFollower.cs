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

    private class MovementData
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool jumped;
        public bool dashed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameManager = GameManager.Instance; // Cache reference to prevent GC
        for (int i = 0; i < maxStoredPositions; i++)
            movementHistory[i] = new MovementData(); // Preallocate objects
    }

    public void StorePosition(Vector2 position, Vector2 velocity, bool jumped, bool dashed)
    {
        // Store data in circular buffer (reusing preallocated objects)
        MovementData data = movementHistory[currentIndex];
        data.position = position;
        data.velocity = velocity;
        data.jumped = jumped;
        data.dashed = dashed;

        currentIndex = (currentIndex + 1) % maxStoredPositions; // Circular index
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

        int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions; // Start from oldest

        while (totalStored > 0)
        {
            MovementData data = movementHistory[replayIndex];

            rb.MovePosition(data.position);
            rb.linearVelocity = data.velocity;

            replayIndex = (replayIndex + 1) % maxStoredPositions;
            totalStored--;

            yield return new WaitForSeconds(0.1f);
        }

        isReplaying = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered: " + other.gameObject.name);  // Add this line for debugging
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over! Shadow caught the player.");
            gameManager.GameOver(); // Use cached reference
        }
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! Shadow hit an obstacle.");
            gameManager.GameOver(); // Call the Game Over method
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision entered: " + collision.gameObject.name);  // Add this line for debugging
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! Shadow hit an obstacle.");
            gameManager.GameOver(); // Use cached reference
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over! Shadow caught the player.");
            gameManager.GameOver(); // Use cached reference
        }
    }

   


}
