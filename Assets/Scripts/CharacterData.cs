using UnityEngine;

public enum ECharacterClass
{
    Knight, Ranger, Wizard
}

public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public ECharacterClass Class;
    public Sprite PortraitImage;
}