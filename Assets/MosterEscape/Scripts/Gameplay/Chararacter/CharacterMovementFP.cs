using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovementFP : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool isFirstPerson = true;

    [Header("Referencias")]
    [SerializeField] private Camera mainCamera;

    private CharacterController characterController;
    private Animator playerAnimator;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = mainCamera ?? Camera.main;
    }

    private void Update()
    {
        if (isFirstPerson)
        {
            HandleFirstPersonMovementWithNavMesh();
        }
        else
        {
            HandleClickToMove();
        }

        UpdateAnimator();
    }

    private void HandleFirstPersonMovementWithNavMesh()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Ahora el transform del PlayerFirst ya rota con el mouse
        Vector3 moveInput = transform.right * x + transform.forward * z;
        Vector3 desiredMove = moveInput.normalized * moveSpeed * Time.deltaTime;
        Vector3 targetPosition = transform.position + desiredMove;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
        {
            Vector3 correctedMove = hit.position - transform.position;
            if (correctedMove.magnitude > 0.01f)
            {
                characterController.Move(correctedMove);
            }
        }
    }

    private void HandleClickToMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Walkable"))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                // Movimiento por NavMesh (modo 3ra persona opcional)
            }
        }
    }

    private void UpdateAnimator()
    {
        if (playerAnimator == null) return;

        bool isRunning = false;

        if (isFirstPerson)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(x, 0, z);
            isRunning = move.magnitude > 0.1f;
        }

        playerAnimator.SetBool("IsRunning", isRunning);
    }
}
