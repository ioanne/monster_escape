using UnityEngine;
using System.Collections;

public class CharacterCombatMovement : MonoBehaviour
{
    [Header("Combate")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackAngle = 60f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private Transform attackOrigin;

    private Animator animator;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttacking = false;
    private Enemy currentEnemy = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (attackOrigin == null)
            attackOrigin = Camera.main.transform;
    }

    private void Update()
    {
        if (MenuManager.Instance != null && MenuManager.Instance.IsAnyMenuOpen)
        {
            Debug.Log("⛔ Ataque bloqueado: hay un menú abierto.");
            return;
        }

        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            if (Time.time - lastAttackTime < attackCooldown)
            {
                Debug.Log("Esperando cooldown de ataque.");
                return;
            }

            lastAttackTime = Time.time;
            isAttacking = true;

            Debug.Log("Ataque iniciado.");
            animator.SetTrigger("AttackTrigger");
            AudioManager.Instance?.Playsound(attackSFX);

            Debug.Log($"Detectando enemigos con OverlapSphere en {attackOrigin.position}, radio {attackRange}...");
            Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRange, enemyLayer);
            Debug.Log($"Detectados {hits.Length} posibles objetivos en rango.");

            foreach (Collider col in hits)
            {
                Debug.Log($"Revisando objetivo: {col.name}");

                Vector3 toTarget = col.transform.position - transform.position;
                toTarget.y = 0f;

                Vector3 forward = transform.forward;
                forward.y = 0f;

                float angle = Vector3.Angle(forward, toTarget.normalized);
                Debug.Log($"Ángulo con {col.name}: {angle} grados (máximo permitido: {attackAngle / 2f})");

                if (angle <= attackAngle / 2f)
                {
                    Enemy enemyComponent = col.GetComponentInParent<Enemy>();
                    if (enemyComponent != null)
                    {
                        // Rotar al enemigo justo antes de golpear
                        Vector3 direction = (enemyComponent.transform.position - transform.position).normalized;
                        direction.y = 0;
                        Quaternion lookRotation = Quaternion.LookRotation(direction);
                        transform.rotation = lookRotation;

                        Debug.Log($"Enemigo válido detectado: {enemyComponent.name}, aplicando daño: {attackDamage}");
                        enemyComponent.TakeDamage(attackDamage);

                        // Desuscribir anterior si cambia
                        if (currentEnemy != null && currentEnemy != enemyComponent)
                        {
                            currentEnemy.OnEnemyDeath -= ClearCurrentEnemy;
                        }

                        currentEnemy = enemyComponent;
                        currentEnemy.OnEnemyDeath += ClearCurrentEnemy;

                        UIManager.Instance?.ShowEnemyHealthBar(currentEnemy);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning($"El objeto {col.name} no tiene componente Enemy en el padre.");
                    }
                }
                else
                {
                    Debug.Log($"{col.name} está fuera del ángulo permitido.");
                }
            }

            StartCoroutine(ResetAttackAfterDelay(attackCooldown));
        }
    }

    private void ClearCurrentEnemy()
    {
        if (currentEnemy != null)
        {
            Debug.Log($"Desmarcando enemigo: {currentEnemy.name}");
            currentEnemy.OnEnemyDeath -= ClearCurrentEnemy;
            UIManager.Instance?.HideEnemyHealthBar(currentEnemy);
            currentEnemy = null;
        }
    }

    private IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackOrigin != null)
        {
            Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
        }
    }
}
