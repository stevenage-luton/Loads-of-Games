using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletNoteInteract : ItemInteract
{
    Animator m_animator;

    [SerializeField]
    AnimationCurve lerpCurve;

    [SerializeField]
    GameObject targetObject;
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_animator.SetBool("Folded", true);

        ContractorEventSystem.instance.onPlayerTeleportToToilet += RemoveFromToilet;

    }

    void RemoveFromToilet()
    {
        StartCoroutine(LerpUpwards());
    }

    IEnumerator LerpUpwards()
    {
        Vector3 targetPosition = targetObject.transform.position; /*+ pickUpObject.transform.forward * heldItemData.zoomDistance;*/

        float lerpDuration = lerpCurve.keys[lerpCurve.length - 1].time;

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = lerpCurve.Evaluate(lerpElapsed / lerpDuration);

            transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, interpolationFactor);

            yield return null;
        }
        m_animator.SetBool("Folded", false);
        ContractorEventSystem.instance.PickUpItemTrigger(this.gameObject);
    }

    public override void OnInteract()
    {
        
    }

    public override void OnDrop()
    {
        m_animator.SetBool("Folded", true);
        ContractorEventSystem.instance.EatItem(this.gameObject);
        ContractorEventSystem.instance.RemoveGunFromToilet();
    }
}
