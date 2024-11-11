using UnityEngine;
using UnityEngine.AI;

public interface IMovement
{
    void Initialize(GameObject player, NavMeshAgent agent, Animator anim);
    void MoveToPlayer();
    void StopMoving();
    void FaceTarget();
}
