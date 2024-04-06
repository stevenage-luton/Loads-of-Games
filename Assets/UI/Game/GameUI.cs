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

    [SerializeField]
    VisualTreeAsset m_BarTemplate;

    private VisualElement root;
    private VisualElement Container;

    private VisualElement dayEndElement;

    private VisualElement barElement;

    bool ticking = false;

    public GameController controller;

    Label DayText;

    Button ButtonText;

    // Start is called before the first frame update
    void Start()
    {
        time = startTime;
        _uiDocument.panelSettings.sortingOrder = -1;

        root = _uiDocument.rootVisualElement;
        Container = root.Q<VisualElement>("ScreenContainer");
        timerBar = root.Q<ProgressBar>();

        barElement = root.Q<VisualElement>("DayTimer");

        dayEndElement = root.Q<VisualElement>("LevelEndScreen");
        DayText = dayEndElement.Q<Label>("EmailsAnswered");
        ButtonText = dayEndElement.Q<Button>("EndDayButton");

        ButtonText.RegisterCallback<ClickEvent>(EndDayButtonClick);

        timerBar.highValue =controller.dayDuration;

        GameEventSystem.instance.onDayEnd += DeleteOldBar;
        GameEventSystem.instance.onDayBegin += AddNewBar;

        ticking = true;
    }

    void Update()
    {
        timerBar.value = controller.dayDuration - controller.dayTime;
    }

    void DeleteOldBar() {

        ticking = false;
        _uiDocument.panelSettings.sortingOrder = 2;

        Container.AddToClassList("background--black");
        barElement.RemoveFromClassList("bar--in");
        dayEndElement.RemoveFromClassList("endScreen-off");

        DayText.text = "You answered " + controller.sentEmails + " emails.";

        ButtonText.text = "Click to begin day " + (controller.day+1) + ".";


    }

    void AddNewBar(int day)
    {
        Container.RemoveFromClassList("background--black");
        barElement.AddToClassList("bar--in");
        dayEndElement.AddToClassList("endScreen-off");
        _uiDocument.panelSettings.sortingOrder = -1;
    }

    void EndDayButtonClick(ClickEvent evt)
    {
        GameEventSystem.instance.DayBeginTrigger(controller.day);
    }
}
