using UnityEngine;

public class PlayerKillTracker : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerSkillsManager skillsManager;

    private int currentKills;

    public void RegisterKill()
    {
        int requiredKills = skillsManager.GetKillsRequiredForHeal();

        if (requiredKills == int.MaxValue)
            return;

        currentKills++;

        Debug.Log("Bajas: " + currentKills + "/" + requiredKills);

        if (currentKills >= requiredKills)
        {
            currentKills = 0;

            playerHealth.Heal(10);

            Debug.Log("Que sigan viniendo activada : +10 HP");
        }
    }
}
