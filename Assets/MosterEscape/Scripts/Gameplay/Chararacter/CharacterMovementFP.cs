using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovementFP : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool isFirstPerson = false; // <<< Nuevo
    [SerializeField] private Camera mainCamera;
    private NavMeshAgent navAgent;
    private Animator playerAnimator;
    private CharacterController characterController;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>(); // para movimiento FPS
        playerAnimator = GetComponent<Animator>();
        mainCamera = mainCamera ?? Camera.main;
    }

    private void Update()
    {
        if (isFirstPerson)
        {
            HandleFirstPersonMovement();
        }
        else
        {
            HandleMovementInput();
        }

        UpdateAnimator();
    }

    private void HandleFirstPersonMovement()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        // Movimiento hacia adelante y atrás
        Vector3 move = transform.forward * z;
        if (move.magnitude >= 0.1f)
        {
            NavMeshHit hit;
            Vector3 targetPosition = transform.position + move * moveSpeed * Time.deltaTime;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                navAgent.Move(move * moveSpeed * Time.deltaTime);
            }
        }

        // Rotación con teclado
        float rotationSpeed = 120f; // Velocidad de giro en grados por segundo
        if (Mathf.Abs(x) > 0.1f)
        {
            transform.Rotate(Vector3.up, x * rotationSpeed * Time.deltaTime);
        }
    }

    public void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Walkable"))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                MoveTo(hit.point);
            }
        }
    }

    private void MoveTo(Vector3 destination)
    {
        navAgent.destination = destination;
        navAgent.isStopped = false;
        navAgent.stoppingDistance = 0f;
    }

    public void MoveToTarget(Transform target, float stoppingDistance)
    {
        navAgent.destination = target.position;
        navAgent.stoppingDistance = stoppingDistance;
        navAgent.isStopped = false;
    }

    public void StopMovement()
    {
        if (isFirstPerson)
        {
            // No hay navmesh en primera persona
            return;
        }
        navAgent.isStopped = true;
    }

    public bool IsMoving()
    {
        if (isFirstPerson)
            return characterController.velocity.magnitude > 0.1f;

        return navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance;
    }

    private void UpdateAnimator()
    {
        bool isRunning = false;
        if (isFirstPerson)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(x, 0, z);
            isRunning = move.magnitude > 0.1f; // Si hay input, está corriendo
        }
        else
        {
            isRunning = navAgent.velocity.sqrMagnitude > 0f;
        }
        playerAnimator.SetBool("IsRunning", isRunning);
    }
}
