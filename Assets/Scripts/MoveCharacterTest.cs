using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    private CharacterController controller;
    public Camera mainCamera; // La camara principal
    public NavMeshAgent navAgent;
    public Animator playerAnimator;
    public LayerMask enemyLayer;
    public MeshCollider swordCollider;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        swordCollider = GetComponentInChildren<MeshCollider>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Asigna la camara principal si no se ha asignado
        }
    }

    void Update()
    {
        AnimatorVariablesControlles();
        
        if (Input.GetMouseButtonDown(0))
        {
            Movimiento();
        }

        if (Physics.CheckSphere(transform.position, 3f, enemyLayer))
        {
            Attack();
        }
        else
        {
            //playerAnimator.SetBool("IsAttacking", false);
        }
    }

    void Movimiento(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            if (hit.collider.tag == "Walkable")
            {
                navAgent.destination = hit.point;
            }
            if (hit.collider.tag == "Enemy")
            {
                navAgent.destination = hit.point;
            }
            Debug.Log(hit.collider.tag);
        }
    }

    void AnimatorVariablesControlles()
    {
        // Idle y Caminar
        if (navAgent.velocity.sqrMagnitude == 0f)
        {
            playerAnimator.SetBool("IsRunning", false);
        }
        else
        {
            playerAnimator.SetBool("IsRunning", true);
        }
        // Pegar - FALTA
        // Curarse - FALTA
        // Interactuar - FALTA
        // Morir - FALTA
    }
    private void Attack()
    {
        playerAnimator.SetTrigger("AttackTrigger");
        //navAgent.destination = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Hit to Enemy");
        }
    }
}
