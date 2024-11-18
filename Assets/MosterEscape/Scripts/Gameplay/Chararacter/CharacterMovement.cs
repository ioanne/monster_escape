using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera mainCamera;
    private NavMeshAgent navAgent;
    private Animator playerAnimator;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = mainCamera ?? Camera.main;
    }

    void Update()
    {
        HandleMovementInput();
        UpdateAnimator();
    }

    public void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Walkable"))
            {
                // Verificar si EventSystem está disponible antes de usarlo
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return; // Si el puntero está sobre la UI, no ejecutar el movimiento
                }
                MoveTo(hit.point);
            }
        }
    }

    private void MoveTo(Vector3 destination)
    {
        navAgent.destination = destination;
        navAgent.isStopped = false;
        navAgent.stoppingDistance = 0f; // Restablece la distancia de parada
    }

    public void MoveToTarget(Transform target, float stoppingDistance)
    {
        navAgent.destination = target.position;
        navAgent.stoppingDistance = stoppingDistance;
        navAgent.isStopped = false;
    }

    public void StopMovement()
    {
        navAgent.isStopped = true;
    }

    public bool IsMoving()
    {
        // Devuelve true si el agente tiene un destino y se está moviendo
        return navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance;
    }

    private void UpdateAnimator()
    {
        bool isRunning = navAgent.velocity.sqrMagnitude > 0f;
        playerAnimator.SetBool("IsRunning", isRunning);
    }
}
