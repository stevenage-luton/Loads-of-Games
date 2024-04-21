using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoliosisJam : AudioPlayer
{
    void Start()
    {
        GameEventSystem.instance.onBeginScoliosisMode += PlaySound;
        GameEventSystem.instance.onEndScoliosisMode += StopSound;
    }


}
