using UnityEngine;

#if UNITY_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

[ExecuteAlways]
public class TorchFlicker : MonoBehaviour
{
    [SerializeField]
    private float baseIntensity = 100f;
    [SerializeField]
    private float flickerAmount = 50f;

    [SerializeField]
    private float flickerSpeed = 20f;

    private Light torchLight;

#if UNITY_HDRP
    private HDAdditionalLightData hdLight;
#endif

    private float time;
    private int frameDelay = 2;
    void Start()
    {
        torchLight = GetComponent<Light>();

#if UNITY_HDRP
        hdLight = GetComponent<HDAdditionalLightData>();
#endif
    }

    void Update()
    {
        if (frameDelay > 0)
        {
            frameDelay--;
            return;
        }

        time += Time.deltaTime * flickerSpeed;
        float noise = Mathf.PerlinNoise(time, 0f);
        float variation = (noise - 0.5f) * flickerAmount;

#if UNITY_HDRP
        if (hdLight != null)
        {
            hdLight.intensity = Mathf.Max(0f, baseIntensity + variation);
        }
        else if (torchLight != null)
        {
            torchLight.intensity = Mathf.Max(0f, baseIntensity + variation);
        }
#else
        if (torchLight != null)
        {
            torchLight.intensity = Mathf.Max(0f, baseIntensity + variation);
        }
#endif
    }
}
