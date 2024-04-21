using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteract : Interactable
{
    Animator m_animator;

    [SerializeField]
    List<Light> m_lights;

    [SerializeField]
    List<GameObject> m_lightBase;

    [SerializeField]
    Light sun;

    bool m_lightsOn;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_lightsOn = false;

        foreach (Light light in m_lights)
        {
            light.enabled = m_lightsOn;
        }
    }

    public void ClickOn()
    {
        m_lightsOn = !m_lightsOn;
        foreach (GameObject light in m_lightBase)
        {
            var lightRender = light.GetComponent<Renderer>();
            if (m_lightsOn)
            {
                lightRender.material.EnableKeyword("_EMISSION");
            }
            else
            {
                lightRender.material.DisableKeyword("_EMISSION");
            }

        }

        foreach (Light light in m_lights)
        {
            light.enabled = m_lightsOn;
        }

        if (m_lightsOn)
        {
            sun.intensity = Constants.SUN_ON;
        }
        else
        {
            sun.intensity = Constants.SUN_OFF;
        }
    }

    public override void OnInteract()
    {
        m_animator.SetTrigger("Interact");
        
    }


}
