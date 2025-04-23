////using UnityEngine;

////public class CameraFollow2D : MonoBehaviour
////{
////    [Header("Target")]
////    [SerializeField] private Transform player; // Player to follow

////    [Header("Follow Settings")]
////    [SerializeField] private float smoothTime = 0.3f; // Smooth damp time
////    [SerializeField] private Vector2 offset = new Vector2(2f, 2f); // Initial offset (player on left, top)

////    [Header("Look Down Settings")]
////    [SerializeField] private LayerMask platformLayer; // Layers for platforms triggering look down
////    [SerializeField] private float lookDownOffset = -2f; // Extra downward offset when on platform
////    [SerializeField] private float platformCheckRadius = 0.2f; // Radius to check for platform under player

////    [Header("Bounds")]
////    [SerializeField] private Collider2D boundsCollider; // Collider defining camera bounds
////    [SerializeField] private bool useBounds = true; // Toggle bounds usage

////    private Vector3 velocity = Vector3.zero;
////    private Camera cam;
////    private bool isLookingDown = false;

////    void Start()
////    {
////        cam = GetComponent<Camera>();
////        if (player == null)
////        {
////            Debug.LogError("Player Transform not assigned in CameraFollow2D!");
////        }
////        if (useBounds && boundsCollider == null)
////        {
////            Debug.LogWarning("Bounds Collider not assigned, bounds will be ignored!");
////            useBounds = false;
////        }
////    }

////    void LateUpdate()
////    {
////        if (player == null) return;

////        // Check if player is on a specified platform
////        isLookingDown = CheckForPlatform();

////        // Calculate target position
////        Vector3 targetPosition = new Vector3(
////            player.position.x + offset.x,
////            player.position.y + offset.y + (isLookingDown ? lookDownOffset : 0f),
////            transform.position.z // Preserve camera's z position
////        );

////        // Smoothly move to target position
////        Vector3 desiredPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

////        // Apply bounds if enabled
////        if (useBounds && boundsCollider != null)
////        {
////            desiredPosition = ClampCameraPosition(desiredPosition);
////        }

////        // Update camera position
////        transform.position = desiredPosition;
////    }

////    private bool CheckForPlatform()
////    {
////        // Check for platform under player using overlap circle
////        Vector2 checkPosition = new Vector2(player.position.x, player.position.y - 0.1f);
////        Collider2D hit = Physics2D.OverlapCircle(checkPosition, platformCheckRadius, platformLayer);
////        return hit != null;
////    }

////    private Vector3 ClampCameraPosition(Vector3 desiredPosition)
////    {
////        // Get camera's orthographic size and aspect ratio
////        float camHeight = cam.orthographicSize;
////        float camWidth = camHeight * cam.aspect;

////        // Get bounds from collider
////        Bounds bounds = boundsCollider.bounds;

////        // Clamp position to keep camera within bounds
////        float clampedX = Mathf.Clamp(
////            desiredPosition.x,
////            bounds.min.x + camWidth,
////            bounds.max.x - camWidth
////        );
////        float clampedY = Mathf.Clamp(
////            desiredPosition.y,
////            bounds.min.y + camHeight,
////            bounds.max.y - camHeight
////        );

////        return new Vector3(clampedX, clampedY, desiredPosition.z);
////    }

////    // Visualize platform check radius in Editor
////    void OnDrawGizmos()
////    {
////        if (player != null)
////        {
////            Gizmos.color = Color.yellow;
////            Gizmos.DrawWireSphere(new Vector2(player.position.x, player.position.y - 0.1f), platformCheckRadius);
////        }
////    }
////}

//using UnityEngine;

//public class CameraFollow2D : MonoBehaviour
//{
//    [Header("Target")]
//    [SerializeField] private Transform player; // Player to follow

//    [Header("Follow Settings")]
//    [SerializeField] private float smoothTime = 0.3f; // Smooth damp time
//    [SerializeField] private Vector2 offset = new Vector2(2f, 2f); // Initial offset (player on left, top)

//    [Header("Look Down Settings")]
//    [SerializeField] private LayerMask platformLayer; // Layers for platforms triggering look down
//    [SerializeField] private float lookDownOffset = -2f; // Extra downward offset when on platform
//    [SerializeField] private float platformCheckRadius = 0.2f; // Radius to check for platform under player

//    [Header("Bounds")]
//    [SerializeField] private Collider2D boundsCollider; // Collider defining camera bounds
//    [SerializeField] private bool useBounds = true; // Toggle bounds usage

//    private Vector3 velocity = Vector3.zero;
//    private Camera cam;
//    private bool isLookingDown = false;

//    void Start()
//    {
//        cam = GetComponent<Camera>();
//        if (player == null)
//        {
//            Debug.LogError("Player Transform not assigned in CameraFollow2D!");
//        }
//        if (useBounds && boundsCollider == null)
//        {
//            Debug.LogWarning("Bounds Collider not assigned, bounds will be ignored!");
//            useBounds = false;
//        }
//    }

//    void LateUpdate()
//    {
//        if (player == null) return;

//        // Check if player is on a specified platform
//        isLookingDown = CheckForPlatform();

