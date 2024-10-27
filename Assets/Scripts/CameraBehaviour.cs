using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player; // El objeto jugador
    public Vector3 offset; // Desplazamiento de la cámara con respecto al jugador
    public float smoothSpeed = 0.125f; // Velocidad de suavizado

    void LateUpdate()
    {
        // Verificamos que el jugador no sea nulo
        if (player != null)
        {
            // La posición deseada es la posición del jugador más el offset
            Vector3 desiredPosition = player.position + offset;
            // Suavizamos el movimiento de la cámara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Actualizamos la posición de la cámara
            transform.position = smoothedPosition;
        }
    }
}
