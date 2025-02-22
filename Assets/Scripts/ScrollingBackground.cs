using UnityEngine;

public class SeamlessScrolling : MonoBehaviour
{
    public float scrollSpeed = 2f;      // How fast the backgrounds scroll
    public float bgWidth = 10f;         // Width of one background image (in Unity units)
    public float buffer = 0.1f;         // A small buffer to trigger repositioning early
    public Transform player;            // Reference to the player (or the camera's follow target)

    private Transform[] backgrounds;    // Array holding all background transforms
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Get all background images (assumed to be children of this GameObject)
        int count = transform.childCount;
        backgrounds = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        // Move each background left continuously
        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }

        // Calculate the camera's right edge (assuming an orthographic camera)
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        float camRightEdge = player.position.x + camHalfWidth;

        // Determine the rightmost background by comparing their x-positions
        Transform rightmost = backgrounds[0];
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > rightmost.position.x)
                rightmost = bg;
        }

        // If the right edge of the rightmost background is about to fall short of the camera's right edge...
        if (rightmost.position.x + (bgWidth / 2) < camRightEdge - buffer)
        {
            // Find the leftmost background
            Transform leftmost = backgrounds[0];
            foreach (Transform bg in backgrounds)
            {
                if (bg.position.x < leftmost.position.x)
                    leftmost = bg;
            }
            // Reposition the leftmost background to the right of the rightmost one
            leftmost.position = new Vector3(rightmost.position.x + bgWidth, leftmost.position.y, leftmost.position.z);
        }
    }
}
