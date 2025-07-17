// VertexCounterWindow.cs
// Editor script para mostrar estadísticas de vértices y LODs

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class VertexCounterWindow : EditorWindow
{
    private Vector2 scroll;
    private List<(string name, int vertexCount, int triangleCount, int lodCount)> results = new();
    private string searchFilter = "";
    private bool clearConsoleOnRun = true;
    private bool showOnlyFiltered = false;
    private bool countOnlySelected = false;

    [MenuItem("Tools/Contador de Vértices Avanzado")]
    public static void ShowWindow()
    {
        GetWindow<VertexCounterWindow>("Contador de Vértices");
    }

    void OnGUI()
    {
        GUILayout.Label("Opciones", EditorStyles.boldLabel);

        clearConsoleOnRun = EditorGUILayout.Toggle("Limpiar consola al contar", clearConsoleOnRun);
        countOnlySelected = EditorGUILayout.Toggle("Contar solo objetos seleccionados", countOnlySelected);
        searchFilter = EditorGUILayout.TextField("Filtrar por nombre", searchFilter);
        showOnlyFiltered = EditorGUILayout.Toggle("Mostrar solo filtrados", showOnlyFiltered);

        if (GUILayout.Button("Contar vértices"))
        {
            if (clearConsoleOnRun) Debug.ClearDeveloperConsole();
            ContarVertices();
        }

        GUILayout.Space(10);
        GUILayout.Label("Resultados", EditorStyles.boldLabel);

        scroll = GUILayout.BeginScrollView(scroll);
        foreach (var (name, vtx, tri, lods) in results)
        {
            if (showOnlyFiltered && !name.ToLower().Contains(searchFilter.ToLower()))
                continue;

            GUILayout.Label($"{name}: {vtx} vértices, {tri} triángulos{(lods > 0 ? $", {lods} LODs" : "")}");
        }
        GUILayout.EndScrollView();
    }

    void ContarVertices()
    {
        results.Clear();

        GameObject[] objectsToCount = countOnlySelected ? Selection.gameObjects : GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in objectsToCount)
        {
            var mf = obj.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                int vertexCount = mf.sharedMesh.vertexCount;
                int triangleCount = mf.sharedMesh.triangles.Length / 3;

                int lodCount = 0;
                var lodGroup = obj.GetComponent<LODGroup>();
                if (lodGroup != null)
                {
                    lodCount = lodGroup.GetLODs().Length;
                }

                results.Add((obj.name, vertexCount, triangleCount, lodCount));
                Debug.Log($"🧩 {obj.name}: {vertexCount} vértices, {triangleCount} triángulos{(lodCount > 0 ? $", {lodCount} LODs" : "")}");
            }
        }

        results.Sort((a, b) => b.vertexCount.CompareTo(a.vertexCount));
        Debug.Log($"✅ Se encontraron {results.Count} objetos con mallas.");
    }
}
