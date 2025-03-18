using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D player;
    


    [Header("Particle Effects")]
    [Range(0, 10)]
    public int occurAfterVelocity;
    private float particleCounter;
    [Range(0, 0.8f)]
    public float dustFormationPeriod;
    [SerializeField] ParticleSystem dustParticle;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
    }

    // Call this to start running animation
    public void SetIsRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
        if (isRunning)
        {
            particleCounter += Time.deltaTime;
            if (!AudioManager.Instance.enemyRunSound.isPlaying) // Prevent restarting on every frame
            {
                AudioManager.Instance.enemyRunSound.loop = true; // Ensure it loops
                AudioManager.Instance.enemyRunSound.Play();
                //dustParticle.Play();             
            }
            if (Mathf.Abs(player.linearVelocityX) > occurAfterVelocity)
            {
                if (particleCounter > dustFormationPeriod)
                {
                    dustParticle.Play();
                    particleCounter = 0;
                }
            }
            //if (particleCounter > dustFormationPeriod)
            //{
            //    dustParticle.Play();
            //    particleCounter = 0;
            //}
        }
        else
        {
            AudioManager.Instance.enemyRunSound.loop = false; // Stop looping
            AudioManager.Instance.enemyRunSound.Stop();
        }

    }

    // Call this to trigger the jump animation
    public void SetIsJumping(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
        if (isJumping)
        {
            AudioManager.Instance.PlayJumpSound();
        }

    }

    public void SetIsClimbing (bool isClimbing)
    {
        animator.SetBool("IsClimbing", isClimbing);
        if (isClimbing)
        {
            animator.Play("sh_climb");  // Make sure "Climb" exists in the Animator
            if (!AudioManager.Instance.climbSound.isPlaying) // Prevent restarting on every frame
            {
                AudioManager.Instance.climbSound.loop = true; // Ensure it loops
                AudioManager.Instance.climbSound.Play();
            }
        }
        else
        {
            AudioManager.Instance.climbSound.loop = false; // Stop looping
            AudioManager.Instance.climbSound.Stop();
        }

    }

    // Call this to play death animation
    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
        AudioManager.Instance.PlayDieSound();
    }

    public void SetIsDashing(bool isDashing)
    {
        animator.SetBool("IsDashing", isDashing);
    }

    public void PlayDashAnimation()
    {
        animator.SetTrigger("Dash");
    }

    public void SetAnimatorSpeed(float speed)
    {
        Animator anim = GetComponent<Animator>();
        anim.speed = speed; // This sets the animation speed (0 pauses, 1 resumes)
    }

    public void SetIsWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }


}
