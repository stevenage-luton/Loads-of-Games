using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskManAnimationController : MonoBehaviour
{

    Animator m_animator;

    [SerializeField]
    List<AnimationData> m_torsoAnimations;
    [SerializeField]
    List<AnimationData> m_armAnimations;
    [SerializeField]
    List<AnimationData> m_legAnimations;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.Play("Slouch/SlouchLegs/HangingHands");
    }

    void Update()
    {
        
    }
}
