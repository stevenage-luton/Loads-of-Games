using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int sentEmails;

    public int day;

    // Start is called before the first frame update
    void Start()
    {
        sentEmails = 0;
        day = 1;
        GameEventSystem.instance.onSendButtonTrigger += IncrementSentEmails;
        GameEventSystem.instance.onDayEnd += IncrementDay;
        GameEventSystem.instance.onDayBegin += NewDay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IncrementSentEmails()
    {
        sentEmails++;
    }
    void IncrementDay()
    {
        day++;
    }

    void NewDay()
    {
        sentEmails--;

        string dayStr = "Day" + day;
    }
}
