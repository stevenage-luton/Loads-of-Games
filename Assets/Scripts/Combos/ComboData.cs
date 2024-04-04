using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Animations/Combo Data")]
public class ComboData : ScriptableObject
{
    public AnimationData TorsoPosition;
    public AnimationData ArmsPosition;
    public AnimationData LegsPosition;

    public float StamRegenModifier = 1;

    public float DrainModifier = 1;
}
