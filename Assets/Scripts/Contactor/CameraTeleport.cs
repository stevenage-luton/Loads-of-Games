using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeleport : MonoBehaviour
{
    [SerializeField]
    AnimationCurve moveLerpCurve;

    public void TeleportCameraToObject(Camera camera, GameObject start, GameObject target)
    {
        start.transform.position = target.transform.position;
        start.transform.rotation = target.transform.rotation;

        camera.transform.parent = target.transform;
        camera.transform.localPosition = Vector3.zero;
        camera.transform.localEulerAngles = Vector3.zero;
    }

    public void LerpCameraToObject(Camera camera, GameObject target)
    {
        StartCoroutine(LerpToMe(camera, target));
    }

    IEnumerator LerpToMe(Camera camera, GameObject target)
    {
        camera.transform.parent = null;
        Vector3 targetPosition = target.transform.position;
        Quaternion targetRotation = target.transform.rotation;

        Vector3 initialPosition = camera.transform.position;
        Quaternion initialRotation = camera.transform.rotation;

        float lerpDuration = moveLerpCurve.keys[moveLerpCurve.length - 1].time;

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = moveLerpCurve.Evaluate(lerpElapsed / lerpDuration);

            camera.transform.position = Vector3.LerpUnclamped(initialPosition, targetPosition, interpolationFactor);
            camera.transform.rotation = Quaternion.LerpUnclamped(initialRotation, targetRotation, interpolationFactor);
            yield return null;
        }
        camera.transform.parent = target.transform;
    }
}
