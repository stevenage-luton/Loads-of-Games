using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteract : Interactable
{
    AudioSource audioPlayer;

    public AudioClip sitDownClip;
    public AudioClip standUpClip;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        GameEventSystem.instance.onEndInteract += PlayStandUpSound;
    }

    public override void OnInteract()
    {
        GameEventSystem.instance.ComputerInteractTrigger();
        audioPlayer.PlayOneShot(sitDownClip);
    }
    public override string OnHover()
    {
        return "access computer";
    }

    private void PlayStandUpSound()
    {
        audioPlayer.PlayOneShot(standUpClip);
    }
}
