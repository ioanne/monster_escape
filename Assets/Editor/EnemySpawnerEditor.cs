using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemySpawner spawner = (EnemySpawner)target;
        Handles.color = Color.green;

        // Permitir que se agreguen puntos de spawn haciendo clic en la escena
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Agregar el punto de spawn al primer EnemySpawnSettings de la lista
                if (spawner.enemySpawnSettings != null && spawner.enemySpawnSettings.Count > 0)
                {
                    spawner.enemySpawnSettings[0].spawnPositions.Add(hit.point);
                    EditorUtility.SetDirty(spawner); // Marcar el objeto como modificado
                }
            }
            Event.current.Use();
        }

        // Dibujar esferas en los puntos de spawn
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
