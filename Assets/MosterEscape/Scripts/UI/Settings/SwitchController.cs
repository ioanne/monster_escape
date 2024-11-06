using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class SwitchController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform toggle;
    [SerializeField] private TextMeshProUGUI textSwitch;
    [SerializeField] private Color onColor = new Color(250f / 255f, 176f / 255f, 55f / 255f);
    [SerializeField] private Color offColor = Color.gray;
    [SerializeField] private float toggleSpeed = 0.2f;

    [SerializeField] private Image toggleImage;
    [SerializeField] private Image switchBg;
    [SerializeField] private Sprite switchOnSprite;
    [SerializeField] private Sprite switchOffSprite;

    private bool isOn = true;
    private Vector2 onPosition = new Vector2(100, 0);
    private Vector2 offPosition = new Vector2(0, 0);
    private Vector2 textOnPosition = new Vector2(-31, 0);
    private Vector2 textOffPosition = new Vector2(31, 0);

    // Evento que notificará a otros scripts cuando el switch cambie de estado
    public event Action<bool> OnSwitchToggled;

    private void Start()
    {
        UpdateSwitchUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleSwitch();
    }

    private void ToggleSwitch()
    {
        isOn = !isOn;
        UpdateSwitchUI();
        StartCoroutine(AnimateToggle());

        // Llama al evento para notificar a AudioControllerUI sobre el cambio de estado
        OnSwitchToggled?.Invoke(isOn);
    }

    private void UpdateSwitchUI()
    {
        textSwitch.text = isOn ? "ON" : "OFF";
        textSwitch.color = isOn ? onColor : offColor;
        switchBg.color = isOn ? onColor : offColor;
        toggleImage.sprite = isOn ? switchOnSprite : switchOffSprite;
        textSwitch.rectTransform.anchoredPosition = isOn ? textOnPosition : textOffPosition;
    }

    private System.Collections.IEnumerator AnimateToggle()
    {
        Vector2 targetPosition = isOn ? onPosition : offPosition;
        float elapsedTime = 0;

        Vector2 startPosition = toggle.anchoredPosition;
        while (elapsedTime < toggleSpeed)
        {
            toggle.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / toggleSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toggle.anchoredPosition = targetPosition;
    }

    // Método para que AudioControllerUI establezca el estado visual inicial
    public void SetSwitchState(bool state)
    {
        isOn = state;
        UpdateSwitchUI();
    }
}
