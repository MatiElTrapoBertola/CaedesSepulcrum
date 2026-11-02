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

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 5f;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    private int currentWave = 0;
    private bool waveActive = false;
    
    private void Start()
    {
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2f);

        StartWave1();
    }

    private void Update()
    {
        if (!waveActive)
            return;

        aliveEnemies.RemoveAll(enemy => enemy == null);

        if (aliveEnemies.Count == 0)
        {
            waveActive = false;

            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextWave()
    {
        Debug.Log("Oleada completada. Siguiente oleada en " + timeBetweenWaves + " segundos");

        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;

        switch (currentWave)
        {
            case 1: StartWave2();
                break;

            case 2: StartWave3();
                break;

            case 3: Debug.Log("Nivel completado");
                break;

            
        }
    }

    private void StartWave1()
    {
        Debug.Log("Iniciando oleada 1");

        SpawnEnemies(normalEnemy, 5);

        waveActive = true;
    }

    private void StartWave2()
    {
        Debug.Log("Iniciando Oleada 2");

        SpawnEnemies(normalEnemy, 8);
        SpawnEnemies(strongEnemy, 2);
    }

    private void StartWave3()
    {
        Debug.Log("Iniciando Oleada 3");

        SpawnEnemies(normalEnemy, 10);
        SpawnEnemies(strongEnemy, 4);
        SpawnEnemies(bossEnemy, 1);

        waveActive = true;
    }

    private void SpawnEnemies(GameObject enemyPrefab, int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            EnemyPortalSpawner portal = portals[Random.Range(0, portals.Length)];

            portal.PlayPortalEffect();

            GameObject enemy = Instantiate(enemyPrefab, portal.transform.position, Quaternion.identity);

            aliveEnemies.Add(enemy);
        }
    }
}
