using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxEffect : MonoBehaviour
{
    public float parallaxSpeed;

    private Transform cameraTransform;
    private float startPositionX;
    private float spriteSizeX;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        startPositionX = transform.position.x;
        spriteSizeX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Calculate the new position based on camera movement
        float relativeDist = (cameraTransform.position.x * parallaxSpeed);
        transform.position = new Vector3(startPositionX + relativeDist, transform.position.y, transform.position.z);

        // Looping logic to prevent background disappearing or jittering
        float offset = cameraTransform.position.x - startPositionX;

        if (offset > spriteSizeX)
        {
            startPositionX += spriteSizeX;
        }
        else if (offset < -spriteSizeX)
        {
            startPositionX -= spriteSizeX;
        }
    }
}
