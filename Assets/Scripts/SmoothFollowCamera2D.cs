//using UnityEngine;

//public class PlayerFollowCamera2D : MonoBehaviour
//{
//    public Transform player; // Reference to the player's transform
//    public PlayerMovement playerMovement; // Reference to PlayerMovement script
//    public Transform groundSprite; // Reference to the ground sprite
//    public float smoothTime = 0.15f; // Time to reach target position (responsive)
//    public float aboveGroundOffsetX = -2f; // Player offset to left side when above ground
//    public float belowGroundOffsetX = 2f; // Player offset to right side when below ground
//    public float horizontalNudgeFactor = 1f; // How much camera nudges left/right
//    public float verticalOffset = 0f; // Vertical offset for following
//    public float groundVisibilityOffset = 0.5f; // Offset to show upper part of ground

//    private Camera mainCamera;
//    private Vector2 originalSize;
//    private Vector3 velocity = Vector3.zero;
//    private float groundY; // Y position of the ground sprite's top edge
//    private bool hasLanded = false; // Track if player has landed
//    private Rigidbody2D playerRb; // Player's Rigidbody2D

//    void Start()
//    {
//        mainCamera = GetComponent<Camera>();
//        // Store original camera dimensions
//        originalSize = new Vector2(
//            mainCamera.orthographicSize * mainCamera.aspect,
//            mainCamera.orthographicSize
//        );

//        // Get the top edge of the ground sprite
//        if (groundSprite != null)
//        {
//            SpriteRenderer groundRenderer = groundSprite.GetComponent<SpriteRenderer>();
//            if (groundRenderer != null)
//            {
//                groundY = groundSprite.position.y + groundRenderer.bounds.extents.y;
//            }
//            else
//            {
//                groundY = groundSprite.position.y;
//            }
//        }

//        // Get player's Rigidbody2D
//        if (player != null)
//        {
//            playerRb = player.GetComponent<Rigidbody2D>();
//        }

//        // Keep camera at initial position (e.g., (0,0,0))
//    }

//    void LateUpdate()
//    {
//        if (player != null && playerMovement != null && playerRb != null)
//        {
//            // Check if player has landed
//            if (!hasLanded && playerMovement.IsGrounded())
//            {
//                hasLanded = true;
//            }

//            // Only follow player after landing
//            if (hasLanded)
//            {
//                // Determine if player is below ground and moving downward
//                bool isBelowGround = player.position.y < groundY;
//                bool isMovingDownBelowGround = isBelowGround && playerRb.linearVelocity.y < 0f;

//                // Get horizontal input for nudge
//                float horizontalInput = Input.GetAxisRaw("Horizontal");
//                float horizontalNudge = horizontalInput * horizontalNudgeFactor;

//                // Set horizontal offset based on ground position
//                float horizontalOffset = isBelowGround ? belowGroundOffsetX : aboveGroundOffsetX;
//                horizontalOffset += horizontalNudge;

//                // Calculate desired position
//                Vector3 desiredPosition = new Vector3(
//                    player.position.x + horizontalOffset,
//                    player.position.y + verticalOffset,
//                    transform.position.z
//                );

//                // Clamp camera to keep lower border at or above ground unless moving down below ground
//                if (!isMovingDownBelowGround)
//                {
//                    float cameraHeight = mainCamera.orthographicSize;
//                    float cameraBottomY = desiredPosition.y - cameraHeight;
//                    if (cameraBottomY < groundY + groundVisibilityOffset)
//                    {
//                        desiredPosition.y = groundY + cameraHeight + groundVisibilityOffset;
//                    }
//                }

//                // Smoothly move to desired position using SmoothDamp
//                transform.position = Vector3.SmoothDamp(
//                    transform.position,
//                    desiredPosition,
//                    ref velocity,
//                    smoothTime
//                );

//                // Maintain original camera dimensions
//                mainCamera.orthographicSize = originalSize.y;
//            }
//        }
//    }
//}


using UnityEngine;

public class SmoothFollowCamera2D : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Camera follow smoothness
    public Vector3 offset; // Offset from the player

    private Camera mainCamera;
    private Vector2 originalSize;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        // Store original camera dimensions
        originalSize = new Vector2(
            mainCamera.orthographicSize * mainCamera.aspect,
            mainCamera.orthographicSize
        );
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate desired position
            Vector3 desiredPosition = player.position + offset;
            // Restrict to X-axis movement only
            desiredPosition.y = transform.position.y;
            desiredPosition.z = transform.position.z;

            // Smoothly interpolate to desired position
            Vector3 smoothedPosition = Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );

            // Apply position
            transform.position = smoothedPosition;

            // Maintain original camera dimensions
            mainCamera.orthographicSize = originalSize.y;
        }
    }
}