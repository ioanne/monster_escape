using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

public class TorchLightTool : EditorWindow
{
    private Color lightColor = new Color(1f, 0.5f, 0.2f);
    private float intensity = 25f;
    private float range = 10f;
    private float flickerAmount = 0.7f;
    private float flickerSpeed = 15f;
    private Vector3 lightOffset = new Vector3(0.143f, 1.071f, -0.081f);
    private LightType lightType = LightType.Point;

#if UNITY_HDRP
    private LightUnit intensityUnit = LightUnit.Lumen;
#endif

    private GameObject firePrefab;
    private Material fireMaterial;
    private bool applyToAllMatching = false;

    [MenuItem("Tools/Agregar Luz tipo Antorcha")]
    public static void ShowWindow()
    {
        GetWindow<TorchLightTool>("Luz Antorcha");
    }

    void OnGUI()
    {
        GUILayout.Label("Parámetros de Luz", EditorStyles.boldLabel);

        lightType = (LightType)EditorGUILayout.EnumPopup("Tipo de luz", lightType);
        lightColor = EditorGUILayout.ColorField("Color", lightColor);
        intensity = EditorGUILayout.Slider("Intensidad", intensity, 0f, 40000f);

#if UNITY_HDRP
        intensityUnit = (LightUnit)EditorGUILayout.EnumPopup("Unidad", intensityUnit);
#else
        EditorGUILayout.HelpBox("Unidades físicas disponibles solo en HDRP. Usando intensidad relativa.", MessageType.Info);
#endif

        range = EditorGUILayout.Slider("Rango", range, 0.5f, 20f);
        lightOffset = EditorGUILayout.Vector3Field("Offset de la luz (X, Y, Z)", lightOffset);

        GUILayout.Space(5);
        GUILayout.Label("Efecto de parpadeo (flicker)", EditorStyles.boldLabel);
        flickerAmount = EditorGUILayout.Slider("Amplitud", flickerAmount, 0f, 2000f);
        flickerSpeed = EditorGUILayout.Slider("Velocidad", flickerSpeed, 0f, 30f);

        GUILayout.Space(10);
        GUILayout.Label("Fuego visual (opcional)", EditorStyles.boldLabel);
        firePrefab = (GameObject)EditorGUILayout.ObjectField("Prefab del fuego", firePrefab, typeof(GameObject), false);
        fireMaterial = (Material)EditorGUILayout.ObjectField("Material HDRP opcional", fireMaterial, typeof(Material), false);

        GUILayout.Space(10);
        GUILayout.Label("Aplicación masiva", EditorStyles.boldLabel);

        if (Selection.activeGameObject != null)
        {
            applyToAllMatching = EditorGUILayout.Toggle("Aplicar a todos los objetos con el mismo mesh", applyToAllMatching);

            if (GUILayout.Button($"Agregar luz {(applyToAllMatching ? "a todos los iguales a" : "solo a")} '{Selection.activeGameObject.name}'"))
            {
                var selectedMesh = Selection.activeGameObject.GetComponent<MeshFilter>()?.sharedMesh;

                if (selectedMesh != null && applyToAllMatching)
                {
                    int count = 0;
                    foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
                    {
                        var mf = obj.GetComponent<MeshFilter>();
                        if (mf != null && mf.sharedMesh == selectedMesh)
                        {
                            AddTorchLightToObject(obj);
                            count++;
                        }
                    }
                    Debug.Log($"🔥 Se agregaron luces a {count} objetos con el mismo mesh que '{Selection.activeGameObject.name}'.");
                }
                else
                {
                    AddTorchLightToObject(Selection.activeGameObject);
                    Debug.Log($"🔥 Se agregó luz a '{Selection.activeGameObject.name}'.");
                }
            }

            if (GUILayout.Button($"Eliminar luces {(applyToAllMatching ? "de todos los iguales a" : "solo de")} '{Selection.activeGameObject.name}'"))
            {
                var selectedMesh = Selection.activeGameObject.GetComponent<MeshFilter>()?.sharedMesh;

                if (selectedMesh != null && applyToAllMatching)
                {
                    int count = 0;
                    foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
                    {
                        var mf = obj.GetComponent<MeshFilter>();
                        if (mf != null && mf.sharedMesh == selectedMesh)
                        {
                            if (RemoveTorchLights(obj)) count++;
                        }
                    }
                    Debug.Log($"🧹 Se eliminaron luces y fuego de {count} objetos con el mismo mesh.");
                }
                else
                {
                    RemoveTorchLights(Selection.activeGameObject);
                    Debug.Log($"🧹 Se eliminaron luces y fuego de '{Selection.activeGameObject.name}'.");
                }
            }
        }
        else
        {
            GUILayout.Label("Seleccioná un objeto para agregar o quitar la luz.");
        }
    }

