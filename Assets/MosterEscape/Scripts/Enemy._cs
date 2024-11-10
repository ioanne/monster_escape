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

    // Evento para notificar cuando el enemigo muere
    public event Action OnDestroyed;

    private bool isAttacking;
    private bool playerAttack = false;

    private void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        // Lógica de actualización (puedes agregar tu propia lógica aquí)
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("Enemy Takes Damage: " + damage);

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Die");
        // Lógica para manejar la muerte del enemigo (animaciones, efectos, etc.)
        OnDestroyed?.Invoke(); // Invocar el evento para notificar que el enemigo ha muerto
        Destroy(gameObject); // Destruir el GameObject
    }
}
