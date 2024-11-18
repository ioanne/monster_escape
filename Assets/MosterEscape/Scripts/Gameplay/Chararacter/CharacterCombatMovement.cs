using UnityEngine;

public class CharacterCombatMovement : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private GameObject selectedEnemy;
    [SerializeField] private AudioClip AttackSFX;

    private bool isAttacking = false;
    private bool enemySelected = false;

    private Animator playerAnimator;
    private CharacterMovement characterMovement;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        HandleCombatInput();
        HandleCancelTargetInput();

        if (enemySelected && !characterMovement.IsMoving())
        {
            RotateTowardsEnemy();
        }

        CheckDistanceToEnemy();
    }

    private void HandleCombatInput()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking) // Bloquea nuevos ataques si ya está atacando
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                ProcessRaycastHit(hit);
            }
        }
    }

    private void ProcessRaycastHit(RaycastHit hit)
    {
        if (IsEnemyHit(hit))
        {
            GameObject enemy = hit.collider.gameObject;
            HandleEnemySelection(enemy);
        }
        else if (hit.collider.CompareTag("Walkable"))
        {
            CancelAttack();
            characterMovement.HandleMovementInput();
        }
    }

    private bool IsEnemyHit(RaycastHit hit)
    {
        return ((1 << hit.collider.gameObject.layer) & enemyLayer) != 0;
    }

    private void HandleEnemySelection(GameObject enemy)
    {
        if (selectedEnemy == enemy)
        {
            if (enemySelected)
            {
                characterMovement.MoveToTarget(enemy.transform, stoppingDistance);
            }
        }
        else
        {
            ClearTarget(); // Unificar la cancelación previa
            SelectNewEnemy(enemy);
        }
    }

    private void SelectNewEnemy(GameObject enemy)
    {
        selectedEnemy = enemy;
        enemySelected = true;
        isAttacking = false;

        SubscribeToEnemyEvents();
        UIManager.Instance.ShowEnemyHealthBar(selectedEnemy.GetComponent<Enemy>());
    }

    private void SubscribeToEnemyEvents()
    {
        Enemy enemyComponent = selectedEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.OnEnemyDeath += ClearTarget;
        }
    }

    private void HandleCancelTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearTarget();
            Debug.Log("Attack and target canceled by pressing ESC");
        }
    }

    private void CheckDistanceToEnemy()
    {
        if (selectedEnemy != null && !isAttacking)
        {
            float distance = Vector3.Distance(transform.position, selectedEnemy.transform.position);
            if (distance <= stoppingDistance)
            {
                AttackEnemy();
            }
        }
    }

    private void AttackEnemy()
    {
        characterMovement.StopMovement();
        isAttacking = true;
        playerAnimator.SetTrigger("AttackTrigger");
        AudioManager.Instance.Playsound(AttackSFX);

        Enemy enemy = selectedEnemy.GetComponent<Enemy>();
        enemy?.TakeDamage(attackDamage);

        StartCoroutine(ResetAttackAfterDelay(1.0f)); // Restablece isAttacking después del retraso
    }

    private void RotateTowardsEnemy()
    {
        if (selectedEnemy == null) return;

        Vector3 direction = (selectedEnemy.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 360f);
    }

    private void CancelAttack()
    {
        isAttacking = false;
        playerAnimator.ResetTrigger("AttackTrigger");
        characterMovement.StopMovement();
    }

    private void ClearTarget()
    {
        if (selectedEnemy != null)
        {
            Enemy enemyComponent = selectedEnemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.OnEnemyDeath -= ClearTarget;
            }
            UIManager.Instance.HideEnemyHealthBar(selectedEnemy.GetComponent<Enemy>());
        }

        CancelAttack();
        selectedEnemy = null;
        enemySelected = false;
    }

    private System.Collections.IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
}
