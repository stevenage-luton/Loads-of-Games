using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ComboData : ScriptableObject
{
    public AnimationData TorsoPosition;
    public AnimationData ArmsPosition;
    public AnimationData LegsPosition;

    public float StamRegenModifier;

    public float DrainModifier;
}
