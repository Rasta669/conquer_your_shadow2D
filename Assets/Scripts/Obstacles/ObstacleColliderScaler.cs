using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObstacleColliderScaler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool updateCollider = true; // Toggle collider scaling
    [SerializeField] private Vector2 sizeMultiplier = Vector2.one; // Optional size adjustment

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Vector2[][] originalColliderPaths; // Store original points for each path
    private Sprite lastSprite; // Track the last sprite to detect changes
    private Vector2 originalSpriteSize; // Store the original sprite's size

    void Start()
    {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Validate setup
        if (spriteRenderer == null)
        {
            Debug.LogError($"No SpriteRenderer found on {gameObject.name}. Disabling script.");
            enabled = false;
            return;
        }

        if (polygonCollider == null)
        {
            Debug.LogWarning($"No PolygonCollider2D found on {gameObject.name}. Collider scaling will not occur.");
            updateCollider = false;
            return;
        }

        // Store initial data
        if (updateCollider)
        {
            StoreOriginalColliderPaths();
            lastSprite = spriteRenderer.sprite;
            originalSpriteSize = spriteRenderer.sprite != null ? spriteRenderer.sprite.bounds.size : Vector2.one;
            UpdateColliderSize();
        }
    }

    void LateUpdate()
    {
        if (updateCollider)
        {
            // Check if the sprite has changed (e.g., via Timeline animation)
            if (spriteRenderer.sprite != lastSprite)
            {
                lastSprite = spriteRenderer.sprite;
                UpdateColliderSize();
            }
            else
            {
                // Still update in case transform.localScale changes
                UpdateColliderSize();
            }
        }
    }

    private void StoreOriginalColliderPaths()
    {
        // Store the initial points for each path in the PolygonCollider2D
        originalColliderPaths = new Vector2[polygonCollider.pathCount][];
        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            originalColliderPaths[i] = polygonCollider.GetPath(i);
        }
    }

    private void UpdateColliderSize()
    {
        if (spriteRenderer == null || polygonCollider == null || spriteRenderer.sprite == null) return;

        // Get current sprite dimensions (in world units)
        Vector2 currentSpriteSize = spriteRenderer.sprite.bounds.size;
        Vector3 localScale = transform.localScale;

        // Calculate scale factors based on sprite size change and localScale
        Vector2 scaleFactor = new Vector2(
            (currentSpriteSize.x / originalSpriteSize.x) * Mathf.Abs(localScale.x) * sizeMultiplier.x,
            (currentSpriteSize.y / originalSpriteSize.y) * Mathf.Abs(localScale.y) * sizeMultiplier.y
        );

        // Update each path in the PolygonCollider2D
        for (int i = 0; i < originalColliderPaths.Length; i++)
        {
            Vector2[] scaledPoints = new Vector2[originalColliderPaths[i].Length];
            for (int j = 0; j < originalColliderPaths[i].Length; j++)
            {
                // Scale the original points
                scaledPoints[j] = new Vector2(
                    originalColliderPaths[i][j].x * scaleFactor.x,
                    originalColliderPaths[i][j].y * scaleFactor.y
                );
            }
            polygonCollider.SetPath(i, scaledPoints);
        }
    }

    // Visualize collider bounds in Editor
    void OnDrawGizmos()
    {
        if (!updateCollider || spriteRenderer == null || polygonCollider == null) return;

        Gizmos.color = Color.green;
        Vector3 localScale = transform.localScale;
        Vector2 scaleFactor = new Vector2(
            Mathf.Abs(localScale.x) * sizeMultiplier.x,
            Mathf.Abs(localScale.y) * sizeMultiplier.y
        );

        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            Vector2[] points = polygonCollider.GetPath(i);
            for (int j = 0; j < points.Length; j++)
            {
                Vector2 current = points[j];
                Vector2 next = points[(j + 1) % points.Length]; // Loop back to first point
                Gizmos.DrawLine(
                    transform.position + new Vector3(current.x, current.y, 0),
                    transform.position + new Vector3(next.x, next.y, 0)
                );
            }
        }
    }
}