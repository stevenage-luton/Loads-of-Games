using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="HoverText/List")]
public class TextLineStorageObject : ScriptableObject
{
    public List<string> lines = new List<string>();
}
