using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    public GameController controller;

    [SerializeField]
    GameObject minuteHand;

    [SerializeField]
    GameObject hourHand;

    const float degreesPerHour = 360 / 12, degreesPerMinute = 360 / 60;

    void Update()
    {
        hourHand.transform.rotation = Quaternion.Euler(180, 180, controller.GetCurrentHour() * degreesPerHour);
        minuteHand.transform.rotation = Quaternion.Euler(180, 180, controller.GetCurrentMinute() * degreesPerMinute);
    }
}
