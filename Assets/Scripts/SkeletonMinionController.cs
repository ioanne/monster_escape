using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonMinionController : MonoBehaviour
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float defense = 3f;
    [SerializeField] private float attack = 10f;
    [SerializeField] private Animator minionAnimator;
    [SerializeField] private NavMeshAgent minionNav;
    [SerializeField] private float targetDistance = 5f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private AudioClip attackSound;

    private AudioSource audioSource;
    private float hp;
    private int routine;
    private float cronometer;
    private Quaternion angleEnemy;
    private float degree;

    [SerializeField] public float radius;
    [SerializeField] public float angle;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    public GameObject playerRef;
    public bool canSeePlayer;
    private bool hasSeenPlayer;

    private bool isAttacking;
    private bool playerAttack = false;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FovRoutine());
        hp = maxHp;
    }

    private void Update()
    {
        if (hp <= 0) // Muerte
        {
            Die();
            return; // Salir de Update si el enemigo está muerto
        }

        if (isAttacking)
        {
            minionNav.isStopped = true;
        }
        else
        {
            minionNav.isStopped = false; // Permitir moverse nuevamente

            if (canSeePlayer || hasSeenPlayer) // Si puede ver al jugador o lo ha visto antes
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);
                if (distanceToPlayer > radius) // Si el jugador se aleja del rango
                {
                    hasSeenPlayer = false; // Dejar de seguir al jugador
                }
                else
                {
                    minionNav.destination = playerRef.transform.position;
                    FacePlayer(); // Gira hacia el jugador
                }
            }
            else
            {
                minionNav.destination = transform.position; // Quedarse en su posición
            }
        }

        // Comprobar si está dentro de la distancia de ataque
        if (Physics.CheckSphere(transform.position, attackDistance, targetMask))
        {
            Attack();
        }
        else
        {
            StopAttacking();
        }

        // Actualizar la animación de correr
        if (minionNav.velocity.sqrMagnitude == 0f)
        {
            minionAnimator.SetBool("IsRunning", false);
        }
        else
        {
            minionAnimator.SetBool("IsRunning", true);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"Enemy health: {hp}");
    }

    private void Die()
    {
        minionAnimator.SetBool("IsDeath", true);
        Destroy(gameObject);
        Debug.Log("Enemy Die");
    }

    private IEnumerator FovRoutine()
    {
        float delay = 0.25f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FovCheck();
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            minionAnimator.SetBool("IsAttacking", true);
            minionNav.isStopped = true; // Detener el NavMeshAgent mientras ataca
        }
    }

    private void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            minionAnimator.SetBool("IsAttacking", false);
            minionNav.isStopped = false; // Permitir que el NavMeshAgent se mueva nuevamente
        }
    }

    private void FovCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    hasSeenPlayer = true; // Recordar que hemos visto al jugador
                    return;
                }
            }
        }

        canSeePlayer = false; // Si no se cumple ninguna condición, no puede ver al jugador
    }

    private void FacePlayer()
    {
        // Gira hacia el jugador
        Vector3 direction = (playerRef.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
