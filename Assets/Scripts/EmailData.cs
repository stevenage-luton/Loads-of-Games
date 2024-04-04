using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Emails/Email Data")]
public class EmailData : ScriptableObject
{
    public EmailUser EmailUser;
    public List<EmailUser> Recipient;
    public string Subject;
    [TextArea(10, 100)]
    public string EmailBody;
    public string time = "09:00";
    //public Sprite PortraitImage;

    public bool hasReply = false;

    public bool replied = false;

    public EmailData reply;

    public List<EmailData> nextInThread;

    public float threadTime;
    public float minTime;
    public float maxTime;
}