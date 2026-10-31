using UnityEngine;
// Este script es para manejar la muerte de enemigos, incluyendo la generación de loot y la desactivación de colisiones
[RequireComponent(typeof(Health))]
public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject lootPrefab; // Prefab de huesos/recursos
    [SerializeField] private int resourceDropAmount = 2;

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

    private void HandleDeath()
    {
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