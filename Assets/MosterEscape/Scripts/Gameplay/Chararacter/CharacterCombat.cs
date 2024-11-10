using UnityEngine;
using UnityEngine.AI;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float stoppingDistance = 2f; // Distancia mínima para detenerse frente al enemigo
    private Animator playerAnimator;
    private MeshCollider swordCollider;
    private NavMeshAgent navAgent; // Referencia al NavMeshAgent
    private GameObject selectedEnemy; // Variable para almacenar el enemigo seleccionado
    private bool isAttacking = false; // Bandera para controlar el estado de ataque
    private bool enemySelected = false; // Bandera para verificar si un enemigo está seleccionado

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        swordCollider = GetComponentInChildren<MeshCollider>();
        navAgent = GetComponent<NavMeshAgent>(); // Obtén el componente NavMeshAgent
        navAgent.stoppingDistance = stoppingDistance; // Establece la distancia de parada
    }

    void Update()
    {
        HandleInput();
        CheckDistanceToEnemy();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta un clic del mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Verifica si el raycast golpea un enemigo
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
            {
                GameObject enemy = hit.collider.gameObject;

                if (selectedEnemy == enemy)
                {
                    if (enemySelected)
                    {
                        // Si el enemigo ya está seleccionado, moverse hacia él y atacar
                        MoveToEnemy();
                    }
                    else
                    {
                        // Selecciona el enemigo
                        enemySelected = true;
                        Debug.Log("Enemy Selected: " + enemy.name);
                    }
                }
                else
                {
                    // Selecciona un nuevo enemigo
                    selectedEnemy = enemy;
                    enemySelected = true;
                    isAttacking = false; // Reinicia el estado de ataque
                    Debug.Log("Enemy Selected: " + enemy.name);
                }
            }
            // Verifica si el raycast golpea un área con el Tag "Walkable"
            else if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Walkable"))
            {
                // Cancela el ataque y mueve al personaje hacia la posición seleccionada
                CancelAttack();
                navAgent.destination = hit.point; // Mover al personaje hacia el punto seleccionado
                navAgent.isStopped = false; // Permitir el movimiento
                navAgent.stoppingDistance = 0f; // Restablece la distancia de parada
                Debug.Log("Attack canceled. Moving to walkable area");
            }
        }
    }

    void MoveToEnemy()
    {
        if (selectedEnemy != null)
        {
            navAgent.isStopped = false; // Permitir que el NavMeshAgent se mueva
            navAgent.stoppingDistance = stoppingDistance; // Establece la distancia de parada
            navAgent.destination = selectedEnemy.transform.position; // Moverse hacia el enemigo
            Debug.Log("Moving to: " + selectedEnemy.name);
        }
    }

    void CancelAttack()
    {
        selectedEnemy = null; // Desselecciona al enemigo
        enemySelected = false; // Reinicia la selección del enemigo
        isAttacking = false; // Reinicia el estado de ataque
        navAgent.isStopped = false; // Permitir que el NavMeshAgent se mueva
        playerAnimator.ResetTrigger("AttackTrigger"); // Opcional: reinicia el trigger de ataque si es necesario
    }

    void CheckDistanceToEnemy()
    {
        if (selectedEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, selectedEnemy.transform.position);

            // Rotar continuamente hacia el enemigo
            RotateTowardsEnemy();

            if (distanceToEnemy <= stoppingDistance && !isAttacking)
            {
                // Detener el movimiento y atacar si está lo suficientemente cerca
                navAgent.isStopped = true;
                isAttacking = true; // Cambia el estado a atacando
                playerAnimator.SetTrigger("AttackTrigger");
                Debug.Log("Attacking: " + selectedEnemy.name);

                // Reiniciar el estado de ataque después de un tiempo (opcional)
                Invoke(nameof(ResetAttack), 1.0f); // 1 segundo después del ataque, reinicia el estado
            }
        }
    }

    void RotateTowardsEnemy()
    {
        if (selectedEnemy != null)
        {
            Vector3 direction = (selectedEnemy.transform.position - transform.position).normalized;
            direction.y = 0; // Asegura que la rotación solo ocurra en el eje y
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 360f);
        }
    }

    void ResetAttack()
    {
        isAttacking = false; // Permite nuevos ataques
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit to Enemy: " + other.name);
        }
    }
}
