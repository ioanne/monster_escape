using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : IAttack
{
    private readonly Animator animator;
    private readonly NavMeshAgent navMeshAgent;
    private bool isAttacking;

    public EnemyAttack(Animator animator, NavMeshAgent navMeshAgent)
    {
        this.animator = animator;
        this.navMeshAgent = navMeshAgent;
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
            navMeshAgent.isStopped = true;
        }
    }

    public void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            navMeshAgent.isStopped = false;
        }
    }
}
