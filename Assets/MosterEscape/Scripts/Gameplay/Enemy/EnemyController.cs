using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float defense = 3f;
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private AudioClip attackSound; // Clip de sonido

    private AudioSource audioSource;
    private float hp;
    private bool isAttacking;
    private GameObject playerRef;

    // Evento para notificar cuando el enemigo muere
    public event Action OnDestroyed;

    private void Start()
    {
        hp = maxHp;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FovRoutine());
    }

    private void Update()
    {
        if (hp <= 0) return; // No hacer nada si el enemigo está muerto

        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);

        // Verificar si el jugador está dentro del radio de detección
        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackDistance)
            {
                StopMoving();
                FaceTarget(); // Mirar al jugador rápidamente
                Attack();
            }
            else
            {
                StopAttacking();
                MoveToPlayer();
            }
        }
        else
        {
            StopMoving(); // Detener al enemigo si el jugador está fuera del rango de detección
        }
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
        animator.SetBool("IsDeath", true);
        OnDestroyed?.Invoke(); // Invocar el evento para notificar que el enemigo ha muerto
        Destroy(gameObject, 2f); // Destruir el GameObject con un pequeño retraso para permitir la animación
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
            navMeshAgent.isStopped = true;
            PlayAttackSound();
        }
    }

    private void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            navMeshAgent.isStopped = false;
        }
    }

    private void MoveToPlayer()
    {
        if (playerRef != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(playerRef.transform.position);
            animator.SetBool("IsRunning", true); // Activar animación de correr
        }
    }

    private void StopMoving()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("IsRunning", false); // Desactivar animación de correr
    }

    private void FaceTarget()
    {
        Vector3 direction = (playerRef.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    private IEnumerator FovRoutine()
    {
        float delay = 0.25f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            // Aquí podrías hacer la lógica de FOV, si es necesario
        }
    }
}
