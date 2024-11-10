using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : IMovement
{
    private readonly NavMeshAgent navMeshAgent;
    private readonly Animator animator;

    public EnemyMovement(NavMeshAgent navMeshAgent, Animator animator)
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
    }

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(destination);
        UpdateAnimation();
    }

    public void Stop()
    {
        navMeshAgent.isStopped = true;
        UpdateAnimation();
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - navMeshAgent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void UpdateAnimation()
    {
        if (navMeshAgent.velocity.sqrMagnitude == 0f)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        }
    }
}
