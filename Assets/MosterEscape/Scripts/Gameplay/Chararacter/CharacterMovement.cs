using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera;
    private NavMeshAgent navAgent;
    private Animator playerAnimator;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        AnimatorVariablesControlles();
        
        if (Input.GetMouseButtonDown(0))
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Solo mover al personaje si se ha hecho clic en una superficie "Walkable"
            if (hit.collider.CompareTag("Walkable"))
            {
                navAgent.destination = hit.point;
            }
            Debug.Log(hit.collider.tag);
        }
    }

    void AnimatorVariablesControlles()
    {
        if (navAgent.velocity.sqrMagnitude == 0f)
        {
            playerAnimator.SetBool("IsRunning", false);
        }
        else
        {
            playerAnimator.SetBool("IsRunning", true);
        }
    }
}
