using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float chaseRange = 5f; // Distance to start chasing
    public float stopChaseHeight = 1.5f; // Height difference to stop chasing
    public float moveSpeed = 2f; // Speed when chasing
    public float walkSpeed = 1f; // Speed when walking back
    public float returnDelay = 2f; // Delay before returning

    private Vector2 spawnPosition; // Store the spawn position
    private bool isChasing = false;
    private bool isReturning = false;
    private CharacterAnimator animator;

    void Start()
    {
        spawnPosition = transform.position; // Store initial spawn position
        animator = GetComponent<CharacterAnimator>(); // Get animator script
        animator.PlayIdleAnimation(); // Set idle as default
    }

    void Update()
    {
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceY = player.position.y - transform.position.y;

        if (distanceX <= chaseRange && Mathf.Abs(distanceY) <= stopChaseHeight)
        {
            isChasing = true;
            isReturning = false;
            ChasePlayer();
        }
        else if (distanceY > stopChaseHeight)
        {
            isChasing = false;
            Invoke(nameof(StartReturning), returnDelay);
        }

        if (isReturning)
        {
            ReturnToSpawn();
        }
    }

    void ChasePlayer()
    {
        animator.SetIsRunning(true);
        animator.SetIsWalking(false);

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), step);

        FlipSprite(player.position.x);
    }

    void StartReturning()
    {
        isReturning = true;
        isChasing = false;
        animator.SetIsRunning(false);
        animator.SetIsWalking(true);
    }

    void ReturnToSpawn()
    {
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, spawnPosition, step);

        FlipSprite(spawnPosition.x);

        if (Vector2.Distance(transform.position, spawnPosition) <= 0.1f)
        {
            isReturning = false;
            animator.SetIsWalking(false);
            animator.PlayIdleAnimation();
        }
    }

    void FlipSprite(float targetX)
    {
        transform.localScale = new Vector3(targetX > transform.position.x ? 1 : -1, 1, 1);
    }
}
