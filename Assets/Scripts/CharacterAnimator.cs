using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Call this to start running animation
    public void SetIsRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }

    // Call this to trigger the jump animation
    public void SetIsJumping(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }

    public void SetIsClimbing (bool isClimbing)
    {
        animator.SetBool("IsClimbing", isClimbing);
        if (isClimbing)
        {
            animator.Play("sh_climb");  // Make sure "Climb" exists in the Animator
        }
    }

    // Call this to play death animation
    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
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
