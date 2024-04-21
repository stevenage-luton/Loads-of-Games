using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DOFScript : MonoBehaviour
{
    public VolumeProfile _Profile;
    UnityEngine.Rendering.Universal.DepthOfField depthOfField;

    ClampedFloatParameter maxBlur;
    int maxFocal = 5;
    int minFocal = 0;

    private WaitForSeconds m_fadeDelay;

    void Awake()
    {
        _Profile.TryGet(out depthOfField);
        depthOfField.active = false;
        maxBlur = depthOfField.focalLength;
        depthOfField.focalLength.value = minFocal;
        depthOfField.gaussianStart.value = minFocal;
    }

    private void Start()
    {
        m_fadeDelay = new WaitForSeconds(0.1f);
        ContractorEventSystem.instance.onPickUpGeneric += DoFTrigger;
        ContractorEventSystem.instance.onDropGeneric += DisableDoFTrigger;
    }

    void DoFTrigger()
    {
        StopAllCoroutines();
        StartCoroutine(EnableDepthOfField());
    }

    void DisableDoFTrigger()
    {
        StopAllCoroutines();
        StartCoroutine(DisableDepthOfField());
    }

    public IEnumerator EnableDepthOfField()
    {
        depthOfField.active = true;
        depthOfField.gaussianStart.value = maxFocal;

        for (int i = minFocal; i < maxFocal; i++)
        {
            depthOfField.gaussianStart.value--;
            yield return m_fadeDelay;
        }

    }

    public IEnumerator DisableDepthOfField()
    {

        depthOfField.gaussianStart.value = minFocal;

        for (int i = minFocal; i < maxFocal; i++)
        {
            depthOfField.gaussianStart.value++;
            yield return m_fadeDelay;
        }
        depthOfField.active = false;
    }
}
