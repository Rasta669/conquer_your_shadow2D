using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string requiredKey = "PortalKey"; // Set in Inspector
    public Animator portalAnimator; // Assign in Inspector
    public Transform teleportDestination; // Set where the player should go
    private NewGameManager gameManager;

    private void Start()
    {
        gameManager = NewGameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null && playerInventory.HasKey(requiredKey))
            {
                PlayKPortalSound();
                OpenPortal(other.transform);
            }
            else
            {
                Debug.Log("You need the key to enter the portal!");
            }
        }
    }

    void OpenPortal(Transform player)
    {
        portalAnimator.Play("portal_anim"); // Play portal animation
        StartCoroutine(WaitAndTeleport(player));
    }

    private IEnumerator WaitAndTeleport(Transform player)
    {
        yield return new WaitForSeconds(1f); // Wait for animation to play
        player.position = teleportDestination.position; // Move player
        if (gameManager != null)
        {
            gameManager.GameWin();
        }
    }

    private void PlayKPortalSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPortalAnimationSound();
        }
        else
        {
            Debug.LogWarning("AudioManager instance is null! Make sure it's in the scene.");
        }
    }
}
