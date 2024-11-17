using UnityEngine;
using UnityEditor;
using System.Linq; // Importar para usar métodos de consulta

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private int selectedEnemyIndex = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemySpawner spawner = (EnemySpawner)target;

        if (spawner.enemySpawnSettings != null && spawner.enemySpawnSettings.Count > 0)
        {
            string[] options = spawner.enemySpawnSettings
                .Select(settings => settings.enemyName)
                .ToArray();

            if (selectedEnemyIndex < 0 || selectedEnemyIndex >= options.Length)
            {
                selectedEnemyIndex = 0;
            }

            selectedEnemyIndex = EditorGUILayout.Popup("Select Enemy", selectedEnemyIndex, options);
        }
        else
        {
            EditorGUILayout.LabelField("No enemy spawn settings available.");
        }
    }

    private void OnSceneGUI()
    {
        EnemySpawner spawner = (EnemySpawner)target;
        Handles.color = Color.green;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (spawner.enemySpawnSettings != null && spawner.enemySpawnSettings.Count > 0)
                {
                    spawner.enemySpawnSettings[selectedEnemyIndex].spawnPositions.Add(hit.point);
                    EditorUtility.SetDirty(spawner);
                }
            }
            Event.current.Use();
        }

        foreach (var settings in spawner.enemySpawnSettings)
        {
            if (settings.spawnPositions != null)
            {
                foreach (var pos in settings.spawnPositions)
                {
                    Handles.SphereHandleCap(0, pos, Quaternion.identity, 0.5f, EventType.Repaint);
                }
            }
        }
    }
}
