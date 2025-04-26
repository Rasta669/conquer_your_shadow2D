////using UnityEngine;

////public class PatrolMovement : MonoBehaviour
////{
////    public float minX = -5f;
////    public float maxX = 5f;
////    public float height = 2f;
////    public float speed = 2f;

////    private int direction = 1; // 1 for right, -1 for left

////    void Update()
////    {
////        transform.position += Vector3.right * direction * speed * Time.deltaTime;

////        if (transform.position.x >= maxX)
////        {
////            direction = -1;
////        }
////        else if (transform.position.x <= minX)
////        {
////            direction = 1;
////        }

////        // Keep the object at the specified height
////        transform.position = new Vector3(transform.position.x, height, transform.position.z);
////    }
////}

////using UnityEngine;

////public class PatrolMovement : MonoBehaviour
////{
////    public float minX = -5f;
////    public float maxX = 5f;
////    public float height = 2f;
////    public float speed = 2f;

////    private int direction = 1; // 1 for right, -1 for left

////    void Update()
////    {
////        transform.position += Vector3.right * direction * speed * Time.deltaTime;

////        if (transform.position.x >= maxX)
////        {
////            direction = -1;
////        }
////        else if (transform.position.x <= minX)
////        {
////            direction = 1;
////        }

////        // Keep the platform at the fixed height
////        transform.position = new Vector3(transform.position.x, height, transform.position.z);
////    }

////    private void OnCollisionEnter2D(Collision2D collision)
////    {
////        if (collision.gameObject.CompareTag("Player"))
////        {
////            collision.transform.SetParent(transform); // Parent player to platform
////        }
////    }

////    private void OnCollisionExit2D(Collision2D collision)
////    {
////        if (collision.gameObject.CompareTag("Player"))
////        {
////            collision.transform.SetParent(null); // Unparent when player leaves
////        }
////    }
////}


//using UnityEngine;

//public class PatrolMovement : MonoBehaviour
//{
//    public float minX = -5f;
//    public float maxX = 5f;
//    public float height = 2f;
//    public float speed = 2f;

//    private int direction = 1; // 1 for right, -1 for left
//    //private Transform playerTransform; // Store the player reference

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


using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [SerializeField] private float minX = -5f; // Minimum X position
    [SerializeField] private float maxX = 5f; // Maximum X position
    [SerializeField] private float height = 2f; // Fixed height
    [SerializeField] private float speed = 2f; // Movement speed

    private int direction = 1; // 1 for right, -1 for left

    void Update()
    {
        // Move the platform
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Reverse direction at boundaries
        if (transform.position.x >= maxX)
        {
            direction = -1;
        }
        else if (transform.position.x <= minX)
        {
            direction = 1;
        }

        // Keep the platform at the fixed height
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Parent the player to the platform
            collision.transform.SetParent(transform);
            Debug.Log($"{collision.gameObject.name} is now parented to {gameObject.name}");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the exiting object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Unparent the player
            collision.transform.SetParent(null);
            Debug.Log($"{collision.gameObject.name} is no longer parented to {gameObject.name}");
        }
    }
}