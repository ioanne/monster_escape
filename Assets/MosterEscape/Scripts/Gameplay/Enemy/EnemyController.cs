using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Enemy enemyStats;
    private IAttack enemyAttack; // Usamos la interfaz IAttack
    private IMovement enemyMovement; // Usamos la interfaz IMovement
    private GameObject playerRef;
    private float distanceToPlayer;

    public event Action OnDestroyed;

    private void Start()
    {
        enemyStats = GetComponent<Enemy>();
        enemyAttack = GetComponent<IAttack>(); // Obtener el componente como IAttack
        enemyMovement = GetComponent<IMovement>(); // Obtener el componente como IMovement

        playerRef = GameObject.FindGameObjectWithTag("Player");

        if (playerRef != null)
        {
            enemyAttack.Initialize(playerRef);
            enemyMovement.Initialize(playerRef, navMeshAgent, animator);
        }

        StartCoroutine(FovRoutine());
    }

    private void Update()
    {
        if (enemyStats.IsDead() || playerRef == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackDistance)
            {
                enemyMovement.StopMoving();
                enemyMovement.FaceTarget();
                enemyAttack.AttemptAttack();
            }
            else
            {
                enemyAttack.StopAttacking();
                enemyMovement.MoveToPlayer();
            }
        }
        else
        {
            enemyMovement.StopMoving();
        }
    }

    private IEnumerator FovRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);
        while (true)
        {
            yield return wait;
            // Aquí podrías hacer la lógica de FOV, si es necesario
        }
    }
}
