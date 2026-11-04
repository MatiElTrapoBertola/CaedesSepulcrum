using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject lootPrefab; 
    [SerializeField] private int resourceDropAmount = 2;

    [Header("Healing Drop Chance")]
    [SerializeField] private GameObject healthPotionPrefab; 
    [Range(0f, 100f)][SerializeField] private float potionDropChance = 35f;
    [SerializeField] private float dropRadius = 2f;

    [Header("Enemy Death Audio")]
    [SerializeField][Range(0f, 2f)] private float deathVolume = 1f;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.onDeath.AddListener(HandleDeath);
    }

    private void OnDestroy()
    {
        health.onDeath.RemoveListener(HandleDeath);
    }

    private void PlayDeathSound()
    {
        if (deathClips == null || deathClips.Length == 0)
            return;

        AudioClip selectedClip = deathClips[Random.Range(0, deathClips.Length)];
        float randomPitch = Random.Range(minPitch, maxPitch);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(selectedClip, deathVolume, randomPitch);
        }
    }

    private void HandleDeath()
    {
        PlayDeathSound();

     
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

       
        DropHealthPotion();

      
        PlayerResources playerResources = FindFirstObjectByType<PlayerResources>();

        if (playerResources != null)
        {
            PlayerSkillsManager skillsManager = FindFirstObjectByType<PlayerSkillsManager>();
            float materialMultiplier = 1f;

            if (skillsManager != null)
            {
                materialMultiplier = skillsManager.GetMaterialMultiplier();
            }

            int finalMaterials = Mathf.RoundToInt(resourceDropAmount * materialMultiplier);
            playerResources.AddMaterials(finalMaterials);

            Debug.Log("Materiales obtenidos: " + finalMaterials);
        }

        GetComponent<Collider>().enabled = false;

        PlayerKillTracker killTracker = FindFirstObjectByType<PlayerKillTracker>();

        if (killTracker != null)
        {
            killTracker.RegisterKill();
        }

        Destroy(gameObject, 0.1f);
    }

    private void DropHealthPotion()
    {
        if (healthPotionPrefab == null) return;

        float roll = Random.Range(0f, 100f);

        if (roll <= potionDropChance)
        {
          
            Vector2 randomPoint = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomPoint.x, 0.5f, randomPoint.y);

           
            Instantiate(healthPotionPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("¡El enemigo soltó una poción de curación dispersa!");
        }
    }

}