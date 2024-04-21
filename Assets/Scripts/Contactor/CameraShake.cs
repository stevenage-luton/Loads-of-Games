using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve shakeCurve;

    private void Start()
    {
        ContractorEventSystem.instance.onGunFireTrigger += TriggerCameraShake;
        ContractorEventSystem.instance.onDoorKickTrigger += TriggerCameraShake;
    }

    void TriggerCameraShake()
    {
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        Vector3 oldPos = transform.localPosition;

        float lerpDuration = shakeCurve.keys[shakeCurve.length - 1].time;

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = shakeCurve.Evaluate(lerpElapsed / lerpDuration);

            transform.localPosition = oldPos + Random.insideUnitSphere * interpolationFactor;

            yield return null;
        }

        transform.localPosition = oldPos;
    }
}
