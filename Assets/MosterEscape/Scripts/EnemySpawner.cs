using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnSettings
{
    public GameObject enemyPrefab;
    public List<Vector3> spawnPositions;
    public float respawnTime;
}

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public List<EnemySpawnSettings> enemySpawnSettings;
    private Dictionary<GameObject, Coroutine> respawnCoroutines = new Dictionary<GameObject, Coroutine>();

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
        foreach (var settings in enemySpawnSettings)
        {
            for (int i = 0; i < settings.spawnPositions.Count; i++)
            {
                SpawnEnemy(settings, i);
            }
        }
    }

    private void SpawnEnemy(EnemySpawnSettings settings, int spawnIndex)
    {
        if (settings.spawnPositions.Count == 0)
        {
            Debug.LogWarning("Spawn positions list is empty.");
            return;
        }

        // Obtener la posición de spawn específica
        Vector3 spawnPosition = settings.spawnPositions[spawnIndex];

        if (settings.enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is null.");
            return;
        }

        // Instanciar el enemigo en la posición de spawn
        GameObject newEnemy = Instantiate(settings.enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.SetActive(true); // Activar el enemigo
        Debug.Log("Spawned enemy at position: " + spawnPosition);

        // Obtener el componente Enemy y suscribirse al evento OnDestroyed
        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.OnDestroyed += () =>
            {
                // Iniciar la corrutina de reaparición cuando el enemigo muere
                if (respawnCoroutines.ContainsKey(newEnemy))
                    StopCoroutine(respawnCoroutines[newEnemy]);

                respawnCoroutines[newEnemy] = StartCoroutine(RespawnEnemy(settings, spawnIndex));
            };
        }
        else
        {
            Debug.LogWarning("Enemy component not found on the spawned enemy.");
        }
    }

    private IEnumerator RespawnEnemy(EnemySpawnSettings settings, int spawnIndex)
    {
        yield return new WaitForSeconds(settings.respawnTime);
        SpawnEnemy(settings, spawnIndex);
    }
}
