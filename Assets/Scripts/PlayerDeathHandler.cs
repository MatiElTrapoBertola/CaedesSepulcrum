using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement3D playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private float deathDelay = 3f;

    private bool isDead;

    public void HandleDeath()
    {
        if (isDead)
            return;

        isDead = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        animator.SetTrigger("Dying");

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);

        SceneManager.LoadScene("Defeat");
    }
}