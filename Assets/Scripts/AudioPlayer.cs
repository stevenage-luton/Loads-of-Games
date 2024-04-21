using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource m_AudioSource;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    protected void PlaySound()
    {
        m_AudioSource.Play();
    }

    protected void StopSound()
    {
        m_AudioSource.Stop();
    }

}
