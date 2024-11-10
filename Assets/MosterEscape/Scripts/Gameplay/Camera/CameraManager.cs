using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private ICameraFollow cameraFollow;
    private CameraDragHandler cameraDragHandler;
    private Camera mainCamera;
    private bool followPlayer = true; // Inicia siguiendo al jugador
    private float doubleClickTime = 0.3f; // Tiempo máximo entre clics para considerar un doble clic
    private float lastClickTime = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        cameraFollow = new CameraFollow();
        cameraDragHandler = new CameraDragHandler(mainCamera, ResetFollowPlayer);
    }

    void LateUpdate()
    {
        // Seguir al jugador si followPlayer es verdadero
        if (followPlayer)
        {
            cameraFollow.Follow(player, offset, smoothSpeed);
        }

        // Manejar el arrastre de la cámara
        cameraDragHandler.HandleDrag(player.position, offset);

        // Detectar doble clic derecho para volver a seguir al jugador
        if (Input.GetMouseButtonDown(1))
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            lastClickTime = Time.time;

            if (timeSinceLastClick <= doubleClickTime)
            {
                // Doble clic detectado: activar el seguimiento del jugador
                followPlayer = true;
            }
        }
    }

    // Método para desactivar el seguimiento del jugador
    void ResetFollowPlayer()
    {
        followPlayer = false;
    }
}
