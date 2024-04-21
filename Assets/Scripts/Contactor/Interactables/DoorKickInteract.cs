using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKickInteract : Interactable
{
    Animator m_animator;

    ParticleSystem m_particles;
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_particles = transform.Find("DoorParticles").GetComponent<ParticleSystem>();
    }
    public override void OnInteract()
    {
        m_animator.SetBool("Kicked", true);
        ChangeAllLayerMasks(this.gameObject, 0);
        ContractorEventSystem.instance.KickDoor();
        m_particles.Play();
    }

    void ChangeAllLayerMasks(GameObject objectToChange, int layerMask)
    {
        objectToChange.layer = layerMask;
        foreach (Transform child in objectToChange.transform)
        {
            child.gameObject.layer = layerMask;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                ChangeAllLayerMasks(child.gameObject, layerMask);

        }
    }
}
