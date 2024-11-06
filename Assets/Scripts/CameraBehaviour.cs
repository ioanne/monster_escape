using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player; // El objeto jugador
    public Vector3 offset; // Desplazamiento de la c�mara con respecto al jugador
    public float smoothSpeed = 0.125f; // Velocidad de suavizado

    // Pruebas de movimiento de la camara
    public Camera CameraObj;
    private Vector3 origin;
    private Vector3 difference;
    private Vector3 resetCamera;
    private bool isDragging = false;

    void Start()
    {
        CameraObj = Camera.main;
        resetCamera = CameraObj.transform.position;
    }
    void LateUpdate()
    {
        // Verificamos que el jugador no sea nulo
        if (player != null)
        {
            // La posici�n deseada es la posici�n del jugador m�s el offset
            Vector3 desiredPosition = player.position + offset;
            // Suavizamos el movimiento de la c�mara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Actualizamos la posici�n de la c�mara
            transform.position = smoothedPosition;
        }

        if (Input.GetMouseButton(1))
        {
            difference = CameraObj.ScreenToWorldPoint(Input.mousePosition) - CameraObj.transform.position;
            if (isDragging == false)
            {
                isDragging = true;
                origin = CameraObj.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            isDragging = false;
        }
            
        if (isDragging)
        {
            CameraObj.transform.position = origin - difference;
        }

        if (Input.GetMouseButtonUp(1))
        {
            CameraObj.transform.position = player.position + offset;
        }
    }
}
