using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class EnemySpawnSettings : SpawnSettings
{

}

public class EnemySpawner : Spawner<EnemySpawnSettings, EnemyController>, ISpawner
{
    public List<EnemySpawnSettings> enemySpawnSettings;

    protected override List<EnemySpawnSettings> GetSpawnSettings()
    {
        return enemySpawnSettings;
    }

    List<SpawnSettings> ISpawner.GetSpawnSettings()
    {
        return enemySpawnSettings.Cast<SpawnSettings>().ToList();
    }

    protected override void OnObjectCollectedOrDestroyed(GameObject obj, EnemySpawnSettings settings, int spawnIndex)
    {
        EnemyController enemyComponent = obj.GetComponent<EnemyController>();
        if (enemyComponent != null)
        {
            enemyComponent.OnDestroyed += () =>
            {
                StartRespawnCoroutine(obj, settings, spawnIndex);
            };
        }
    }
}
