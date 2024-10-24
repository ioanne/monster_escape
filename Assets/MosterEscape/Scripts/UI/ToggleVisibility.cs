using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleVisibility : MonoBehaviour
{
    [System.Serializable]
    public class TogglePair
    {
        public Button button;
        public GameObject objectToHide;
        public GameObject objectToShow;
    }

    public List<TogglePair> togglePairs = new List<TogglePair>();

    void Start()
    {
        foreach (var pair in togglePairs)
        {
            if (pair.button != null)
            {
                pair.button.onClick.AddListener(() => ToggleObjects(pair));
            }
            else
            {
                Debug.LogError("Button is not assigned.");
            }
        }
    }

    void ToggleObjects(TogglePair pair)
    {
        if (pair.objectToHide != null && pair.objectToShow != null)
        {
            pair.objectToHide.SetActive(false);
            pair.objectToShow.SetActive(true);
        }
        else
        {
            Debug.LogError("Objects to hide or show are not assigned.");
        }
    }
}
