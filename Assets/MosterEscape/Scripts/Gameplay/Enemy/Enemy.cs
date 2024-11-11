using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float defense = 3f;

    [Header("Attack Stats")]
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackSpeed = 2f;

    private float hp;

    private void Awake()
    {
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        hp -= Mathf.Max(damage - defense, 0);
        Debug.Log("Enemy Takes Damage: " + damage);
        if (hp <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return hp <= 0;
    }

    private void Die()
    {
        Debug.Log("Enemy Die");
        GetComponent<Animator>().SetBool("IsDeath", true);
        Destroy(gameObject, 2f);
    }

    public float GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackCooldown()
    {
        return attackCooldown;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}
