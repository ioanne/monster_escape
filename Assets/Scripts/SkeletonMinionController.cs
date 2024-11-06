using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private AudioClip attackSound; // Clip de sonido

    private AudioSource audioSource;
    private float hp;
    private int routine;
    private float cronometer;
    private Quaternion angleEnemy;
    private float degree;

    // Field of View Variables
    [SerializeField] public float radius;
    [SerializeField] public float angle;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    public GameObject playerRef;
    public bool canSeePlayer;

    
    // Evento para notificar cambios en la salud
    //public event Action<float, float, string> OnHealthChanged;
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
        if(canSeePlayer)    // Target al player
        {
            minionNav.destination = playerRef.transform.position;
        }
        else
        {
            minionNav.destination = transform.position;
        }

        if (hp <= 0)    // Muerte
        {
            Die();
        }

        if (Physics.CheckSphere(transform.position, attackDistance, targetMask))
        {
            Attack();
        }
        else
        {
            minionAnimator.SetBool("IsAttacking", false);
        }

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
        minionAnimator.SetBool("IsAttacking", true);
        minionNav.destination = transform.position;
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

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
