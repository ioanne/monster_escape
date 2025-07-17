using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityMeshSimplifier;
using System.IO;
using System.Collections.Generic;

public class LODGenerator : EditorWindow
{
    private int lodCount = 3;
    private float minQuality = 0.5f;
    private float[] qualityLevels;
    private float[] screenHeights;
    private bool autoAdjustQualities = false;
    private bool forceLastLODAlwaysVisible = true;
    private int minVertexCount = 100;
    private bool applyToAllMatching = false;

    [MenuItem("Tools/Generar LODs")]
    public static void ShowWindow()
    {
        GetWindow<LODGenerator>("Generar LODs");
    }

    void OnEnable()
    {
        CalculateQualityLevels();
        CalculateScreenHeights();
    }

    void OnGUI()
    {
        GUILayout.Label("Configuración de LODs", EditorStyles.boldLabel);

        int newLodCount = EditorGUILayout.IntSlider("Cantidad de LODs", lodCount, 2, 8);
        float newMinQuality = EditorGUILayout.Slider("Calidad mínima (último LOD)", minQuality, 0.05f, 1.0f);

        if (newLodCount != lodCount || Mathf.Abs(newMinQuality - minQuality) > 0.001f)
        {
            lodCount = newLodCount;
            minQuality = newMinQuality;
            CalculateQualityLevels();
            CalculateScreenHeights();
        }

        minVertexCount = EditorGUILayout.IntField("Mínimo de vértices", minVertexCount);
        autoAdjustQualities = EditorGUILayout.Toggle("Autoajustar calidades", autoAdjustQualities);
        forceLastLODAlwaysVisible = EditorGUILayout.Toggle("Evitar que desaparezca el último LOD", forceLastLODAlwaysVisible);

        if (!autoAdjustQualities)
        {
            for (int i = 0; i < lodCount; i++)
            {
                qualityLevels[i] = EditorGUILayout.Slider($"LOD{i} calidad", qualityLevels[i], 0.05f, 1.0f);
            }
        }
        else
        {
            CalculateQualityLevels();
        }

        for (int i = 0; i < lodCount; i++)
        {
            screenHeights[i] = EditorGUILayout.Slider($"LOD{i} pantalla %", screenHeights[i], 0.0001f, 1.0f);
        }

        GUILayout.Space(10);
        GUILayout.Label("Modo prueba rápida", EditorStyles.boldLabel);

        if (Selection.activeGameObject != null)
        {
            applyToAllMatching = EditorGUILayout.Toggle("Aplicar a todos los objetos con el mismo mesh", applyToAllMatching);

            if (GUILayout.Button($"Aplicar LOD {(applyToAllMatching ? "a todos los iguales a" : "solo a")} '{Selection.activeGameObject.name}'"))
            {
                var selectedMesh = Selection.activeGameObject.GetComponent<MeshFilter>()?.sharedMesh;
                if (selectedMesh != null && applyToAllMatching)
                {
                    int count = 0;
                    var allObjects = new List<GameObject>(GameObject.FindObjectsOfType<GameObject>());
                    foreach (var obj in allObjects)
                    {
                        if (obj == null) continue;

                        var meshFilter = obj.GetComponent<MeshFilter>();
                        if (meshFilter != null && meshFilter.sharedMesh == selectedMesh)
                        {
                            if (ApplyLODToSingleObject(obj)) count++;
                        }
                    }
                    Debug.Log($"✔ {count} objetos con el mismo mesh procesados.");
                }
                else
                {
                    ApplyLODToSingleObject(Selection.activeGameObject);
                }

                var mf = Selection.activeGameObject.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                {
                    int vertexCount = mf.sharedMesh.vertexCount;
                    Debug.Log($"🔍 '{Selection.activeGameObject.name}' tiene {vertexCount} vértices en su malla.");
                }
            }

            if (GUILayout.Button($"Limpiar LODs de '{Selection.activeGameObject.name}'"))
            {
                var selectedMesh = Selection.activeGameObject.GetComponent<MeshFilter>()?.sharedMesh;
                if (selectedMesh != null && applyToAllMatching)
                {
                    int count = 0;
                    var allObjects = new List<GameObject>(GameObject.FindObjectsOfType<GameObject>());
                    foreach (var obj in allObjects)
                    {
                        if (obj == null) continue;

                        var meshFilter = obj.GetComponent<MeshFilter>();
                        if (meshFilter != null && meshFilter.sharedMesh == selectedMesh)
                        {
                            if (ClearLODs(obj)) count++;
                        }
                    }
                    Debug.Log($"🧹 {count} objetos con LODs eliminados.");
                }
                else
                {
                    ClearLODs(Selection.activeGameObject);
                }
            }
        }
        else
        {
            GUILayout.Label("Seleccioná un objeto en la jerarquía para aplicar o limpiar LOD.");
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Forzar que el último LOD siempre se vea"))
        {
            ForceAllLODsVisible();
        }
    }

    void CalculateQualityLevels()
    {
        qualityLevels = new float[lodCount];
        for (int i = 0; i < lodCount; i++)
        {
            float t = i / (float)(lodCount - 1);
            qualityLevels[i] = Mathf.Lerp(1.0f, minQuality, t);
        }
    }

    void CalculateScreenHeights()
    {
        screenHeights = new float[lodCount];
        for (int i = 0; i < lodCount; i++)
        {
            screenHeights[i] = Mathf.Max(0.0001f, 0.6f / Mathf.Pow(2f, i));
        }
    }

    bool ApplyLODToSingleObject(GameObject obj)
    {
        if (obj == null || !obj.activeInHierarchy) return false;

        var existingLod = obj.GetComponent<LODGroup>();
        if (existingLod != null)
        {
            ClearLODs(obj);
        }

        var mf = obj.GetComponent<MeshFilter>();
        var mr = obj.GetComponent<MeshRenderer>();
        if (mf == null || mr == null || mf.sharedMesh == null) return false;

        Mesh originalMesh = mf.sharedMesh;
        List<Renderer> renderers = new List<Renderer> { mr };
        Mesh previousMesh = originalMesh;

        for (int i = 1; i < lodCount; i++)
        {
            float quality = qualityLevels[i];
            Mesh simplifiedMesh = SimplifyMesh(originalMesh, quality, minVertexCount);
            if (simplifiedMesh.vertexCount >= previousMesh.vertexCount)
            {
                simplifiedMesh = previousMesh;
            }
            GameObject lodChild = CreateLODChild(obj, simplifiedMesh, $"LOD{i}");
            renderers.Add(lodChild.GetComponent<Renderer>());
            previousMesh = simplifiedMesh;
        }

        LOD[] lods = new LOD[lodCount];
        for (int i = 0; i < lodCount; i++)
        {
            float height = screenHeights[i];
            if (forceLastLODAlwaysVisible && i == lodCount - 1)
                height = 0.0001f;

            lods[i] = new LOD(height, new[] { renderers[i] });
        }

        var lodGroup = obj.AddComponent<LODGroup>();
        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        return true;
    }

    void ForceAllLODsVisible()
    {
        int fixedCount = 0;
        foreach (var lodGroup in GameObject.FindObjectsOfType<LODGroup>())
        {
            if (lodGroup == null) continue;
            var lods = lodGroup.GetLODs();
            if (lods.Length == 0) continue;

            lods[lods.Length - 1].screenRelativeTransitionHeight = 0.0001f;
            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
            fixedCount++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log($"🔧 LODs corregidos para permanecer visibles en {fixedCount} objetos.");
    }

    Mesh SimplifyMesh(Mesh original, float quality, int minVertexCount)
    {
        var simplifier = new MeshSimplifier();
        simplifier.Initialize(original);
        simplifier.SimplifyMesh(quality);

        Mesh result = simplifier.ToMesh();
        if (result.vertexCount < minVertexCount)
        {
            Debug.LogWarning($"⚠ Simplificación excesiva evitada en '{original.name}', resultado tenía solo {result.vertexCount} vértices");
            return original;
        }

        return result;
    }

    GameObject CreateLODChild(GameObject parent, Mesh mesh, string name)
    {
        GameObject child = new GameObject(name);
        child.transform.parent = parent.transform;
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;

        var mf = child.AddComponent<MeshFilter>();
        var mr = child.AddComponent<MeshRenderer>();
        mf.sharedMesh = mesh;
        mr.sharedMaterials = parent.GetComponent<MeshRenderer>().sharedMaterials;

        return child;
    }

    bool ClearLODs(GameObject obj)
    {
        if (obj == null) return false;

        var lod = obj.GetComponent<LODGroup>();
        if (lod == null) return false;

        DestroyImmediate(lod);

        for (int i = obj.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = obj.transform.GetChild(i);
            if (child != null && child.name.StartsWith("LOD"))
            {
                DestroyImmediate(child.gameObject);
            }
        }

        return true;
    }
}
