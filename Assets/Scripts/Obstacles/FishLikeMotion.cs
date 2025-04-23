using UnityEngine;

public class FishLikeMotion : MonoBehaviour
{
    [SerializeField] private float minX = -5f; // Minimum X position
    [SerializeField] private float maxX = 5f;  // Maximum X position
    [SerializeField] private float minY = -3f; // Minimum Y position
    [SerializeField] private float maxY = 3f;  // Maximum Y position
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private float curveStrength = 1f; // Strength of U-shaped curve

    private Vector2 targetPosition;
    private float curveTimer;

    void Start()
    {
        // Initialize with a random target
        PickNewTarget();
    }

    void Update()
    {
        // Move towards target with U-shaped motion
        Vector2 currentPos = transform.position;
        Vector2 direction = (targetPosition - currentPos).normalized;

        // Calculate curve effect
        curveTimer += Time.deltaTime * speed;
        float curveOffset = Mathf.Sin(curveTimer) * curveStrength;

        // Apply movement
        Vector2 newPos = Vector2.MoveTowards(
            currentPos,
            targetPosition + new Vector2(0, curveOffset),
            speed * Time.deltaTime
        );

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        // Pick new target when close enough
        if (Vector2.Distance(currentPos, targetPosition) < 0.1f)
        {
            PickNewTarget();
            curveTimer = 0f;
        }
    }

    void PickNewTarget()
    {
        // Randomly select new target within bounds
        targetPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }
}