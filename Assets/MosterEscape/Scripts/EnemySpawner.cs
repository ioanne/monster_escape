using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public GameObject enemyPrefab;
    public List<Vector3> spawnPositions;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int maxEnemies = 3;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        // Revisar si algún enemigo ha sido destruido y spawnear uno nuevo
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPositions.Count == 0) return;

        // Seleccionar una posición de spawn aleatoria de la lista
        int spawnIndex = Random.Range(0, spawnPositions.Count);
        Vector3 spawnPosition = spawnPositions[spawnIndex];

        // Instanciar el enemigo en la posición de spawn
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(newEnemy);
    }
}
