using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Portals")]
    [SerializeField] private EnemyPortalSpawner[] portals;

    [Header("Enemies")]
    [SerializeField] private GameObject normalEnemy;
    [SerializeField] private GameObject strongEnemy;
    [SerializeField] private GameObject bossEnemy;

    [Header("Enemy Counts")]
    [SerializeField] private int normalEnemyCount = 30;
    [SerializeField] private int strongEnemyCount = 0;

    [Header("Boss")]
    [SerializeField] private bool spawnBoss = false;
    [SerializeField] private float bossSpawnDelay = 10f;

    [Header("Spawn Settings")]
    [SerializeField] private float portalDelay = 1f;
    [SerializeField] private float minSpawnInterval = 0.25f;
    [SerializeField] private float maxSpawnInterval = 1f;
    [SerializeField] private float spawnRadius = 3f;

    [Header("Initialize controls")]
    private float spawnTimer = 0f;
    private bool waveStarted = false;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();
    private bool waveFinished = false;
    private int activeSpawnCoroutines = 0;

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        OpenAllPortals();

        yield return new WaitForSeconds(portalDelay);

        if (normalEnemy != null && normalEnemyCount > 0)
        {
            SpawnEnemies(normalEnemy, normalEnemyCount);
        }

        if (strongEnemy != null && strongEnemyCount > 0)
        {
            SpawnEnemies(strongEnemy, strongEnemyCount);
        }

        if (spawnBoss && bossEnemy != null)
        {
            StartCoroutine(SpawnBossAfterDelay());
        }
    }

    private void Update()
    {
        if (!waveStarted)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 3f)
            {
                waveStarted = true;
            }
            return;
        }

        if (waveFinished) return;

        aliveEnemies.RemoveAll(enemy => enemy == null);

        bool hasSpawnedAll = (activeSpawnCoroutines == 0);

        if (waveStarted && hasSpawnedAll && aliveEnemies.Count == 0)
        {
            int totalExpectedEnemies = normalEnemyCount + strongEnemyCount + (spawnBoss ? 1 : 0);

            if (totalExpectedEnemies > 0)
            {
                waveFinished = true;
                Debug.Log("¡Todos los enemigos derrotados! Transicionando a la Iglesia en 4 segundos...");


                StartCoroutine(DelayedTransitionRoutine());
            }
        }
    }

    private IEnumerator DelayedTransitionRoutine()
    {
        yield return new WaitForSeconds(4f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnNightCompleted();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ChurchScene");
        }
    }

    private void OpenAllPortals()
    {
        foreach (EnemyPortalSpawner portal in portals)
        {
            if (portal != null)
            {
                portal.PlayPortalEffect();
            }
        }
    }

    private void SpawnEnemies(GameObject enemyPrefab, int amount)
    {
        StartCoroutine(SpawnEnemiesCoroutine(enemyPrefab, amount));
    }

    private IEnumerator SpawnEnemiesCoroutine(GameObject enemyPrefab, int amount)
    {
        activeSpawnCoroutines++;

        for (int i = 0; i < amount; i++)
        {
            EnemyPortalSpawner spawnPortal = portals[Random.Range(0, portals.Length)];

            if (spawnPortal != null)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
                Vector3 spawnPosition = spawnPortal.transform.position + randomOffset;

                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                if (!enemy.CompareTag("Enemy"))
                {
                    enemy.tag = "Enemy";
                }

                aliveEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
        }

        activeSpawnCoroutines--;
    }

    private IEnumerator SpawnBossAfterDelay()
    {
        yield return new WaitForSeconds(bossSpawnDelay);

        OpenAllPortals();

        yield return new WaitForSeconds(portalDelay);

        EnemyPortalSpawner bossPortal = portals[Random.Range(0, portals.Length)];

        if (bossPortal != null)
        {
            GameObject boss = Instantiate(bossEnemy, bossPortal.transform.position, Quaternion.identity);

            if (!boss.CompareTag("Enemy"))
            {
                boss.tag = "Enemy";
            }

            aliveEnemies.Add(boss);

            Debug.Log("El jefe ha llegado");
        }
    }

    public bool IsWaveFinished()
    {
        return waveFinished;
    }
}