using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;


public class TextAnimator : MonoBehaviour
{
    Transform m_Transform;

    public AnimationCurve collectLerpCurve;

    public Vector3 startPosition;

    public float moveDistance = 1;

    public TMP_Text TextComponent;

    private IObjectPool<TextAnimator> pool;

    public IObjectPool<TextAnimator> ObjectPool { set => pool = value; }

    void Awake()
    {
        m_Transform = transform;
        startPosition = m_Transform.position;

        TextComponent = GetComponentInChildren<TMP_Text>();
    }

    public void BeginLerp()
    {
        StartCoroutine(LerpUpwards());
    }

    public IEnumerator LerpUpwards()
    {
        float lerpDuration = collectLerpCurve.keys[collectLerpCurve.length - 1].time;

        Vector3 endPosition = new(startPosition.x, startPosition.y + moveDistance, startPosition.z);

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = collectLerpCurve.Evaluate(lerpElapsed / lerpDuration);

            m_Transform.position = Vector3.LerpUnclamped(startPosition, endPosition, interpolationFactor);
            yield return null;
        }

        pool.Release(this);
    }
}
