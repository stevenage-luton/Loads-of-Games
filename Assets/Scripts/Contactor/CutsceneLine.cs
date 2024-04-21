using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ending/SingleLine")]
public class CutsceneLine : ScriptableObject
{
    [SerializeField] private float m_duration;

    [TextArea(15, 20)]
    public string Line;

    public float fullDuration { get { return m_duration + 1.0f; } }
}
