using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInteract : ItemInteract
{

    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }


    public override void OnInteract()
    {
        ContractorEventSystem.instance.PickUpItemTrigger(this.gameObject);
    }

    public override void OnDrop()
    {
        m_animator.SetBool("Folded", true);
        ContractorEventSystem.instance.EatItem(this.gameObject);
    }
}
