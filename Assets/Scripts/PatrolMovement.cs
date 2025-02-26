using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public float minX = -5f;
    public float maxX = 5f;
    public float height = 2f;
    public float speed = 2f;

    private int direction = 1; // 1 for right, -1 for left

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x >= maxX)
        {
            direction = -1;
        }
        else if (transform.position.x <= minX)
        {
            direction = 1;
        }

        // Keep the object at the specified height
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
