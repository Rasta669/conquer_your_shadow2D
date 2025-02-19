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

    // Call this to play death animation
    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
    }
}
