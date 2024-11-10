using UnityEngine;

public interface ICameraFollow
{
    void Follow(Transform target, Vector3 offset, float smoothSpeed);
}
