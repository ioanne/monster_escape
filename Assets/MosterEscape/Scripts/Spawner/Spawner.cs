using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Spawner<TSettings, TController> : MonoBehaviour
    where TSettings : SpawnSettings
    where TController : MonoBehaviour
{
    private Dictionary<GameObject, Coroutine> respawnCoroutines = new Dictionary<GameObject, Coroutine>();

    protected abstract List<TSettings> GetSpawnSettings();
    protected abstract void OnObjectCollectedOrDestroyed(GameObject obj, TSettings settings, int spawnIndex);

    private void Start()
    {
        foreach (var settings in GetSpawnSettings())
        {
            for (int i = 0; i < settings.spawnPositions.Count; i++)
            {
                SpawnObject(settings, i);
            }
        }
    }

    private void SpawnObject(TSettings settings, int spawnIndex)
    {
        if (settings.spawnPositions.Count == 0)
        {
            Debug.LogWarning("Spawn positions list is empty.");
            return;
        }

        Vector3 spawnPosition = settings.spawnPositions[spawnIndex];

        if (settings.prefab == null)
        {
            Debug.LogWarning("Prefab is null.");
            return;
        }

        GameObject newObject = Instantiate(settings.prefab, spawnPosition, Quaternion.identity);
        newObject.SetActive(true);
        Debug.Log("Spawned object at position: " + spawnPosition);

        TController controller = newObject.GetComponent<TController>();
        if (controller != null)
        {
            OnObjectCollectedOrDestroyed(newObject, settings, spawnIndex);
        }
        else
        {
            Debug.LogWarning("Controller component not found on the spawned object.");
        }
    }

    protected void StartRespawnCoroutine(GameObject obj, TSettings settings, int spawnIndex)
    {
        if (respawnCoroutines.ContainsKey(obj))
            StopCoroutine(respawnCoroutines[obj]);

        respawnCoroutines[obj] = StartCoroutine(RespawnObject(settings, spawnIndex));
    }

    private IEnumerator RespawnObject(TSettings settings, int spawnIndex)
    {
        yield return new WaitForSeconds(settings.respawnTime);
        SpawnObject(settings, spawnIndex);
    }
}
