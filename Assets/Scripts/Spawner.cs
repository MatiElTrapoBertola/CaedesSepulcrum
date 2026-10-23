using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnPoint = new List<GameObject>();

    [SerializeField] private int maxEnemies = 20;   // NUEVO: enemigos totales de la ronda
    private int spawned = 0;
    [SerializeField] float timer = 0;
    [SerializeField] float timeToSpawn;

    void Update()
    {
        if (spawned >= maxEnemies) return;
        timer += Time.deltaTime;
        if (timer >= timeToSpawn)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
    void SpawnEnemy()
    {
        int random = Random.Range(0, enemies.Count);
        int random2 = Random.Range(0, spawnPoint.Count);
        Instantiate(enemies[random], spawnPoint[random2].transform.position, transform.rotation);
        spawned++;
    }

   
}
