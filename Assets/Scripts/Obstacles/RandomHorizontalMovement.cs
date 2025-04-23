using UnityEngine;
using System.Collections;

public class RandomHorizontalMovement : MonoBehaviour
{
    [Tooltip("Minimum X position the object can move to.")]
    public float minX = -5f;

    [Tooltip("Maximum X position the object can move to.")]
    public float maxX = 5f;

    [Tooltip("Speed of the movement.")]
    public float speed = 2f;

    [Tooltip("Time to wait before changing direction.")]
    public float changeDirectionInterval = 2f;

    private float targetX;
    private bool movingRight = true;

    void Start()
    {
        // Initialize the target X position
        SetNewTargetX();

        // Start the coroutine to change direction periodically
        StartCoroutine(ChangeDirection());
    }

    void Update()
    {
        // Move the object towards the target X position
        MoveHorizontally();
    }

    void MoveHorizontally()
    {
        // Calculate the movement step
        float step = speed * Time.deltaTime;

        // Move towards the target X position
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), step);

        // If the object has reached the target X position, set a new target
        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
        {
            SetNewTargetX();
        }
    }

    void SetNewTargetX()
    {
        // Generate a new random X position within the specified range
        targetX = Random.Range(minX, maxX);

        // Determine if the object should move right or left based on the target X position
        movingRight = (targetX > transform.position.x);
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(changeDirectionInterval);

            // Set a new target X position to change direction
            SetNewTargetX();
        }
    }
}