using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkeletonMinionController))]
public class FovEditor : Editor
{
    void OnSceneGUI()
    {
        SkeletonMinionController controller = (SkeletonMinionController)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(controller.transform.position, Vector3.up, Vector3.forward, 360, controller.radius);

        Vector3 viewAngle01 = DirectionFromAngle(controller.transform.eulerAngles.y, -controller.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(controller.transform.eulerAngles.y, controller.angle / 2);

        Handles.color = Color.cyan;
        Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngle01 * controller.radius);
        Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngle02 * controller.radius);

        if (controller.canSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(controller.transform.position, controller.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(MathF.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
