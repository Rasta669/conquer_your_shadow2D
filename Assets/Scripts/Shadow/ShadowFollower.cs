
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class ShadowFollower : MonoBehaviour
{
    public float delayTime = 1f;
    public float interpolation_factor = 10f;
    private Rigidbody2D rb;

    private const int maxStoredPositions = 100;
    private MovementData[] movementHistory = new MovementData[maxStoredPositions];
    private int currentIndex = 0, totalStored = 0;
    private bool isReplaying = false;
    private float lastDirection = 1f;

    private static GameManager gameManager;
    private CharacterAnimator characterAnimator;
    private bool isDead = false;



    private class MovementData
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool jumped;
        public bool dashed;
        public bool isRunning;
        public float direction;
        public bool isClimbing; // New variable for climbing state
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        characterAnimator = GetComponent<CharacterAnimator>();

        for (int i = 0; i < maxStoredPositions; i++)
            movementHistory[i] = new MovementData();

        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(EnableShadowAfterDelay());
    }

    private IEnumerator EnableShadowAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    //public void StorePosition(Vector2 position, Vector2 velocity, bool jumped, bool dashed, bool isRunning, float direction)
    //{
    //    MovementData data = movementHistory[currentIndex];
    //    data.position = position;
    //    data.velocity = velocity;
    //    data.jumped = jumped;
    //    data.dashed = dashed;
    //    data.isRunning = isRunning;
    //    data.direction = direction;
    //    //data.isClimbing = isClimbing;

    //    currentIndex = (currentIndex + 1) % maxStoredPositions;
    //    totalStored = Mathf.Min(totalStored + 1, maxStoredPositions);

    //    if (!isReplaying)
    //    {
    //        StopCoroutine("ReplayMovement");
    //        StartCoroutine("ReplayMovement");
    //    }
    //}

    public void StorePosition(Vector2 position, Vector2 velocity, bool jumped, bool dashed, bool isRunning, float direction, bool isClimbing)
    {
        MovementData data = movementHistory[currentIndex];
        data.position = position;
        data.velocity = velocity;
        data.jumped = jumped;
        data.dashed = dashed;
        data.isRunning = isRunning;
        data.direction = direction;
        data.isClimbing = isClimbing; // Store climbing state

        currentIndex = (currentIndex + 1) % maxStoredPositions;
        totalStored = Mathf.Min(totalStored + 1, maxStoredPositions);

        if (!isReplaying)
        {
            StopCoroutine("ReplayMovement");
            StartCoroutine("ReplayMovement");
        }
    }


    //private IEnumerator ReplayMovement()
    //{
    //    isReplaying = true;
    //    yield return new WaitForSeconds(delayTime);

    //    int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions;

    //    while (totalStored > 0)
    //    {
    //        MovementData data = movementHistory[replayIndex];
    //        rb.position = data.position;
    //        rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);

    //        // Rotate shadow based on player's movement direction
    //        if (data.direction != lastDirection)
    //        {
    //            lastDirection = data.direction;
    //            transform.localScale = new Vector3(lastDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    //        }

    //        if (characterAnimator != null)
    //        {
    //            characterAnimator.SetIsRunning(data.isRunning);
    //            characterAnimator.SetIsJumping(data.jumped);
    //            if (data.dashed)
    //            {
    //                characterAnimator.PlayDashAnimation();
    //            }
    //        }

    //        replayIndex = (replayIndex + 1) % maxStoredPositions;
    //        totalStored--;

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    isReplaying = false;
    //}

    private IEnumerator ReplayMovement()
    {
        isReplaying = true;
        yield return new WaitForSeconds(delayTime);

        int replayIndex = (currentIndex - totalStored + maxStoredPositions) % maxStoredPositions;

        while (totalStored > 0)
        {
            MovementData data = movementHistory[replayIndex];

            rb.position = data.position;
            //rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);
            rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);


            // Rotate shadow based on player's movement direction
            if (data.direction != lastDirection)
            {
                lastDirection = data.direction;
                transform.localScale = new Vector3(lastDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            //if (characterAnimator != null)
            //{
            //    characterAnimator.SetIsRunning(data.isRunning);
            //    characterAnimator.SetIsJumping(data.jumped);
            //    characterAnimator.SetIsClimbing(data.isClimbing);

            //    if (data.dashed)
            //    {
            //        characterAnimator.PlayDashAnimation();
            //    }

            //    if (data.isClimbing)
            //    {
            //        characterAnimator.SetIsClimbing(true);
            //        rb.gravityScale = 0; // Disable gravity when climbing
            //    }
            //    else
            //    {
            //        characterAnimator.SetIsClimbing(false);
            //        rb.gravityScale = 2f; // Restore gravity when not climbing
            //    }
            //}

            if (characterAnimator != null)
            {
                characterAnimator.SetIsRunning(data.isRunning);
                characterAnimator.SetIsJumping(data.jumped);
                characterAnimator.SetIsClimbing(data.isClimbing);   

                if (data.dashed)
                {
                    characterAnimator.PlayDashAnimation();
                }

                // Handle climbing animations
                if (data.isClimbing)
                {
                    characterAnimator.SetIsClimbing(true);
                    //rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.velocity.y); // Apply climbing movement
                    rb.linearVelocity = new Vector2(0, data.velocity.y); // Move vertically only
                }
                else
                {
                    characterAnimator.SetIsClimbing(false);
                    rb.linearVelocity = new Vector2((data.position.x - rb.position.x) * interpolation_factor, rb.linearVelocity.y);
                }
            }

            //Debug.Log($"Shadow Climbing: {data.isClimbing}");
            Debug.Log($"Shadow Climbing: {data.isClimbing}, Velocity: {data.velocity.y}");
            




            replayIndex = (replayIndex + 1) % maxStoredPositions;
            totalStored--;

            yield return new WaitForSeconds(0.1f);
        }
        isReplaying = false;
    }

}

