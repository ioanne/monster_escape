using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IMovement
{
    private GameObject playerRef;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public void Initialize(GameObject player, NavMeshAgent agent, Animator anim)
    {
        playerRef = player;
        navMeshAgent = agent;
        animator = anim;
    }

    public void MoveToPlayer()
    {
        if (playerRef != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(playerRef.transform.position);
            animator.SetBool("IsRunning", true);
        }
    }

    public void StopMoving()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("IsRunning", false);
    }

    public void FaceTarget()
    {
        Vector3 direction = (playerRef.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
}
