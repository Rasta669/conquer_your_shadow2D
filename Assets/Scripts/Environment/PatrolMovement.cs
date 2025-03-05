//using UnityEngine;

//public class PatrolMovement : MonoBehaviour
//{
//    public float minX = -5f;
//    public float maxX = 5f;
//    public float height = 2f;
//    public float speed = 2f;

//    private int direction = 1; // 1 for right, -1 for left

//    void Update()
//    {
//        transform.position += Vector3.right * direction * speed * Time.deltaTime;

//        if (transform.position.x >= maxX)
//        {
//            direction = -1;
//        }
//        else if (transform.position.x <= minX)
//        {
//            direction = 1;
//        }

//        // Keep the object at the specified height
//        transform.position = new Vector3(transform.position.x, height, transform.position.z);
//    }
//}

//using UnityEngine;

//public class PatrolMovement : MonoBehaviour
//{
//    public float minX = -5f;
//    public float maxX = 5f;
//    public float height = 2f;
//    public float speed = 2f;

//    private int direction = 1; // 1 for right, -1 for left

//    void Update()
//    {
//        transform.position += Vector3.right * direction * speed * Time.deltaTime;

//        if (transform.position.x >= maxX)
//        {
//            direction = -1;
//        }
//        else if (transform.position.x <= minX)
//        {
//            direction = 1;
//        }

//        // Keep the platform at the fixed height
//        transform.position = new Vector3(transform.position.x, height, transform.position.z);
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            collision.transform.SetParent(transform); // Parent player to platform
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            collision.transform.SetParent(null); // Unparent when player leaves
//        }
//    }
//}


using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public float minX = -5f;
    public float maxX = 5f;
    public float height = 2f;
    public float speed = 2f;

    private int direction = 1; // 1 for right, -1 for left
    private Transform playerTransform; // Store the player reference

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform; // Store reference to player
            playerTransform.SetParent(transform); // Attach player to platform
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerTransform == collision.transform) // Ensure it's the right player
            {
                playerTransform.SetParent(null); // Detach player
                playerTransform = null; // Clear reference
            }
        }
    }
}
