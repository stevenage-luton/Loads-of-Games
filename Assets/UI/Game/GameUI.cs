using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uiDocument;

    private ProgressBar timerBar;

    private float startTime = 30.0f;
    private float time;

    [SerializeField]
    VisualTreeAsset m_DayEndTemplate;

    private VisualElement root;

    bool ticking = false;

    public GameController m_controller;


    // Start is called before the first frame update
    void Start()
    {
        time = startTime;

        root = _uiDocument.rootVisualElement;

        timerBar = root.Q<ProgressBar>();

        GameEventSystem.instance.onDayEnd += DeleteOldBar;

        ticking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ticking)
        {
            return;
        }
        time -= Time.deltaTime;

        timerBar.value = time;

        if (time <= 0.0f)
        {
            GameEventSystem.instance.DayEndTrigger();
        }
    }

    void DeleteOldBar() {
        root.Clear();
        ticking = false;

        var emailBody = m_DayEndTemplate.Instantiate();
        Label DayText = emailBody.Q<Label>("EmailsAnswered");
        DayText.text = "You answered " + m_controller.sentEmails + " emails.";

        Button ButtonText = emailBody.Q<Button>("EndDayButton");
        ButtonText.text = "Click to begin day " + (m_controller.day+1) + ".";

        root.Add(emailBody);

    }
}
