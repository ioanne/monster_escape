using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float closedRotation = 0f;
    [SerializeField] private float openRotation = -89.997f;
    [SerializeField] private float transitionDuration = 1.5f;
    private bool isOpen = false;
    private float stepIncrement; // Incremento de cada paso

    void Start()
    {
        // Calcula el incremento por paso según el tiempo total y los grados a recorrer
        float totalDegrees = Mathf.Abs(openRotation - closedRotation);
        stepIncrement = totalDegrees / (transitionDuration / Time.deltaTime);
    }

    public void Interact()
    {
        isOpen = !isOpen;
        float targetRotation = isOpen ? openRotation : closedRotation;
        StartCoroutine(RotateDoorInSteps(targetRotation));
    }

    private System.Collections.IEnumerator RotateDoorInSteps(float targetRotation)
    {
        float currentRotation = transform.localEulerAngles.y;
        if (currentRotation > 180f) currentRotation -= 360f; // Asegúrate de que la rotación sea en el rango correcto

        float direction = isOpen ? -1 : 1; // Define si estamos abriendo o cerrando
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration && Mathf.Abs(currentRotation - targetRotation) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            currentRotation += stepIncrement * direction; // Incrementa o decrementa la rotación en pasos
            currentRotation = Mathf.Clamp(currentRotation, Mathf.Min(closedRotation, openRotation), Mathf.Max(closedRotation, openRotation));
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentRotation, transform.localEulerAngles.z);

            yield return null;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetRotation, transform.localEulerAngles.z); // Asegúrate de alcanzar la rotación final exacta
    }
}
