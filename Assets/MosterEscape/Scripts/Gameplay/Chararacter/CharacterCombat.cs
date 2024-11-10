using UnityEngine;

public class CharacterCombat : MonoBehaviour
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
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer))
            {
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
                }
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Walkable"))
            {
                // Cancela el ataque y el enfoque en el enemigo
                CancelAttack();
                characterMovement.MoveTo(hit.point); // Mueve al personaje hacia el punto seleccionado
                Debug.Log("Attack canceled. Moving to walkable area");
            }
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
                Invoke(nameof(ResetAttack), 1.0f);
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
        selectedEnemy = null; // Desselecciona al enemigo
        enemySelected = false; // Reinicia la selección del enemigo
        isAttacking = false; // Reinicia el estado de ataque
        playerAnimator.ResetTrigger("AttackTrigger"); // Opcional: reinicia el trigger de ataque
        characterMovement.StopMovement(); // Detiene el movimiento
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}
