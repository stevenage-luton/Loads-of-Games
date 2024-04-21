using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockInteract : Interactable
{
    [SerializeField]
    TMP_Text TextComponent;

    public const int HoursToDay = 24, MinutesToHour = 60;

    public float dayDuration = 100.0f;
    float currentTime = 0;
    public float dayTime;
    string middleString;

    private WaitForSeconds alarmFlashTimer;

    Renderer m_lightRenderer;

    private void Start()
    {       middleString = " : ";
        StartCoroutine(disableAlarm());
    }

    void Update()
    {
        dayTime += Time.deltaTime;
        currentTime = dayTime % dayDuration;

        TextComponent.text = TimeToString();

        m_lightRenderer = GetComponent<Renderer>();

        alarmFlashTimer = new WaitForSeconds(0.5f);
    }

    public float GetCurrentHour()
    {
        return (currentTime * HoursToDay / dayDuration) + 2;
    }

    public float GetCurrentMinute()
    {
        return (currentTime * HoursToDay * MinutesToHour / dayDuration) % MinutesToHour;
    }

    public string TimeToString()
    {
        return Mathf.FloorToInt(GetCurrentHour()).ToString("00") + middleString + Mathf.FloorToInt(GetCurrentMinute()).ToString("00");
    }

    public override void OnInteract()
    {
        StopAllCoroutines();
        TextComponent.enabled = true;
        m_lightRenderer.materials[1].DisableKeyword("_EMISSION");
        StartCoroutine(disableMiddleString());
    }

    IEnumerator enableAlarm()
    {
        yield return alarmFlashTimer;
        TextComponent.enabled = true;
        m_lightRenderer.materials[1].EnableKeyword("_EMISSION");
        StartCoroutine(disableAlarm());
    }

    IEnumerator disableAlarm()
    {
        yield return alarmFlashTimer;
        TextComponent.enabled = false;
        m_lightRenderer.materials[1].DisableKeyword("_EMISSION");
        StartCoroutine(enableAlarm());
    }

    IEnumerator enableMiddleString()
    {
        yield return alarmFlashTimer;
        middleString = "   ";
        StartCoroutine(disableMiddleString());
    }

    IEnumerator disableMiddleString()
    {
        yield return alarmFlashTimer;
        middleString = " : ";
        StartCoroutine(enableMiddleString());
    }
}
