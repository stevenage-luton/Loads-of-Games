using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBoxInteract : Interactable
{
    Animator m_animator;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        m_animator.SetTrigger("Interact");
    }
}
