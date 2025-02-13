//using UnityEngine;

//public class BouncingObstacle : MonoBehaviour
//{
//    public float bounceHeight = 2f;
//    public float bounceSpeed = 2f;

//    private bool movingUp = true;
//    private float startY;

//    void Start()
//    {
//        startY = transform.position.y;
//    }

//    void Update()
//    {
//        Bounce();
//    }

//    void Bounce()
//    {
//        float moveDirection = movingUp ? 1f : -1f;
//        transform.position += Vector3.up * moveDirection * bounceSpeed * Time.deltaTime;

//        if (transform.position.y >= startY + bounceHeight || transform.position.y <= startY)
//            movingUp = !movingUp;
//    }
//}


using UnityEngine;

public class BouncingObstacle : MonoBehaviour
{
    public float bounceHeight = 2f; // Max height from start
    public float bounceSpeed = 2f; // Speed of bouncing
    public float threshold = 0.1f; // Small buffer to prevent jittering

    private Vector3 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position; // Store initial position
    }

    void Update()
    {
        Bounce();
    }

    void Bounce()
    {
        float direction = movingUp ? 1f : -1f;
        transform.position += Vector3.up * direction * bounceSpeed * Time.deltaTime;

        // Fix jittering: Check if within a small range rather than using >= or <=
        if (movingUp && transform.position.y >= startPos.y + bounceHeight - threshold)
        {
            transform.position = new Vector3(transform.position.x, startPos.y + bounceHeight, transform.position.z);
            movingUp = false;
        }
        else if (!movingUp && transform.position.y <= startPos.y + threshold)
        {
            transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
            movingUp = true;
        }
    }
}
