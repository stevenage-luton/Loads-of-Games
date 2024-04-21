using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : AudioPlayer
{
    void Start()
    {
        GameEventSystem.instance.onGenericDayStart += PlaySound;
    }

}
