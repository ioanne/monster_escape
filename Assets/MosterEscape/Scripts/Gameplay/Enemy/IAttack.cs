using UnityEngine;

public interface IAttack
{
    void Initialize(GameObject player);
    void AttemptAttack();
    void StopAttacking();
}