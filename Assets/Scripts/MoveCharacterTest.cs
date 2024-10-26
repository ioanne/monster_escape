using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    private CharacterController controller;
    public Camera mainCamera; // La cámara principal

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Asigna la cámara principal si no se ha asignado
        }
    }

    void Update()
    {
        // Obtén la entrada del teclado
        float horizontal = Input.GetAxis("Horizontal"); // A y D
        float vertical = Input.GetAxis("Vertical"); // W y S

        // Crea un vector de movimiento basado en la orientación de la cámara
        Vector3 forward = mainCamera.transform.forward; // Dirección hacia adelante de la cámara
        Vector3 right = mainCamera.transform.right; // Dirección a la derecha de la cámara

        // Anula el componente Y para que no afecte el movimiento
        forward.y = 0;
        right.y = 0;

        // Normaliza el vector para que no se acelere diagonalmente
        forward.Normalize();
        right.Normalize();

        // Calcula el vector de movimiento
        Vector3 move = right * horizontal + forward * vertical;

        // Mueve el personaje
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
