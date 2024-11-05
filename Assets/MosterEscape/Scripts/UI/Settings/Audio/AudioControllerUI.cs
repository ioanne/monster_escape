using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si estás usando TextMeshPro en lugar de Text

public class AudioControllerUI : MonoBehaviour
{
    [Header("Audio UI Elements")]
    public Slider volumeSlider;          // Slider de volumen
    public SwitchController muteSwitch;  // Switch visual para silenciar
    public TextMeshProUGUI volumeText;   // Texto para mostrar el porcentaje de volumen (usa Text si no estás usando TextMeshPro)

    private void Start()
    {
        // Configura el rango y valor inicial del slider
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = 0.5f; // Iniciar en 50% de volumen

        // Establece el valor inicial del volumen en AudioController y el texto de porcentaje
        AudioController.Instance.SetVolume(volumeSlider.value * 100); // Convierte a porcentaje
        UpdateVolumeText(volumeSlider.value);

        // Configura el estado inicial del switch según el estado de silencio en AudioController
        muteSwitch.SetSwitchState(!AudioController.Instance.IsMuted());

        // Agrega listeners para los cambios en el slider y el switch
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChange(); });
        muteSwitch.OnSwitchToggled += OnMuteSwitchToggled;
    }

    private void OnVolumeSliderChange()
    {
        // Ajusta el volumen en el AudioController
        AudioController.Instance.SetVolume(volumeSlider.value * 100); // Convierte a porcentaje

        // Actualiza el texto del porcentaje
        UpdateVolumeText(volumeSlider.value);
    }

    private void OnMuteSwitchToggled(bool isOn)
    {
        // Cambia el estado de silencio en AudioController según el estado del switch
        if (isOn && AudioController.Instance.IsMuted())
        {
            AudioController.Instance.ToggleMute();  // Desilenciar si está en ON
        }
        else if (!isOn && !AudioController.Instance.IsMuted())
        {
            AudioController.Instance.ToggleMute();  // Silenciar si está en OFF
        }
    }

    private void UpdateVolumeText(float sliderValue)
    {
        int volumePercentage = Mathf.RoundToInt(sliderValue * 100); // Convierte el valor del slider a porcentaje
        volumeText.text = volumePercentage + "%"; // Actualiza el texto con el porcentaje
    }
}
