//using UnityEngine;

//public class EnemyChase : MonoBehaviour
//{
//    public Transform player; // Reference to the player
//    public float chaseRange = 5f; // Distance to start chasing
//    public float stopChaseHeight = 1.5f; // Height difference to stop chasing
//    public float moveSpeed = 2f; // Speed when chasing
//    public float walkSpeed = 1f; // Speed when walking back
//    public float returnDelay = 2f; // Delay before returning
//    public float patrolRange = 3f; // Distance the enemy will patrol
//    public float patrolSpeed = 1f; // Speed when patrolling

//    private Vector2 spawnPosition;
//    private Vector2 patrolPointA, patrolPointB;
//    private bool isChasing = false;
//    private bool isReturning = false;
//    private bool isPatrolling = false;
//    private CharacterAnimator animator;
//    private Vector2 targetPatrolPoint;

//    private bool isFacingRight = true; // Track which direction the enemy is facing

//    void Start()
//    {
//        spawnPosition = transform.position;
//        patrolPointA = new Vector2(spawnPosition.x - patrolRange, spawnPosition.y);
//        patrolPointB = new Vector2(spawnPosition.x + patrolRange, spawnPosition.y);
//        targetPatrolPoint = patrolPointA; // Start moving left

//        animator = GetComponent<CharacterAnimator>();
//        if (animator == null)
//        {
//            Debug.LogError("Animator component missing on " + gameObject.name);
//            return;
//        }

//        FlipSprite(-1); // Face left at start
//        Invoke(nameof(StartPatrolling), 2f);
//    }

//    void Update()
//    {
//        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
//        float distanceY = player.position.y - transform.position.y;

//        // **STOP CHASING if player moves too high**
//        if (isChasing && distanceY > stopChaseHeight)
//        {
//            isChasing = false;
//            isReturning = true;
//            Invoke(nameof(StartPatrolling), returnDelay);
//        }

//        // **Start chasing instantly without transition**
//        if (!isChasing && distanceX <= chaseRange && Mathf.Abs(distanceY) <= stopChaseHeight)
//        {
//            isChasing = true;
//            isReturning = false;
//            isPatrolling = false;

//            // Set running animation immediately
//            animator.SetIsRunning(true);
//            animator.SetIsWalking(false);

//            // Flip to face the player
//            FlipSprite(player.position.x - transform.position.x);
//        }

//        if (isChasing)
//        {
//            ChasePlayer();
//        }
//        else if (isReturning)
//        {
//            ReturnToSpawn();
//        }
//        else if (isPatrolling)
//        {
//            Patrol();
//        }
//    }

//    void StartReturning()
//    {
//        isReturning = true;
//        isChasing = false;
//        animator.SetIsRunning(false); // Ensure running is off
//        animator.SetIsWalking(true); // Walking animation when returning
//        FlipSprite(spawnPosition.x - transform.position.x);
//    }

//    void ReturnToSpawn()
//    {
//        float step = walkSpeed * Time.deltaTime;
//        transform.position = Vector2.MoveTowards(transform.position, spawnPosition, step);

//        FlipSprite(spawnPosition.x - transform.position.x);

//        if (Vector2.Distance(transform.position, spawnPosition) <= 0.1f)
//        {
//            isReturning = false;
//            StartPatrolling();
//        }
//    }

//    void StartPatrolling()
//    {
//        if (!isChasing)
//        {
//            isPatrolling = true;
//            animator.SetIsWalking(true); // Walking animation during patrol
//            animator.SetIsRunning(false); // Ensure running animation is off during patrol
//            FlipSprite(targetPatrolPoint.x - transform.position.x);
//        }
//    }

//    void Patrol()
//    {
//        transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);

//        if (Vector2.Distance(transform.position, targetPatrolPoint) < 0.1f)
//        {
//            targetPatrolPoint = (targetPatrolPoint == patrolPointA) ? patrolPointB : patrolPointA;
//            FlipSprite(targetPatrolPoint.x - transform.position.x);
//        }
//    }

//    void ChasePlayer()
//    {
//        float step = moveSpeed * Time.deltaTime;
//        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), step);

//        // Only flip the sprite when the enemy changes direction relative to the player
//        FlipSprite(player.position.x - transform.position.x);
//    }

