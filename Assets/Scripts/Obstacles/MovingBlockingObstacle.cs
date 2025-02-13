using UnityEngine;

public class MovingBlockingObstacle : MonoBehaviour
{
    public float moveDistance = 2f; // How far it moves up
    public float moveSpeed = 2f; // Speed of movement
    public float waitTime = 1f; // Time to wait at top and bottom

    private Vector3 startPos;
    private bool movingUp = true;
    private float waitTimer;
    private Collider2D col;

    void Start()
    {
        startPos = transform.position;
        col = GetComponent<Collider2D>();
        waitTimer = waitTime;
    }

    void Update()
    {
        MoveObstacle();
    }

    void MoveObstacle()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        // Move up or down
        float direction = movingUp ? 1f : -1f;
        transform.position += Vector3.up * direction * moveSpeed * Time.deltaTime;

        // Check if it reached top or bottom
        if (movingUp && transform.position.y >= startPos.y + moveDistance)
        {
            movingUp = false;
            ToggleBlocking(true); // Blocks player when down
            waitTimer = waitTime;
        }
        else if (!movingUp && transform.position.y <= startPos.y)
        {
            movingUp = true;
            ToggleBlocking(false); // Unblocks when moving up
            waitTimer = waitTime;
        }
    }

    void ToggleBlocking(bool state)
    {
        col.enabled = state; // Enable collider when blocking, disable when unblocking
    }
}
