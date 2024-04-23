using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public const int HoursToDay = 8, MinutesToHour = 60;

    public float dayDuration = 100.0f;
    public float maxSpineHealth = 60.0f;

    public float timeSinceLastSeatChange = 0.0f;
    public float minTimeBetweenSeatChanges = 5.0f;

    public int finalDay = 4;
    float endGameTimer;

    public int sentEmails;

    public int day;

    float currentTime = 0;
    public float dayTime;
    public float spineHealth;

    float scoliosisTimer = 20.0f;

    float scoliosisEndValue = 0.0f;

    bool ticking = false;

    GameObject Player;
    GameObject PlayerStart;

    List<EmailData> emails;
    Dictionary<float, EmailData> m_emailsToRecieve;

    float valueToRemove;

    float spineDrainModifier;
    public float passiveRegenModifier = 1;

    public ComboData ActualCombo;
    public ComboData AnimationCombo;

    bool seated = false;
    bool readyToUseSpineMode = true;


    // Start is called before the first frame update
    void Start()
    {
        day = 1;
        GameEventSystem.instance.onSendButtonTrigger += IncrementSentEmails;
        GameEventSystem.instance.onDayEnd += IncrementDay;
        GameEventSystem.instance.onDayBegin += NewDay;
        GameEventSystem.instance.awaitNextEmailInChain += AddNewEmailToThread;
        GameEventSystem.instance.onSpineModeButton += StopTicking;
        GameEventSystem.instance.onEndSpineModeButton += StartTicking;
        GameEventSystem.instance.onUpdateComboAnimation += UpdateAnimationCombo;
        GameEventSystem.instance.onConfirmSpineMode += UpdateSpineCombo;
        GameEventSystem.instance.onGenericDayStart += UpdateSpineCombo;
        GameEventSystem.instance.onComputerInteract += SitDown;
        GameEventSystem.instance.onEndInteract += StandUp;

        GameEventSystem.instance.onEndGame += StopTicking;
        GameEventSystem.instance.onEndGame += StandUp;

        endGameTimer = 0;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStart = GameObject.FindGameObjectWithTag("PlayerStart");
        GameEventSystem.instance.DayBeginTrigger(day);
        timeSinceLastSeatChange = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ticking)
        {
            return;
        }
        dayTime += Time.deltaTime;

        timeSinceLastSeatChange += Time.deltaTime;

        if (timeSinceLastSeatChange >= minTimeBetweenSeatChanges && readyToUseSpineMode)
        {
            readyToUseSpineMode = false;
            GameEventSystem.instance.SignalToggleSpineUpdateReady();
        }

        if (seated == true)
        {
            spineHealth += Time.deltaTime * spineDrainModifier;
        }
        else if (seated == false)
        {
            spineHealth -= Time.deltaTime / passiveRegenModifier;
        }

        if (spineHealth > maxSpineHealth)
        {
            GameEventSystem.instance.EndInteractTrigger();
            GameEventSystem.instance.BeginScoliosisMode();
            StartScoliosisTimer();
        }



        spineHealth = Mathf.Clamp(spineHealth, 0.0f, maxSpineHealth);

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

        if (dayTime >= scoliosisEndValue && scoliosisEndValue > 0.0)
        {
            GameEventSystem.instance.EndScoliosisMode();
            scoliosisEndValue = 0.0f;
        }

        if (endGameTimer != 0)
        {
            if (dayTime >= endGameTimer)
            {
                GameEventSystem.instance.EndGame();
            }
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
        GameEventSystem.instance.EndScoliosisMode();

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
        timeSinceLastSeatChange = 5.0f;
        scoliosisEndValue = 0.0f;
        readyToUseSpineMode = true;
        m_emailsToRecieve = new Dictionary<float, EmailData>();
        emails = new List<EmailData>();
        StandUp();
        dayTime = 0.0f;
        spineHealth = 0.0f;
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
        if (finalDay == day)
        {
            endGameTimer = Random.Range(10.0f, 20.0f);
        }

        //if (finalDay == day)
        //{
        //    endGameTimer = Random.Range(maxSpineHealth, dayDuration - 60.0f);
        //}
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

    void StopTicking()
    {
        ticking = false;
    }

    void StartTicking()
    {
        ticking = true;
    }

    void UpdateAnimationCombo(ComboData data)
    {
        AnimationCombo = data;
        GameEventSystem.instance.UpdateComboUI();
    }

    void UpdateSpineCombo()
    {
        ActualCombo = AnimationCombo;
        float bonusHealth = (ActualCombo.ArmsPosition.StaminaRegain + ActualCombo.TorsoPosition.StaminaRegain + ActualCombo.LegsPosition.StaminaRegain) * ActualCombo.StamRegenModifier;
        spineDrainModifier = ActualCombo.DrainModifier;
        spineHealth = Mathf.Max(0.0f, spineHealth - bonusHealth);
        timeSinceLastSeatChange = 0.0f;
        GameEventSystem.instance.SignalToggleSpineUpdateReady();
        readyToUseSpineMode = true;
    }

    void SitDown()
    {
        seated = true;
    }
    void StandUp()
    {
        seated = false;
    }
    void StartScoliosisTimer()
    {
        scoliosisEndValue = dayTime + scoliosisTimer;
    }
}
