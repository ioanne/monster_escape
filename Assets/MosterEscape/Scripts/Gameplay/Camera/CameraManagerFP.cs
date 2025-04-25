using UnityEngine;

public class CameraManagerFP : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private bool isFirstPerson = false; // <<< Nuevo

    private ICameraFollow cameraFollow;
    private CameraDragHandler cameraDragHandler;
    private Camera mainCamera;
    private bool followPlayer = true;
    private float doubleClickTime = 0.3f;
    private float lastClickTime = 0f;

    private void Start()
    {
        mainCamera = Camera.main;
        
        if (!isFirstPerson)
        {
            cameraFollow = new CameraFollow();
            cameraDragHandler = new CameraDragHandler(mainCamera, ResetFollowPlayer);
        }
    }

    private void LateUpdate()
    {
        if (isFirstPerson) return; // Si es primera persona, no hacemos nada de tercera

        if (followPlayer)
        {
            cameraFollow.Follow(player, offset, smoothSpeed);
        }

        cameraDragHandler.HandleDrag(player.position, offset);

        if (Input.GetMouseButtonDown(1))
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            lastClickTime = Time.time;

            if (timeSinceLastClick <= doubleClickTime)
            {
                followPlayer = true;
            }
        }
    }

    private void ResetFollowPlayer()
    {
        followPlayer = false;
    }
}
