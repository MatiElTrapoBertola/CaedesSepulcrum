using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entro en la pocion: " + other.name);

        Health playerHealth =
            other.GetComponent<Health>();

        if (playerHealth == null)
            return;

        PlayerSkillsManager skillsManager =
            other.GetComponent<PlayerSkillsManager>();

        int bonusHealing = 0;

        if (skillsManager != null)
        {
            bonusHealing =
                skillsManager.GetPotionHealingBonus();
        }

        int finalHealing =
            healAmount + bonusHealing;

        playerHealth.Heal(finalHealing);

        Debug.Log(
            "Poción usada. Curación: " +
            finalHealing);

        Destroy(gameObject);
    }
}