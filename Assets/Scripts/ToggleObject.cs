//using UnityEngine;

//public class ToggleObject : MonoBehaviour
//{
//    [SerializeField] private float toggleTime = 2f; // Time in seconds to toggle, set in Inspector
//    private SpriteRenderer spriteRenderer; // Use SpriteRenderer for 2D sprite
//    private Collider2D objectCollider; // Use Collider2D for PolygonCollider2D
//    private float timer = 0f;
//    private bool isObjectActive = true;

//    void Start()
//    {
//        // Get the SpriteRenderer component
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        if (spriteRenderer == null)
//        {
//            Debug.LogError($"No SpriteRenderer found on {gameObject.name}!");
//        }
//        else
//        {
//            Debug.Log($"SpriteRenderer found on {gameObject.name}");
//        }

//        // Get the Collider2D component (e.g., PolygonCollider2D)
//        objectCollider = GetComponent<Collider2D>();
//        if (objectCollider == null)
//        {
//            Debug.LogError($"No Collider2D found on {gameObject.name}!");
//        }
//        else
//        {
//            Debug.Log($"Collider2D found on {gameObject.name}: {objectCollider.GetType().Name}, Enabled: {objectCollider.enabled}");
//        }
//    }

//    void Update()
//    {
//        // Increment timer
//        timer += Time.deltaTime;

//        // Check if toggle time has been reached
//        if (timer >= toggleTime)
//        {
//            // Toggle active state
//            isObjectActive = !isObjectActive;

//            // Toggle SpriteRenderer visibility
//            if (spriteRenderer != null)
//            {
//                spriteRenderer.enabled = isObjectActive;
//                Debug.Log($"{gameObject.name} SpriteRenderer set to: {spriteRenderer.enabled}");
//            }
//            else
//            {
//                Debug.LogWarning($"{gameObject.name} has no SpriteRenderer to toggle");
//            }

//            // Toggle Collider2D
//            if (objectCollider != null)
//            {
//                objectCollider.enabled = isObjectActive;
//                Debug.Log($"{gameObject.name} Collider2D ({objectCollider.GetType().Name}) set to enabled: {objectCollider.enabled}");
//            }
//            else
//            {
//                Debug.LogError($"{gameObject.name} has no Collider2D to toggle");
//            }

//            // Reset timer
//            timer = 0f;
//        }
//    }
//}

using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] private float timeOn = 2f; // Time in seconds to remain on, set in Inspector
    [SerializeField] private float timeOff = 2f; // Time in seconds to remain off, set in Inspector
    private SpriteRenderer spriteRenderer; // Use SpriteRenderer for 2D sprite
    private Collider2D objectCollider; // Use Collider2D for PolygonCollider2D
    private float timer = 0f;
    private bool isObjectActive = true;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"No SpriteRenderer found on {gameObject.name}!");
        }
        else
        {
            Debug.Log($"SpriteRenderer found on {gameObject.name}");
        }

        // Get the Collider2D component (e.g., PolygonCollider2D)
        objectCollider = GetComponent<Collider2D>();
        if (objectCollider == null)
        {
            Debug.LogError($"No Collider2D found on {gameObject.name}!");
        }
        else
        {
            Debug.Log($"Collider2D found on {gameObject.name}: {objectCollider.GetType().Name}, Enabled: {objectCollider.enabled}");
        }
    }

    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Determine current duration based on state
        float currentDuration = isObjectActive ? timeOn : timeOff;

        // Check if the current duration has been reached
        if (timer >= currentDuration)
        {
            // Toggle active state
            isObjectActive = !isObjectActive;

            // Toggle SpriteRenderer visibility
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = isObjectActive;
                Debug.Log($"{gameObject.name} SpriteRenderer set to: {spriteRenderer.enabled}");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} has no SpriteRenderer to toggle");
            }

            // Toggle Collider2D
            if (objectCollider != null)
            {
                objectCollider.enabled = isObjectActive;
                Debug.Log($"{gameObject.name} Collider2D ({objectCollider.GetType().Name}) set to enabled: {objectCollider.enabled}");
            }
            else
            {
                Debug.LogError($"{gameObject.name} has no Collider2D to toggle");
            }

            // Reset timer
            timer = 0f;
        }
    }
}