//        // Calculate target position
//        Vector3 targetPosition = new Vector3(
//            player.position.x + offset.x,
//            player.position.y + offset.y + (isLookingDown ? lookDownOffset : 0f),
//            transform.position.z // Preserve camera's z position
//        );

//        // Smoothly move to target position
//        Vector3 desiredPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

//        // Apply bounds if enabled
//        if (useBounds && boundsCollider != null)
//        {
//            desiredPosition = ClampCameraPosition(desiredPosition);
//        }

//        // Update camera position
//        transform.position = desiredPosition;
//    }

//    private bool CheckForPlatform()
//    {
//        // Check for platform under player using overlap circle
//        Vector2 checkPosition = new Vector2(player.position.x, player.position.y - 0.1f);
//        Collider2D hit = Physics2D.OverlapCircle(checkPosition, platformCheckRadius, platformLayer);
//        return hit != null;
//    }

//    private Vector3 ClampCameraPosition(Vector3 desiredPosition)
//    {
//        // Get bounds from collider
//        Bounds bounds = boundsCollider.bounds;

//        // Clamp the camera's center position to stay within bounds
//        float clampedX = Mathf.Clamp(
//            desiredPosition.x,
//            bounds.min.x,
//            bounds.max.x
//        );
//        float clampedY = Mathf.Clamp(
//            desiredPosition.y,
//            bounds.min.y,
//            bounds.max.y
//        );

//        return new Vector3(clampedX, clampedY, desiredPosition.z);
//    }

//    // Visualize platform check radius in Editor
//    void OnDrawGizmos()
//    {
//        if (player != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(new Vector2(player.position.x, player.position.y - 0.1f), platformCheckRadius);
//        }
//    }
//}

using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player; // Player to follow

    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.3f; // Smooth damp time
    [SerializeField] private Vector2 offset = new Vector2(2f, 2f); // Initial offset (player on left, top)

    [Header("Lookahead Settings")]
    [SerializeField] private float lookaheadDistance = 2f; // Distance to look ahead in facing direction
    [SerializeField] private float lookaheadSmoothTime = 0.3f; // Smooth time for lookahead adjustment

    [Header("Look Down Settings")]
    [SerializeField] private LayerMask platformLayer; // Layers for platforms triggering look down
    [SerializeField] private float lookDownOffset = -2f; // Extra downward offset when on platform
    [SerializeField] private float platformCheckRadius = 0.2f; // Radius to check for platform under player

    [Header("Bounds")]
    [SerializeField] private Collider2D boundsCollider; // Collider defining camera bounds
    [SerializeField] private bool useBounds = true; // Toggle bounds usage

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private bool isLookingDown = false;
    private float currentLookaheadOffset = 0f;
    private float lookaheadVelocity = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned in CameraFollow2D!");
        }
        if (useBounds && boundsCollider == null)
        {
            Debug.LogWarning("Bounds Collider not assigned, bounds will be ignored!");
            useBounds = false;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Check if player is on a specified platform
        isLookingDown = CheckForPlatform();

        // Calculate lookahead offset based on facing direction
        float targetLookaheadOffset = CalculateLookaheadOffset();

        // Smoothly interpolate the lookahead offset
        currentLookaheadOffset = Mathf.SmoothDamp(
            currentLookaheadOffset,
            targetLookaheadOffset,
            ref lookaheadVelocity,
            lookaheadSmoothTime
        );

        // Calculate target position with lookahead
        Vector3 targetPosition = new Vector3(
            player.position.x + offset.x + currentLookaheadOffset,
            player.position.y + offset.y + (isLookingDown ? lookDownOffset : 0f),
            transform.position.z // Preserve camera's z position
        );

        // Smoothly move to target position
        Vector3 desiredPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Apply bounds if enabled
        if (useBounds && boundsCollider != null)
        {
            desiredPosition = ClampCameraPosition(desiredPosition);
        }

        // Update camera position
        transform.position = desiredPosition;
    }

    private bool CheckForPlatform()
    {
        // Check for platform under player using overlap circle
        Vector2 checkPosition = new Vector2(player.position.x, player.position.y - 0.1f);
        Collider2D hit = Physics2D.OverlapCircle(checkPosition, platformCheckRadius, platformLayer);
        return hit != null;
    }

    private float CalculateLookaheadOffset()
    {
        // Determine facing direction based on player's localScale.x
        // Assumes positive scale (e.g., 1) is right, negative (e.g., -1) is left
        float facingDirection = player.localScale.x > 0 ? 1f : -1f;
        return lookaheadDistance * facingDirection;
    }

    private Vector3 ClampCameraPosition(Vector3 desiredPosition)
    {
        // Get bounds from collider
        Bounds bounds = boundsCollider.bounds;

        // Clamp the camera's center position to stay within bounds
        float clampedX = Mathf.Clamp(
            desiredPosition.x,
            bounds.min.x,
            bounds.max.x
        );
        float clampedY = Mathf.Clamp(
            desiredPosition.y,
            bounds.min.y,
            bounds.max.y
        );

        return new Vector3(clampedX, clampedY, desiredPosition.z);
    }

    // Visualize platform check radius in Editor
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector2(player.position.x, player.position.y - 0.1f), platformCheckRadius);
        }
    }
}