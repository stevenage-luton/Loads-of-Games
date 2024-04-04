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

    bool ticking = false;

    public GameController controller;


    // Start is called before the first frame update
    void Start()
    {
        time = startTime;
        _uiDocument.panelSettings.sortingOrder = -1;

        root = _uiDocument.rootVisualElement;
        Container = root.Q<VisualElement>("ScreenContainer");
        timerBar = root.Q<ProgressBar>();

        timerBar.highValue =controller.dayDuration;

        GameEventSystem.instance.onDayEnd += DeleteOldBar;
        GameEventSystem.instance.onDayBegin += AddNewBar;

        ticking = true;
    }

    // Update is called once per frame
    void Update()
    {
        timerBar.value = controller.dayDuration - controller.dayTime;
    }

    void DeleteOldBar() {
        Container.Clear();

        ticking = false;
        _uiDocument.panelSettings.sortingOrder = 2;
        var DayEndTemp = m_DayEndTemplate.Instantiate();
        Label DayText = DayEndTemp.Q<Label>("EmailsAnswered");
        DayText.text = "You answered " + controller.sentEmails + " emails.";

        Button ButtonText = DayEndTemp.Q<Button>("EndDayButton");
        ButtonText.text = "Click to begin day " + (controller.day+1) + ".";

        ButtonText.RegisterCallback<ClickEvent>(EndDayButtonClick);

        DayEndTemp.style.flexGrow = 1f;

        Container.Add(DayEndTemp);

    }

    void AddNewBar(int day)
    {
        Container.Clear();
        _uiDocument.panelSettings.sortingOrder = -1;
        var BarTemp = m_BarTemplate.Instantiate();

        Container.Add(BarTemp);

        BarTemp.style.alignItems = Align.Stretch;
        BarTemp.style.justifyContent = Justify.FlexEnd;
        BarTemp.style.flexGrow = 1f;
        BarTemp.style.marginLeft = 80.0f;
        BarTemp.style.marginRight = 80.0f;

        timerBar = root.Q<ProgressBar>();
        timerBar.highValue = controller.dayDuration;
    }

    void EndDayButtonClick(ClickEvent evt)
    {
        GameEventSystem.instance.DayBeginTrigger(controller.day);
    }
}