    void AddTorchLightToObject(GameObject obj)
    {
        GameObject lightObj = new GameObject("TorchLight");
        lightObj.transform.parent = obj.transform;
        lightObj.transform.localPosition = lightOffset;

        Light light = lightObj.AddComponent<Light>();
        
        Vector3 pos = lightObj.transform.localPosition;
        pos.y = 0.5f;
        lightObj.transform.localPosition = pos;


        light.type = lightType;
        light.color = lightColor;
        light.range = range;
        light.shadows = LightShadows.Soft;
        light.lightmapBakeType = LightmapBakeType.Realtime;
        lightObj.isStatic = false;

#if UNITY_HDRP
        var hdLight = lightObj.GetComponent<HDAdditionalLightData>() ?? lightObj.AddComponent<HDAdditionalLightData>();
        hdLight.intensityUnit = intensityUnit;
        hdLight.intensity = intensity;
        hdLight.SetUseCustomFade(true);
        hdLight.fadeDistance = 100f;
        hdLight.affectDiffuse = true;
        hdLight.affectSpecular = true;
        hdLight.volumetricDimmer = 1f;
#else
        light.intensity = intensity / 800f;
#endif

        var flicker = lightObj.AddComponent<TorchFlicker>();
#if UNITY_HDRP
        flicker.baseIntensity = intensity;
        flicker.flickerAmount = flickerAmount;
#else
        flicker.baseIntensity = light.intensity;
        flicker.flickerAmount = flickerAmount / 800f;
#endif
        flicker.flickerSpeed = flickerSpeed;

        Undo.RegisterCreatedObjectUndo(lightObj, "Agregar Luz tipo Antorcha");

        if (firePrefab != null)
        {
            GameObject fireInstance = (GameObject)PrefabUtility.InstantiatePrefab(firePrefab);
            fireInstance.name = "TorchFire"; // 🔥 NOMBRE FIJO
            fireInstance.transform.SetParent(obj.transform, false);
            fireInstance.transform.localPosition = Vector3.zero;

            if (fireMaterial != null)
            {
                var renderer = fireInstance.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = fireMaterial;
                }
            }

            Undo.RegisterCreatedObjectUndo(fireInstance, "Agregar fuego visual");
        }
    }

    bool RemoveTorchLights(GameObject obj)
    {
        int removed = 0;

        foreach (Transform child in obj.transform)
        {
            if (child.name == "TorchLight" || child.name == "TorchFire" || child.name == "CartoonFireTwo")
            {
                Undo.DestroyObjectImmediate(child.gameObject);
                removed++;
            }
        }

        return removed > 0;
    }


    [ExecuteInEditMode]
    public class TorchFlicker : MonoBehaviour
    {
        public float baseIntensity = 800f;
        public float flickerAmount = 50f;
        public float flickerSpeed = 15f;

#if UNITY_HDRP
        private HDAdditionalLightData hdLight;
#else
        private Light torchLight;
#endif

        private float time;

        void Start()
        {
#if UNITY_HDRP
            hdLight = GetComponent<HDAdditionalLightData>();
#else
            torchLight = GetComponent<Light>();
#endif
        }

        void Update()
        {
            time += Time.deltaTime * flickerSpeed;
            float noise = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f;
            float variation = noise * flickerAmount;
            float finalIntensity = Mathf.Max(0f, baseIntensity + variation);

#if UNITY_HDRP
            if (hdLight != null)
                hdLight.intensity = finalIntensity;
#else
            if (torchLight != null)
                torchLight.intensity = finalIntensity;
#endif
        }
    }
}
