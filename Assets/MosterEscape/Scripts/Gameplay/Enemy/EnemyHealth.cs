using UnityEngine;

public class EnemyHealth : IHealth
{
    private float hp;
    private readonly float maxHp;
    private readonly Animator animator;

    public EnemyHealth(float maxHp, Animator animator)
    {
        this.maxHp = maxHp;
        this.animator = animator;
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"Enemy health: {hp}");
        if (hp <= 0) Die();
    }

    public void Die()
    {
        animator.SetBool("IsDeath", true);
        // Aquí podrías notificar a la UI que el enemigo ha muerto
        Debug.Log("Enemy Die");
        // Lógica adicional de muerte, como destruir el enemigo
    }
}
