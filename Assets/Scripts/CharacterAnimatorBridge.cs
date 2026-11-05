using UnityEngine;

public class CharacterAnimatorBridge : MonoBehaviour
{
    private Animator animator;
    private Health health;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if(health != null)
        {
            health.onTakeDamage.AddListener(PlayHitAnimation);
            health.onDeath.AddListener(PlayDeathAnimation);
        }
    }

    private void OnDisable()
    {
        if(health != null)
        {
            health.onTakeDamage.RemoveListener(PlayHitAnimation);
            health.onDeath.RemoveListener(PlayDeathAnimation);
        }
    }

    private void PlayHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    private void PlayDeathAnimation()
    {
        if(animator != null)
        {
            int deathVariant = Random.Range(0, 2);
            animator.SetInteger("DeathIndex", deathVariant);

            animator.SetBool("Dead", true);
        }
    }


}
