using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHouseDoorInteract : Interactable
{
    AudioSource m_AudioSource;
    bool canLeave = false;

    [SerializeField] Light sun;
    private void Start()
    {
        ContractorEventSystem.instance.eatItemTrigger += EnableLeave;
        m_AudioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        if (canLeave)
        {
            ContractorEventSystem.instance.ElevatorTeleport();
            sun.intensity = Constants.SUN_OFF;
            m_AudioSource.Play();
        }
    }

    void EnableLeave(GameObject note)
    {
        canLeave = true;
    }
}
