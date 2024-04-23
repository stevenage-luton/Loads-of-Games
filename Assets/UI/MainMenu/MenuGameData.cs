using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Main Menu/GameInfo")]
public class MenuGameData : ScriptableObject
{
    public string gameName;
    public string charName;
    public string job;
    public string objective;
    public string sceneName;
}
