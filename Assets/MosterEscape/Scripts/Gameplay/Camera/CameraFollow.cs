using UnityEngine;

public class CameraFollow : ICameraFollow
{
    public void Follow(Transform target, Vector3 offset, float smoothSpeed)
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, smoothSpeed);
        Camera.main.transform.position = smoothedPosition;
    }
}
