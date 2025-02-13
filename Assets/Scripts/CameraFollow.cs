using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 7f;
    public Vector3 offset = new Vector3(4f, 2f, -10f); // Adjusted for shadow visibility
    public float lookAheadFactor = 2f; // Prevents too much looking ahead
    public float verticalFollowThreshold = 1.5f;
    public float verticalSmoothing = 0.2f;

    public Camera mainCamera;
    public float defaultZoom = 7f;
    public float maxZoomOut = 8.5f;
    public float zoomSpeed = 1.5f;

    private Vector3 velocity = Vector3.zero;
    private Rigidbody2D playerRb;
    private Vector2 lastVelocity;

    private float minXLimit = 0f; // Minimum X boundary (adjust based on level)
    private float maxXLimit = 100f; // Maximum X boundary (adjust based on level)

    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        mainCamera.orthographicSize = defaultZoom;
    }

    void FixedUpdate()
    {
        if (playerRb != null)
        {
            lastVelocity = playerRb.linearVelocity;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        float playerSpeedX = lastVelocity.x;
        float playerY = player.position.y;

        // Look ahead logic (prevents camera from over-extending)
        float lookAhead = Mathf.Clamp(playerSpeedX * 0.2f, -lookAheadFactor, lookAheadFactor);

        // Vertical follow logic (only moves up/down when needed)
        float targetY = transform.position.y;
        if (Mathf.Abs(playerY - transform.position.y) > verticalFollowThreshold)
        {
            targetY = Mathf.Lerp(transform.position.y, playerY, verticalSmoothing);
        }

        // Calculate target position, ensuring it stays within the level boundaries
        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(player.position.x + lookAhead, minXLimit, maxXLimit), // Clamp X movement
            targetY,
            -10f // Ensure the camera stays in 2D view
        ) + offset;

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 🎥 Dynamic Zoom: Zoom out when moving fast
        float targetZoom = Mathf.Lerp(defaultZoom, maxZoomOut, Mathf.Abs(playerSpeedX) / 10f);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}
