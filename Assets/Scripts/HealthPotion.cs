using UnityEngine;



public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;

    [Header("Audio")]
    [SerializeField][Range(0f, 2f)] private float pickupVolume = 1f;
    [SerializeField] private AudioClip[] pickupClips;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;


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

        PlayPickupSound();

        Destroy(gameObject);
    }

    private void PlayPickupSound()
    {
        if (pickupClips == null || pickupClips.Length == 0)
            return;

        AudioClip selectedClip = pickupClips[Random.Range(0, pickupClips.Length)];

        AudioSource.PlayClipAtPoint(selectedClip, transform.position, pickupVolume);


    }
}