//    void FlipSprite(float direction)
//    {
//        // Flip the sprite to face the correct direction
//        if (direction < 0 && isFacingRight)
//        {
//            transform.localScale = new Vector3(-1, 1, 1); // Flip to left
//            isFacingRight = false;
//        }
//        else if (direction > 0 && !isFacingRight)
//        {
//            transform.localScale = new Vector3(1, 1, 1); // Flip to right
//            isFacingRight = true;
//        }
//    }
//}

using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float chaseRange = 5f; // Distance to start chasing
    public float stopChaseHeight = 1.5f; // Height difference to stop chasing
    public float moveSpeed = 2f; // Speed when chasing
    public float walkSpeed = 1f; // Speed when walking back
    public float returnDelay = 2f; // Delay before returning
    public float patrolRange = 3f; // Distance the enemy will patrol
    public float patrolSpeed = 1f; // Speed when patrolling

    private Vector2 spawnPosition;
    private Vector2 patrolPointA, patrolPointB;
    private bool isChasing = false;
    private bool isReturning = false;
    private bool isPatrolling = false;
    private CharacterAnimator animator;
    private Vector2 targetPatrolPoint;

    private bool isFacingRight = true; // Track which direction the enemy is facing

    void Start()
    {
        spawnPosition = transform.position;
        patrolPointA = new Vector2(spawnPosition.x - patrolRange, spawnPosition.y);
        patrolPointB = new Vector2(spawnPosition.x + patrolRange, spawnPosition.y);
        targetPatrolPoint = patrolPointA; // Start moving left

        animator = GetComponent<CharacterAnimator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing on " + gameObject.name);
            return;
        }

        FlipSprite(-1); // Face left at start
        Invoke(nameof(StartPatrolling), 2f);
    }

    void Update()
    {
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceY = player.position.y - transform.position.y;

        // **STOP CHASING if player moves too high**
        if (isChasing && distanceY > stopChaseHeight)
        {
            isChasing = false;
            isReturning = true;
            Invoke(nameof(StartPatrolling), returnDelay);
        }

        // **Start chasing instantly without transition**
        if (!isChasing && distanceX <= chaseRange && Mathf.Abs(distanceY) <= stopChaseHeight)
        {
            isChasing = true;
            isReturning = false;
            isPatrolling = false;

            // Set running animation immediately
            animator.SetIsRunning(true);
            animator.SetIsWalking(false);

            // Flip to face the player (only horizontally, not based on jumping)
            FlipSprite(player.position.x - transform.position.x);
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isReturning)
        {
            ReturnToSpawn();
        }
        else if (isPatrolling)
        {
            Patrol();
        }
    }

    void StartReturning()
    {
        isReturning = true;
        isChasing = false;
        animator.SetIsRunning(false); // Ensure running is off
        animator.SetIsWalking(true); // Walking animation when returning
        FlipSprite(spawnPosition.x - transform.position.x);
    }

    void ReturnToSpawn()
    {
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, spawnPosition, step);

        FlipSprite(spawnPosition.x - transform.position.x);

        if (Vector2.Distance(transform.position, spawnPosition) <= 0.1f)
        {
            isReturning = false;
            StartPatrolling();
        }
    }

    void StartPatrolling()
    {
        if (!isChasing)
        {
            isPatrolling = true;
            animator.SetIsWalking(true); // Walking animation during patrol
            animator.SetIsRunning(false); // Ensure running animation is off during patrol
            FlipSprite(targetPatrolPoint.x - transform.position.x);
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPatrolPoint) < 0.1f)
        {
            targetPatrolPoint = (targetPatrolPoint == patrolPointA) ? patrolPointB : patrolPointA;
            FlipSprite(targetPatrolPoint.x - transform.position.x);
        }
    }

    void ChasePlayer()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), step);

        // Only flip the sprite horizontally when the enemy moves in the direction of the player
        FlipSprite(player.position.x - transform.position.x);
    }

    void FlipSprite(float direction)
    {
        // Flip the sprite only when the enemy needs to change direction relative to the player
        if (direction < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip to left
            isFacingRight = false;
        }
        else if (direction > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1); // Flip to right
            isFacingRight = true;
        }
    }
}
