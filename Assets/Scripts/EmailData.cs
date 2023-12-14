using UnityEngine;


[CreateAssetMenu] //This adds an entry to the **Create** menu
public class EmailData : ScriptableObject
{
    public string EmailUserName;
    public string EmailAddress;
    public string RecipientAddress;
    public string Subject;
    [TextArea(10, 100)]
    public string EmailBody;
    public string time = "09:00";
    public Sprite PortraitImage;

    public EmailData reply;
}