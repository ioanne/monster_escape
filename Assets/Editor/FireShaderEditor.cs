using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

public class FireEffectGenerator : EditorWindow
{
    private Texture2D fireTexture;
    private Texture2D noiseTexture;
    private Color fireColor = new Color(1f, 0.5f, 0.2f);
    private float fireIntensity = 5f;
    private float planeHeight = 1.5f;
    private float planeWidth = 1.0f;
    private bool doubleSided = true;

    private bool addLight = true;
    private float lightIntensity = 800f;
    private float lightRange = 8f;
    private Color lightColor = new Color(1f, 0.5f, 0.2f);

    [MenuItem("Tools/Agregar Efecto de Fuego")]
    public static void ShowWindow()
    {
        GetWindow<FireEffectGenerator>("Generador de Fuego");
    }

    void OnGUI()
    {
        GUILayout.Label("Texturas del Fuego", EditorStyles.boldLabel);
        fireTexture = (Texture2D)EditorGUILayout.ObjectField("Textura de Fuego", fireTexture, typeof(Texture2D), false);
        noiseTexture = (Texture2D)EditorGUILayout.ObjectField("Textura de Ruido", noiseTexture, typeof(Texture2D), false);

        GUILayout.Space(10);
        GUILayout.Label("Configuración del Shader", EditorStyles.boldLabel);
        fireColor = EditorGUILayout.ColorField("Color del Fuego", fireColor);
        fireIntensity = EditorGUILayout.Slider("Intensidad Emisión", fireIntensity, 0f, 50f);
        doubleSided = EditorGUILayout.Toggle("Plano Doble Cara", doubleSided);
        planeWidth = EditorGUILayout.Slider("Ancho del Plano", planeWidth, 0.1f, 5f);
        planeHeight = EditorGUILayout.Slider("Alto del Plano", planeHeight, 0.1f, 5f);

        GUILayout.Space(10);
        GUILayout.Label("Configuración de Luz", EditorStyles.boldLabel);
        addLight = EditorGUILayout.Toggle("Agregar Luz", addLight);
        if (addLight)
        {
            lightColor = EditorGUILayout.ColorField("Color de Luz", lightColor);
            lightIntensity = EditorGUILayout.Slider("Intensidad de Luz (lm)", lightIntensity, 0f, 40000f);
            lightRange = EditorGUILayout.Slider("Rango de Luz", lightRange, 1f, 20f);
        }

        GUILayout.Space(10);

        if (Selection.activeGameObject == null)
        {
            EditorGUILayout.HelpBox("Selecciona un objeto en la escena.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Agregar efecto de fuego al objeto seleccionado"))
        {
            CreateFireEffect();
        }
    }

    void CreateFireEffect()
    {
        GameObject target = Selection.activeGameObject;
        GameObject fireObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        fireObject.name = "FireEffect";
        fireObject.transform.SetParent(target.transform);
        fireObject.transform.localPosition = Vector3.up;
        fireObject.transform.localRotation = Quaternion.identity;
        fireObject.transform.localScale = new Vector3(planeWidth, planeHeight, 1);

        if (doubleSided)
        {
            fireObject.GetComponent<MeshRenderer>().material.doubleSidedGI = true;
        }

        string shaderPath = "Assets/Shaders/FireShaderTemplate.shadergraph";
        string newShaderPath = "Assets/Shaders/FireShaderInstance.shadergraph";
        string matPath = "Assets/Materials/FireMaterial.mat";

        if (File.Exists(shaderPath))
        {
            File.Copy(shaderPath, newShaderPath, true);
            AssetDatabase.ImportAsset(newShaderPath);
        }
        else
        {
            Debug.LogError("No se encontró FireShaderTemplate.shadergraph en Assets/Shaders");
            return;
        }

        Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(newShaderPath);
        Material mat = new Material(shader);

        if (fireTexture != null) mat.SetTexture("_BaseMap", fireTexture);
        if (noiseTexture != null) mat.SetTexture("_DistortionMap", noiseTexture);
        mat.SetColor("_EmissionColor", fireColor * fireIntensity);

        AssetDatabase.CreateAsset(mat, matPath);
        AssetDatabase.SaveAssets();

        fireObject.GetComponent<Renderer>().sharedMaterial = mat;

        if (addLight)
        {
            GameObject lightObj = new GameObject("FireLight");
            lightObj.transform.SetParent(fireObject.transform);
            lightObj.transform.localPosition = Vector3.zero;
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = lightColor;
            light.intensity = lightIntensity / 800f;
            light.range = lightRange;

#if UNITY_HDRP
            var hd = lightObj.AddComponent<HDAdditionalLightData>();
            hd.intensity = lightIntensity;
            hd.intensityUnit = LightUnit.Lumen;
#endif
        }

        Debug.Log("✨ Fuego generado en " + target.name);
    }
}
