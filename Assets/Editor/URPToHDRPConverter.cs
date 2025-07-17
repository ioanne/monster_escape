using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class URPToHDRPConverter : EditorWindow
{
    [MenuItem("Tools/Convert Materials URP → HDRP")]
    public static void ShowWindow()
    {
        GetWindow<URPToHDRPConverter>("URP to HDRP Converter");
    }

    private Material hdrpTemplate;

    void OnGUI()
    {
        GUILayout.Label("Conversión de materiales URP a HDRP", EditorStyles.boldLabel);
        
        hdrpTemplate = (Material)EditorGUILayout.ObjectField("Material base HDRP (Lit)", hdrpTemplate, typeof(Material), false);

        if (GUILayout.Button("Convertir todos los materiales"))
        {
            ConvertAllMaterials();
        }
    }

    void ConvertAllMaterials()
    {
        if (hdrpTemplate == null)
        {
            Debug.LogError("⚠️ Asigná un material HDRP/Lit como plantilla.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat.shader.name.Contains("Universal Render Pipeline") || mat.shader.name.Contains("URP"))
            {
                Material newMat = new Material(hdrpTemplate);
                newMat.CopyPropertiesFromMaterial(mat);
                
                // Overwrite the original material
                EditorUtility.CopySerialized(newMat, mat);
                EditorUtility.SetDirty(mat);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Se convirtieron {count} materiales de URP a HDRP.");
    }
}
