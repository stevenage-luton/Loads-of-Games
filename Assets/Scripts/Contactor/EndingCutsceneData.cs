using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ending/Cutscene")]
public class EndingCutsceneData : ScriptableObject
{
    public List<CutsceneLine> cutscene;
}
