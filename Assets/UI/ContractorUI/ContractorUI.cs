using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ContractorUI : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uiDocument;

    private VisualElement root;

    private VisualElement crosshair;

    private VisualElement screenContainer;

    private Label cutsceneLabel;

    [SerializeField]
    EndingCutsceneData VictoryCutscene;

    [SerializeField]
    EndingCutsceneData LossCutscene;

    [SerializeField]
    private EndingCutsceneData m_currentCutscene;

    int cutsceneIncrement;

    void Start()
    {
        root = _uiDocument.rootVisualElement;

        crosshair = root.Q<VisualElement>("Crosshair");

        screenContainer = root.Q<VisualElement>("ScreenContainer");

        cutsceneLabel = root.Q<Label>("TextBar");

        ContractorEventSystem.instance.onHoverInteractableTrigger += TurnToHand;
        ContractorEventSystem.instance.onStopHoverTrigger += TurnToSquare;
        ContractorEventSystem.instance.onKillTarget += LoadVictoryCutscene;
        ContractorEventSystem.instance.onPickUpGeneric += DisableCrosshair;
        ContractorEventSystem.instance.onDropGeneric += EnableCrosshair;
        ContractorEventSystem.instance.onPickUpGunTrigger += EnableCrosshair;
        ContractorEventSystem.instance.onExitLevelTrigger += FadeInCutScene;

        m_currentCutscene = LossCutscene;

        cutsceneLabel.text = "";

        cutsceneIncrement = 0;
    }

    void DisableCrosshair()
    {
        crosshair.style.visibility = Visibility.Hidden;
    }

    void EnableCrosshair()
    {
        crosshair.style.visibility = Visibility.Visible;
    }

    void EnableCrosshair(GameObject gun)
    {
        crosshair.style.visibility = Visibility.Visible;
    }

    void TurnToHand()
    {
        crosshair.AddToClassList("Crosshair--hand");
    }

    void TurnToSquare()
    {
        crosshair.RemoveFromClassList("Crosshair--hand");
    }

    void LoadVictoryCutscene()
    {
        Debug.Log("Loading Victory WINe");
        m_currentCutscene = VictoryCutscene;
    }

    void FadeInCutScene()
    {
        DisableCrosshair();
        screenContainer.RemoveFromClassList("ScreenContainer--clear");
        StartCoroutine(EndingCutscene());
    }

    IEnumerator EndingCutscene()
    {
        if (cutsceneIncrement == 0)
        {
            yield return new WaitForSeconds(4.5f);
        }

        cutsceneLabel.text = m_currentCutscene.cutscene[cutsceneIncrement].Line;
        cutsceneLabel.AddToClassList("textbar--in");

        yield return new WaitForSeconds(m_currentCutscene.cutscene[cutsceneIncrement].fullDuration);

        cutsceneLabel.RemoveFromClassList("textbar--in");

        yield return new WaitForSeconds(2.5f);

        cutsceneIncrement++;

        if (cutsceneIncrement < m_currentCutscene.cutscene.Count)
        {
            StartCoroutine(EndingCutscene());
        }
    }
}
