using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    private CharacterController controller;
    public Camera mainCamera; // La camara principal
    public NavMeshAgent navAgent;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Asigna la camara principal si no se ha asignado
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Movimiento();
        }
    }

    void Movimiento(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            navAgent.destination = hit.point;
        }
    }
}
