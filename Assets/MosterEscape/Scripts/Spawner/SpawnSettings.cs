using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnSettings
{
    public string Name;
    public GameObject prefab;
    public List<Vector3> spawnPositions;
    public float respawnTime;
}