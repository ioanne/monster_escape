using UnityEngine;
using System;

public class CameraDragHandler
{
    private readonly Camera camera;
    private Vector3 origin;
    private Vector3 difference;
    private bool isDragging = false;
    private bool returnToPosition = false;
    private float returnSpeed = 0.2f;
    private Action onDragStart;

    public CameraDragHandler(Camera camera, Action onDragStart)
    {
        this.camera = camera;
        this.onDragStart = onDragStart;
    }

    public void HandleDrag(Vector3 playerPosition, Vector3 offset)
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Comenzar a arrastrar la cámara
            isDragging = true;
            origin = camera.ScreenToWorldPoint(Input.mousePosition);
            onDragStart?.Invoke(); // Llamar a la acción para desactivar el seguimiento
        }

        if (Input.GetMouseButton(1) && isDragging)
        {
            // Mover la cámara mientras se arrastra con el clic derecho
            difference = camera.ScreenToWorldPoint(Input.mousePosition) - camera.transform.position;
            camera.transform.position = origin - difference;
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Dejar de arrastrar al soltar el clic derecho
            isDragging = false;
        }

        if (returnToPosition)
        {
            // Volver a la posición original de forma gradual si es necesario
            Vector3 targetPosition = playerPosition + offset;
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, returnSpeed);

            if (Vector3.Distance(camera.transform.position, targetPosition) < 0.1f)
            {
                returnToPosition = false;
            }
        }
    }
}
