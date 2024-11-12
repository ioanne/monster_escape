using UnityEngine;

public class CharacterCombatMovement : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float stoppingDistance = 2f;
    private Animator playerAnimator;
    private GameObject selectedEnemy;
    private bool isAttacking = false;
    private bool enemySelected = false;

    private CharacterMovement characterMovement; // Referencia a CharacterMovement

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>(); // Obtén el componente CharacterMovement
    }

    void Update()
    {
        HandleCombatInput();
        HandleCancelTargetInput();

        if (enemySelected) // Siempre rota hacia el enemigo mientras esté seleccionado
        {
            RotateTowardsEnemy();
        }

        CheckDistanceToEnemy();
    }

    private void HandleCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
                {
                    // Si el raycast golpea un enemigo
                    GameObject enemy = hit.collider.gameObject;
                    if (selectedEnemy == enemy)
                    {
                        if (enemySelected)
                        {
                            // Si el enemigo ya está seleccionado, moverse hacia él y atacar
                            characterMovement.MoveToTarget(enemy.transform, stoppingDistance);
                        }
                        else
                        {
                            enemySelected = true;
                            Debug.Log("Enemy Selected: " + enemy.name);
                        }
                    }
                    else
                    {
                        selectedEnemy = enemy;
                        enemySelected = true;
                        isAttacking = false;
                        Debug.Log("New Enemy Selected: " + enemy.name);

                        // Mostrar la barra de vida del enemigo
                        UIManager.Instance.ShowEnemyHealthBar();
                    }
                }
                else if (hit.collider.CompareTag("Walkable"))
                {
                    // Si el raycast golpea un área "walkable", cancela el ataque y mueve al personaje
                    CancelAttack();
                    characterMovement.HandleMovementInput();
                    Debug.Log("Attack canceled. Moving to walkable area");
                }
            }
        }
    }

    private void HandleCancelTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Cancela el ataque y el objetivo seleccionado de inmediato
            CancelTargetAndAttack();
            Debug.Log("Attack and target canceled by pressing ESC");

            // Ocultar la barra de vida del enemigo
            UIManager.Instance.HideEnemyHealthBar();
        }
    }

    private void CheckDistanceToEnemy()
    {
        if (selectedEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, selectedEnemy.transform.position);
            if (distance <= stoppingDistance && !isAttacking)
            {
                characterMovement.StopMovement(); // Detener el movimiento antes de atacar
                isAttacking = true;
                playerAnimator.SetTrigger("AttackTrigger");
                Debug.Log("Attacking: " + selectedEnemy.name);
                // No usar Invoke para asegurarse de que el ataque pueda ser cancelado de inmediato
                StartCoroutine(ResetAttackAfterDelay(1.0f));
            }
        }
    }

    private void RotateTowardsEnemy()
    {
        if (selectedEnemy != null)
        {
            Vector3 direction = (selectedEnemy.transform.position - transform.position).normalized;
            direction.y = 0; // Asegura que la rotación solo ocurra en el eje Y
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 360f);
        }
    }

    private void CancelAttack()
    {
        isAttacking = false; // Reinicia el estado de ataque
        playerAnimator.ResetTrigger("AttackTrigger"); // Reinicia inmediatamente el trigger de ataque
        characterMovement.StopMovement(); // Detiene el movimiento
    }

    private void CancelTargetAndAttack()
    {
        CancelAttack(); // Cancela el ataque
        selectedEnemy = null; // Desselecciona al enemigo
        enemySelected = false; // Reinicia la selección del enemigo
    }

    private System.Collections.IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
}
