using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uiDocument;

    private VisualElement root;
    private VisualElement textContainerMain;

    private VisualElement contractorContainer;
    private VisualElement emailGameContainer;

    private List<VisualElement> emptyContainers;

    Label nameText, jobText, objectiveText, charText;

    Button contractorButton, emailButton, m_acceptButton;

    [SerializeField]
    private MenuGameData contractorData, emailGameData;

    bool onMenu;

    private void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }

    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        root = _uiDocument.rootVisualElement;

        textContainerMain = root.Q<VisualElement>("Text");

        emptyContainers = root.Query("EmptyIcon").ToList();

        contractorButton = root.Q<Button>("ContractorButton");

        emailButton = root.Q<Button>("EmailGameButton");

        m_acceptButton = root.Q<Button>("AcceptButton");

        contractorContainer = contractorButton.parent;
        emailGameContainer = emailButton.parent;

        nameText = root.Q<Label>("GameText");
        jobText = root.Q<Label>("OccupationText");
        objectiveText = root.Q<Label>("ObjectiveText");
        charText = root.Q<Label>("NameText");

        contractorButton.clickable.clicked += () => GenericGameButtonClick(contractorData, contractorContainer);

        emailButton.clickable.clicked += () => GenericGameButtonClick(emailGameData, emailGameContainer);

        onMenu = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (onMenu)
            {
                onMenu = false;
                m_acceptButton.UnregisterCallback<ClickEvent, string>(LoadScene);
                foreach (VisualElement emptyBox in emptyContainers)
                {
                    emptyBox.RemoveFromClassList("button-container--out");
                    emptyBox.RemoveFromClassList("button-container--in");
                }
                textContainerMain.AddToClassList("text--out");
            }
            else
            {
                Application.Quit();
            }
        }
    }

    private void GenericGameButtonClick(MenuGameData data, VisualElement buttonElement)
    {
        nameText.text = '"' + data.gameName + '"';
        jobText.text = data.job;
        objectiveText.text = data.objective;
        charText.text = data.charName;
        onMenu = true;

        m_acceptButton.UnregisterCallback<ClickEvent, string>(LoadScene);

        foreach (VisualElement emptyBox in emptyContainers)
        {
            emptyBox.AddToClassList("button-container--out");
        }

        buttonElement.RemoveFromClassList("button-container--out");
        buttonElement.AddToClassList("button-container--in");
        textContainerMain.RemoveFromClassList("text--out");

        m_acceptButton.RegisterCallback<ClickEvent, string>(LoadScene,data.sceneName);
    }

    void LoadScene(ClickEvent evt, string sceneName)
    {

        SceneManager.LoadScene(sceneName);

        m_acceptButton.UnregisterCallback<ClickEvent, string>(LoadScene);
    }
}
