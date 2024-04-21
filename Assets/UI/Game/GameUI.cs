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
    private VisualElement SpineButtonsElement;
    private VisualElement barElement;

    private VisualElement ScoliosisModeContainer;

    private VisualElement ScoliosisTextContainer;

    public GameController controller;

    Label DayText, ArmsText, TorsoText, LegsText, ComboNickname, SpineReminder, DrainLabel, RegenLabel, ScoliosisMode;

    Button EndDayButton, PlusOneLegButton, MinusOneLegButton, PlusOneTorsoButton, MinusOneTorsoButton, PlusOneArmsButton, MinisOneArmsButton, ConfirmComboButton;

    private AudioSource audioSource;

    [SerializeField]
    AudioClip introScratch, outroScratch;

    // Start is called before the first frame update
    void Start()
    {
        time = startTime;
        _uiDocument.panelSettings.sortingOrder = -1;

        root = _uiDocument.rootVisualElement;
        Container = root.Q<VisualElement>("ScreenContainer");
        timerBar = root.Q<ProgressBar>();

        SpineButtonsElement = root.Q<VisualElement>("ButtonsContainer");

        ScoliosisModeContainer = root.Q<VisualElement>("ScoliosisModeContainer");
        ScoliosisTextContainer = root.Q<VisualElement>("ScoliosisContainer");
        ScoliosisMode = root.Q<Label>("ScoliosisText");

        barElement = root.Q<VisualElement>("DayTimer");

        SpineReminder = barElement.Q<Label>("SpineReminder");

        SpineReminder.style.opacity = 0;

        dayEndElement = root.Q<VisualElement>("LevelEndScreen");
        DayText = dayEndElement.Q<Label>("EmailsAnswered");

        EndDayButton = dayEndElement.Q<Button>("EndDayButton");

        ArmsText = SpineButtonsElement.Q<Label>("ArmsDescription");
        TorsoText = SpineButtonsElement.Q<Label>("TorsoDescription");
        LegsText = SpineButtonsElement.Q<Label>("LegsDescription");

        ComboNickname = SpineButtonsElement.Q<Label>("BottomLabel");
        DrainLabel = SpineButtonsElement.Q<Label>("DrainModifierLabel");
        RegenLabel = SpineButtonsElement.Q<Label>("StamRegenLabel");

        ConfirmComboButton = SpineButtonsElement.Q<Button>("ConfirmButton");

        PlusOneLegButton = SpineButtonsElement.Q<Button>("LegsPlusOne");
        MinusOneLegButton = SpineButtonsElement.Q<Button>("LegsMinusOne");
        PlusOneTorsoButton = SpineButtonsElement.Q<Button>("TorsoPlusOne");
        MinusOneTorsoButton = SpineButtonsElement.Q<Button>("TorsoMinusOne");
        PlusOneArmsButton = SpineButtonsElement.Q<Button>("ArmsPlusOne");
        MinisOneArmsButton = SpineButtonsElement.Q<Button>("ArmsMinusOne");

        EndDayButton.RegisterCallback<ClickEvent>(EndDayButtonClick);

        PlusOneLegButton.RegisterCallback<ClickEvent, int>(PressSpineModeButtonLeg, 1);
        MinusOneLegButton.RegisterCallback<ClickEvent, int>(PressSpineModeButtonLeg, -1);
        PlusOneTorsoButton.RegisterCallback<ClickEvent, int>(PressSpineModeButtonTorso, 1);
        MinusOneTorsoButton.RegisterCallback<ClickEvent, int>(PressSpineModeButtonTorso, -1);
        PlusOneArmsButton.RegisterCallback<ClickEvent, int>(PressSpineModeButtonArm, 1);
        MinisOneArmsButton.RegisterCallback<ClickEvent, int>(PressSpineModeButtonArm, -1);

        timerBar.highValue = controller.maxSpineHealth;

        GameEventSystem.instance.onDayEnd += DeleteOldBar;
        GameEventSystem.instance.onDayBegin += AddNewBar;
        GameEventSystem.instance.onSpineModeButton += EnterSpineModeUI;
        GameEventSystem.instance.onEndSpineModeButton += ExitSpineModeUI;
        GameEventSystem.instance.onUpdateComboUI += UpdateAnimationComboUI;
        GameEventSystem.instance.onComputerInteract += ToggleSpineWarning;
        GameEventSystem.instance.onEndInteract += ToggleSpineWarning;

        GameEventSystem.instance.onBeginScoliosisMode += AnimateScoliosis;
        GameEventSystem.instance.onEndScoliosisMode += StopScoliosis;

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        timerBar.value = controller.maxSpineHealth - controller.spineHealth;

        SpineReminder.style.opacity = Mathf.InverseLerp(0, controller.maxSpineHealth / 2.0f, controller.spineHealth - controller.maxSpineHealth / 1.5f);
    }

    void DeleteOldBar() {

        _uiDocument.panelSettings.sortingOrder = 2;

        Container.AddToClassList("background--black");
        barElement.RemoveFromClassList("bar--in");
        dayEndElement.RemoveFromClassList("endScreen-off");

        DayText.text = "You answered " + controller.sentEmails + " emails.";

        EndDayButton.text = "Click to begin day " + (controller.day + 1) + ".";


    }

    void AddNewBar(int day)
    {
        Container.RemoveFromClassList("background--black");
        barElement.AddToClassList("bar--in");
        dayEndElement.AddToClassList("endScreen-off");
        _uiDocument.panelSettings.sortingOrder = -1;
        SpineReminder.visible = false;
    }

    void EndDayButtonClick(ClickEvent evt)
    {
        GameEventSystem.instance.DayBeginTrigger(controller.day);
        controller.timeSinceLastSeatChange = 5.0f;
    }

    void PressSpineModeButtonArm(ClickEvent evt, int incrementVal)
    {
        GameEventSystem.instance.ChangeArmTrigger(incrementVal);
    }
    void PressSpineModeButtonLeg(ClickEvent evt, int incrementVal)
    {
        GameEventSystem.instance.ChangeLegTrigger(incrementVal);
    }
    void PressSpineModeButtonTorso(ClickEvent evt, int incrementVal)
    {
        GameEventSystem.instance.ChangeTorsoTrigger(incrementVal);
    }

    void EnterSpineModeUI()
    {
        SpineButtonsElement.AddToClassList("SpineUI");
        Container.RemoveFromClassList("background--Out");
        barElement.RemoveFromClassList("bar--in");
        audioSource.PlayOneShot(introScratch);
    }

    void ExitSpineModeUI()
    {
        SpineButtonsElement.RemoveFromClassList("SpineUI");
        Container.AddToClassList("background--Out");
        audioSource.PlayOneShot(outroScratch);
        barElement.AddToClassList("bar--in");
    }

    void UpdateAnimationComboUI()
    {
        ArmsText.text = controller.AnimationCombo.ArmsPosition.PositionName;
        TorsoText.text = controller.AnimationCombo.TorsoPosition.PositionName;
        LegsText.text = controller.AnimationCombo.LegsPosition.PositionName;
        ComboNickname.text = '"' + controller.AnimationCombo.ComboNickname + '"';

        if (controller.AnimationCombo.DrainModifier <= 0.7)
        {
            DrainLabel.text = "Very Good";
        }
        else if (controller.AnimationCombo.DrainModifier > 0.7 && controller.AnimationCombo.DrainModifier <= 0.99)
        {
            DrainLabel.text = "Good";
        }
        else if (controller.AnimationCombo.DrainModifier > 0.99 && controller.AnimationCombo.DrainModifier <= 1.01)
        {
            DrainLabel.text = "Average";
        }
        else if (controller.AnimationCombo.DrainModifier > 1.01 && controller.AnimationCombo.DrainModifier <= 1.2)
        {
            DrainLabel.text = "Bad";
        }
        else if (controller.AnimationCombo.DrainModifier > 1.2)
        {
            DrainLabel.text = "Very Bad";
        }

        float stamRegen = SumAnimationRegen();

        if (stamRegen <= 2.0)
        {
            RegenLabel.text = "Actively Harmful";

        }
        else if (stamRegen > 2.0 && stamRegen <= 7.0)
        {
            RegenLabel.text = "Very Bad";

        }else if (stamRegen > 7.0 && stamRegen <= 12.0)
        {
            RegenLabel.text = "Bad";
        }
        else if (stamRegen > 12.0 && stamRegen <= 15.0)
        {
            RegenLabel.text = "Average";
        }
        else if (stamRegen > 15.0 && stamRegen <= 20.0)
        {
            RegenLabel.text = "Good";
        }
        else if (stamRegen > 20.0 && stamRegen <= 30.0)
        {
            RegenLabel.text = "Very Good";
        }
        else if (stamRegen > 30.0)
        {
            RegenLabel.text = "Excellent";
        }

        if (controller.AnimationCombo == controller.ActualCombo)
        {
            ConfirmComboButton.RemoveFromClassList("ConfirmButton--In");
            ConfirmComboButton.UnregisterCallback<ClickEvent>(PressComboButton);
            ConfirmComboButton.text = "Must change seating position";
        }
        else
        {
            ConfirmComboButton.AddToClassList("ConfirmButton--In");
            ConfirmComboButton.RegisterCallback<ClickEvent>(PressComboButton);
            ConfirmComboButton.text = "Confirm Combo";
        }
    }

    float SumAnimationRegen()
    {
        return (controller.AnimationCombo.ArmsPosition.StaminaRegain + controller.AnimationCombo.TorsoPosition.StaminaRegain + controller.AnimationCombo.LegsPosition.StaminaRegain) * controller.AnimationCombo.StamRegenModifier;
    }

    void PressComboButton(ClickEvent evt)
    {
        GameEventSystem.instance.ConfirmSpineModeButton();
    }

    void ToggleSpineWarning()
    {
        SpineReminder.visible = !SpineReminder.visible;
    }

    void AnimateScoliosis()
    {
        ScoliosisModeContainer.style.display = DisplayStyle.Flex;
        WobbleText();
    }

    void StopScoliosis()
    {
        ScoliosisModeContainer.style.display = DisplayStyle.None;
    }

    void WobbleText()
    {
        ScoliosisMode.ToggleInClassList("ScoliosisMode-Left");
        ScoliosisTextContainer.ToggleInClassList("ScoliosisContainer--Up");

        ScoliosisMode.RegisterCallback<TransitionEndEvent>(WobbleBack);
    }

    private void WobbleBack(TransitionEndEvent evt)
    {
        ScoliosisMode.ToggleInClassList("ScoliosisMode-Left");
        ScoliosisTextContainer.ToggleInClassList("ScoliosisContainer--Up");
    }
}
