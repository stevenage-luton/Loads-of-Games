using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TannoySound : AudioPlayer
{
    void Start()
    {
        GameEventSystem.instance.onDayEnd += PlaySound;
    }

}
