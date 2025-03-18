using UnityEngine;
using UnityEngine.UIElements;

public class Key : MonoBehaviour
{
    public string keyID; // Unique ID for each key

    private Label keyText;

    private void Start()
    {
        // Get the UI Document and reference the KeyText label
        UIDocument uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument != null)
        {
            keyText = uiDocument.rootVisualElement.Q<Label>("Keytext"); // Match UI Builder ID
            UpdateKeyUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !PlayerInventory.Instance.HasKey(keyID))
        {
            PlayerInventory.Instance.CollectKey(keyID); // Update inventory with key ID
            PlayKeySound(); 
            UpdateKeyUI();
            Destroy(gameObject); // Remove key from scene
        }
    }

    private void UpdateKeyUI()
    {
        if (keyText != null)
        {
            int collectedKeyCount = PlayerInventory.Instance.CollectedKeyCount();
            keyText.text = collectedKeyCount > 0 ? $"Portal keys: {collectedKeyCount} collected" : "Portal keys: None";
        }
    }
    private void PlayKeySound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayKeyAcquiredSound();
        }
        else
        {
            Debug.LogWarning("AudioManager instance is null! Make sure it's in the scene.");
        }
    }
}
