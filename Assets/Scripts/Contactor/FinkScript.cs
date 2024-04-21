using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinkScript : MonoBehaviour
{
    [SerializeField]
    ParticleSystem snoozing;

    Animator m_animator;

    [SerializeField]
    ParticleSystem m_bloodBurst;

    [SerializeField] List<GameObject> m_decals = new List<GameObject>();


    private void Start()
    {
        m_animator = GetComponent<Animator>();

        foreach (GameObject decal in m_decals)
        {
            decal.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            snoozing.Stop();
            m_animator.SetBool("Dead", true);
            ContractorEventSystem.instance.KillTarget();
            m_bloodBurst.Play();
            StartCoroutine(ToggleDecals());
        }
    }

    IEnumerator ToggleDecals()
    {
        foreach (GameObject decal in m_decals)
        {
            decal.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.05f,0.1f));
        }
    }
}
