using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColectableSFX : MonoBehaviour
{

    [SerializeField] private AudioClip collect;

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.Playsound(collect);
        Destroy(gameObject);
    }
}
