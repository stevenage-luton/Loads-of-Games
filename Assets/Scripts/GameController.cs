using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public const int HoursToDay = 8, MinutesToHour = 60;

    public float dayDuration = 100.0f;

    public int sentEmails;

    public int day;

    float currentTime = 0;
    public float dayTime;

    bool ticking = false;

    GameObject Player;
    GameObject PlayerStart;

    List<EmailData> emails;
    Dictionary<float, EmailData> m_emailsToRecieve;

    float valueToRemove;

    // Start is called before the first frame update
    void Start()
    {
        day = 1;
        GameEventSystem.instance.onSendButtonTrigger += IncrementSentEmails;
        GameEventSystem.instance.onDayEnd += IncrementDay;
        GameEventSystem.instance.onDayBegin += NewDay;
        GameEventSystem.instance.awaitNextEmailInChain += AddNewEmailToThread;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStart = GameObject.FindGameObjectWithTag("PlayerStart");
        NewDay(day);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ticking)
        {
            return;
        }
        dayTime += Time.deltaTime;
        currentTime = dayTime % dayDuration;

        foreach (KeyValuePair<float, EmailData> kvp in m_emailsToRecieve)
        {
            if (dayTime >= kvp.Key)
            {
                kvp.Value.time = TimeToString();
                GameEventSystem.instance.RecieveEmail(kvp.Value);
                valueToRemove = kvp.Key;
            }
        }
        if (valueToRemove != 0)
        {
            m_emailsToRecieve.Remove(valueToRemove);
            valueToRemove = 0;
        }
        

        if (dayTime >= dayDuration)
        {
            GameEventSystem.instance.DayEndTrigger();
        }
    }

    public float GetCurrentHour()
    {
        return (currentTime * HoursToDay / dayDuration) + 9;
    }

    public float GetCurrentMinute()
    {
        return (currentTime * HoursToDay * MinutesToHour / dayDuration) % MinutesToHour;
    }

    public string TimeToString()
    {
        return Mathf.FloorToInt(GetCurrentHour()).ToString("00") + ":" + Mathf.FloorToInt(GetCurrentMinute()).ToString("00");
    }

    void IncrementSentEmails()
    {
        sentEmails++;
    }
    void IncrementDay()
    {
        ticking = false;
        day++;

        //foreach (KeyValuePair<float, EmailData> kvp in m_emailsToRecieve)
        //{
        //    kvp.Value.replied = true;
        //    GameEventSystem.instance.RecieveEmail(kvp.Value);
        //}

    }

    void NewDay(int day)
    {
        m_emailsToRecieve = new Dictionary<float, EmailData>();
        emails = new List<EmailData>();

        dayTime = 0.0f;
        sentEmails = 0;
        ticking = true;
        
        string dayStr = "Day" + day;

        CharacterController playerController = Player.GetComponent<CharacterController>();
        playerController.enabled = false;
        Player.transform.position = PlayerStart.transform.position;     
        playerController.enabled = true;
        Player.transform.rotation = PlayerStart.transform.rotation;
        float prot = Player.GetComponentInChildren<PlayerLook>().playerRotation = 0.0f;
        Camera.main.transform.localRotation = Quaternion.Euler(prot, 0f, 0f);

        

        emails.AddRange(Resources.LoadAll<EmailData>("Emails/" + dayStr + "/Incoming"));

        foreach (EmailData email in emails)
        {
            email.replied = false;
            AddNewEmailToThread(email);
        }
    }

    float GenerateRandomTime(float minTime, float maxTime)
    {
        return Random.Range(minTime, maxTime);
    }

    void AddNewEmailToThread(EmailData data)
    {
        float nextEmailTime = data.threadTime;
        if (nextEmailTime == 0)
        {
            nextEmailTime = GenerateRandomTime(data.minTime, data.maxTime);
        }

        nextEmailTime += dayTime;

        Debug.Log("EmailAdded, time until: " + nextEmailTime);
        m_emailsToRecieve.Add(nextEmailTime, data);
    }
}
