using UnityEngine;

public class DisappearingObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Assign the disappearing obstacle prefab
    public float spawnInterval = 10f; // Distance between spawns
    public float despawnTime = 3f; // Time before disappearing
    public float spawnHeight = 1.5f; // Adjust height placement
    public float maxSpawnDistance = 100f; // Limit spawns

    private float lastSpawnX = 0f; // Track last spawn position
    private Transform player; // Reference to player position

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure player has "Player" tag
        lastSpawnX = player.position.x;
    }

    void Update()
    {
        if (player == null) return;

        float playerX = player.position.x;

        // Spawn at intervals (e.g., every 10 units)
        if (playerX >= lastSpawnX + spawnInterval && playerX < maxSpawnDistance)
        {
            SpawnObstacle(playerX + spawnInterval);
            lastSpawnX = playerX;
        }
    }

    void SpawnObstacle(float xPosition)
    {
        Vector3 spawnPosition = new Vector3(xPosition, spawnHeight, 0f);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        Destroy(obstacle, despawnTime); // Make it disappear after some time
    }
}
