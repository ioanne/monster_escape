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
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Clicked on UI");
                    return;
                }
                Debug.Log("Clicked on anothers");
                MoveTo(hit.point);

            }
        }
    }

    private void MoveTo(Vector3 destination)
    {
        navAgent.destination = destination;
        navAgent.isStopped = false;
        navAgent.stoppingDistance = 0f; // Restablece la distancia de parada
        Debug.Log("Moving to: " + destination);
    }

    public void MoveToTarget(Transform target, float stoppingDistance)
    {
        navAgent.destination = target.position;
        navAgent.stoppingDistance = stoppingDistance;
        navAgent.isStopped = false;
        Debug.Log("Moving to target: " + target.name);
    }

    public void StopMovement()
    {
        navAgent.isStopped = true;
        Debug.Log("Movement stopped");
    }

    private void UpdateAnimator()
    {
        bool isRunning = navAgent.velocity.sqrMagnitude > 0f;
        playerAnimator.SetBool("IsRunning", isRunning);
    }
}
