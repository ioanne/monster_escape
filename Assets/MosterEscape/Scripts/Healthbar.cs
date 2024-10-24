using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public void UpdateHelathbar(float maxHealth, float healt)
    {
        barImage.fillAmount = healt / maxHealth;
    }
}
