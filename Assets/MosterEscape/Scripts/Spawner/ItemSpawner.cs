using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ItemSpawnSettings : SpawnSettings
{
}

public class ItemSpawner : Spawner<ItemSpawnSettings, ItemController>, ISpawner
{
    public List<ItemSpawnSettings> itemSpawnSettings;

    protected override List<ItemSpawnSettings> GetSpawnSettings()
    {
        return itemSpawnSettings;
    }

    List<SpawnSettings> ISpawner.GetSpawnSettings()
    {
        return itemSpawnSettings.Cast<SpawnSettings>().ToList();
    }

    protected override void OnObjectCollectedOrDestroyed(GameObject obj, ItemSpawnSettings settings, int spawnIndex)
    {
        // ItemController itemComponent = obj.GetComponent<ItemController>();
        // if (itemComponent != null)
        // {
        //     itemComponent.OnCollected += () =>
        //     {
        //         StartRespawnCoroutine(obj, settings, spawnIndex);
        //     };
        // }
    }
}
