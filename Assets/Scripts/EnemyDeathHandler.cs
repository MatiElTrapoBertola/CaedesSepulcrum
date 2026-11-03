using UnityEngine;
// Este script es para manejar la muerte de enemigos, incluyendo la generación de loot y la desactivación de colisiones
[RequireComponent(typeof(Health))]
public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject lootPrefab; // Prefab de huesos/recursos
    [SerializeField] private int resourceDropAmount = 2;

    [Header("Enemy Death Audio")]
    [SerializeField][Range(0f, 2f)] private float deathVolume= 1f;
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
        if (deathClips == null || deathClips.Length == 0) return;

        AudioClip selectedClip = deathClips[Random.Range(0, deathClips.Length)];

        GameObject tempAudio = new GameObject("EnemyDeathAudio");

        tempAudio.transform.position = transform.position;

        AudioSource source = tempAudio.AddComponent<AudioSource>();

        source.spatialBlend = 1f;

        source.pitch = Random.Range(minPitch, maxPitch);

        source.PlayOneShot(selectedClip, deathVolume);

        Destroy(tempAudio, selectedClip.length);
    }
    private void HandleDeath()
    {
        PlayDeathSound();

        // Dropear huesos o carne podrida
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

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
}