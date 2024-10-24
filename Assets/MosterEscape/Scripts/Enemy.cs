using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float defense = 3f;
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private Animator anim;
    [SerializeField] private float speedWalk;
    [SerializeField] private float speedRun;
    [SerializeField] private GameObject target;
    [SerializeField] private float targetDistance = 5f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private AudioClip attackSound; // Clip de sonido

    private AudioSource audioSource;
    private float hp;
    private int routine;
    private float cronometer;
    private Quaternion angleEnemy;
    private float degree;

    // Evento para notificar cambios en la salud
    //public event Action<float, float, string> OnHealthChanged;

    private bool isAttacking;
    private bool playerAttack = false;

    private void Start()
    {
     
    }

    private void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy Takes Damage");
    }

    private void Die()
    {
        Debug.Log("Enemy Die");
    }
}
