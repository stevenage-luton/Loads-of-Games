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

    [SerializeField]
    List<ComboData> m_comboData;

    public ComboData currentCombo;

    public int m_torsoIndex, m_armIndex, m_legIndex = 1;

    string m_animationToPlay;
    void Start()
    {
        m_animationToPlay = m_torsoAnimations[m_torsoIndex].PositionName + "/" + m_legAnimations[m_legIndex].PositionName + "/" + m_armAnimations[m_armIndex].PositionName;

        m_comboData.AddRange(Resources.LoadAll<ComboData>("ComboData"));

        m_animator = GetComponent<Animator>();

        GameEventSystem.instance.updateTorsoPosition += IncrementTorsoAnimation;
        GameEventSystem.instance.updateLegPosition += IncrementLegAnimation;
        GameEventSystem.instance.updateArmPosition += IncrementArmAnimation;

        UpdateCurrentAnimation();
        UpdateCurrentCombo();
    }

    void Update()
    {
        
    }

    void UpdateCurrentAnimation() {
        m_animationToPlay = m_torsoAnimations[m_torsoIndex].PositionName + "/" + m_legAnimations[m_legIndex].PositionName + "/" + m_armAnimations[m_armIndex].PositionName;

        m_animator.Play(m_animationToPlay);
    }

    void UpdateCurrentCombo()
    {
        foreach (ComboData combo in m_comboData)
        {
            if (combo.TorsoPosition.PositionName == m_torsoAnimations[m_torsoIndex].PositionName && combo.ArmsPosition.PositionName == m_armAnimations[m_armIndex].PositionName && combo.LegsPosition.PositionName == m_legAnimations[m_legIndex].PositionName)
            {
                currentCombo = combo;
                return;
            }
        }
    }

    void IncrementTorsoAnimation(int value)
    {
            if (m_torsoIndex + value >= m_torsoAnimations.Count)
            {
                m_torsoIndex = 0;
            }
            else if(m_torsoIndex + value <= 0)
            {
                m_torsoIndex = m_torsoAnimations.Count -1;
            }
            else
            {
                m_torsoIndex += value;
            }
        UpdateCurrentAnimation();
    }
    void IncrementArmAnimation(int value)
    {
        if (m_armIndex + value >= m_armAnimations.Count)
        {
            m_armIndex = 0;
        }
        else if (m_armIndex + value <= 0)
        {
            m_armIndex = m_armAnimations.Count - 1;
        }
        else
        {
            m_armIndex += value;
        }
        UpdateCurrentAnimation();
    }
    void IncrementLegAnimation(int value)
    {
        if (m_legIndex + value >= m_legAnimations.Count)
        {
            m_legIndex = 0;
        }
        else if (m_legIndex + value <= 0)
        {
            m_legIndex = m_legAnimations.Count - 1;
        }
        else
        {
            m_legIndex += value;
        }
        UpdateCurrentAnimation();
    }

}
