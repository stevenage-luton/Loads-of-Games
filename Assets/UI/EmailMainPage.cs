using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmailMainPage : MonoBehaviour
{

    private UIDocument document;

    [SerializeField]
    private Camera mainCamera;

    private Button inbox;
    private Button sent;
    private Button drafts;
    private Button deleted;
    private VisualElement root;
    private VisualElement emailcontainer;
    private VisualElement buttonContainer;

    AudioSource audioPlayer;

    public AudioClip keyPress;
    public AudioClip mouseClick;

    public bool OnReplyScreen = false;
    private bool readyToSend = false;

    public EmailData currentEmail;

    private Label replyEmailTextLabel;

    private const string currentlySelectedTabClassName = "selected";
    private const string tabClassName = "tab";

    //umxl template for inbox entries
    [SerializeField]
    VisualTreeAsset m_ListEntryTemplate;
    [SerializeField]
    VisualTreeAsset m_BodyEntryTemplate;
    [SerializeField]
    VisualTreeAsset m_ReplySendButton;
    //the ListView
    ListView emaillist;

    List<EmailData> inboxEmails = new List<EmailData>();
    List<EmailData> sentEmails = new List<EmailData>();

    private void Start()
    {
        

        root = document.rootVisualElement;     

        emaillist = root.Q<ListView>("emaillist");
        emailcontainer = root.Q<VisualElement>("emailcontainer");
        buttonContainer = emailcontainer.Q<VisualElement>("replybuttoncontainer");

        inbox = root.Q<Button>("inbox");
        inbox.AddToClassList("unselected");
        inbox.AddToClassList("tab");
        sent = root.Q<Button>("sent");
        sent.AddToClassList("unselected");
        sent.AddToClassList("tab");
        drafts = root.Q<Button>("drafts");
        drafts.AddToClassList("unselected");
        drafts.AddToClassList("tab");
        deleted = root.Q<Button>("deleted");
        deleted.AddToClassList("unselected");
        deleted.AddToClassList("tab");

        inbox.RegisterCallback<ClickEvent>(InboxClicked);
        sent.RegisterCallback<ClickEvent>(SentClicked);

        InitializeEmailList(root,m_ListEntryTemplate);

        RegisterTabCallbacks();

        GameEventSystem.instance.onReplyButtonTrigger += ReplyButtonWasClicked;
        GameEventSystem.instance.onSendButtonTrigger += SendButtonWasClicked;
        GameEventSystem.instance.onReadyToSendTrigger += UpdateReadyToSend;

        audioPlayer = GetComponent<AudioSource>();


    }

    private void OnEnable()
    {
        document = GetComponent<UIDocument>();

        document.panelSettings.SetScreenToPanelSpaceFunction((Vector2 screenPos) => {
            var invalidPosition = new Vector2(float.NaN, float.NaN);

            var cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            Debug.Log(cameraRay);

            Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100, Color.magenta);

            RaycastHit UIhit;
            if (!Physics.Raycast(cameraRay, out UIhit, 100f, LayerMask.GetMask("ScreenUI")))
            {
                Debug.Log($"Invalid Position");
                Debug.Log(UIhit.collider);
                return invalidPosition;
            }

            Vector2 uvForPixel = UIhit.textureCoord;

            uvForPixel.y = 1 - uvForPixel.y;
            uvForPixel.x *= this.document.panelSettings.targetTexture.width;
            uvForPixel.y *= this.document.panelSettings.targetTexture.height;

            return uvForPixel;
        });
    }

    public void InboxClicked(ClickEvent evt)
    {
        Debug.Log("inbox clicked!");
        //inbox.AddToClassList("selected");
        //sent.RemoveFromClassList("selected");
        FillEmailList(inboxEmails);
        emaillist.Rebuild();
    }
    public void SentClicked(ClickEvent evt)
    {
        Debug.Log("sent clicked!");
        //sent.AddToClassList("selected");
        //inbox.RemoveFromClassList("selected");
        FillEmailList(sentEmails);
        emaillist.Rebuild();
    }

    private void RegisterTabCallbacks()
    {
        UQueryBuilder<Button> tabs = GetAllTabs();
        tabs.ForEach((Button tab) => {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });
    }
    private UQueryBuilder<Button> GetAllTabs()
    {
        return root.Query<Button>(className: tabClassName);
    }

    private void TabOnClick(ClickEvent evt)
    {
        Button clickedTab = evt.currentTarget as Button;
        if (!TabIsCurrentlySelected(clickedTab))
        {
            GetAllTabs().Where(
                (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
            ).ForEach(UnselectTab);
            SelectTab(clickedTab);
            emailcontainer.Clear();
            audioPlayer.PlayOneShot(mouseClick);
            OnReplyScreen = false;
            readyToSend = false;
        }
    }

    private static bool TabIsCurrentlySelected(Button tab)
    {
        return tab.ClassListContains(currentlySelectedTabClassName);
    }
    private void UnselectTab(Button tab)
    {
        tab.RemoveFromClassList("selected");
        tab.AddToClassList("unselected");
    }
    private void SelectTab(Button tab)
    {
        tab.RemoveFromClassList("unselected");
        tab.AddToClassList("selected");
    }

    void EnumerateAllEmails()
    {
        inboxEmails = new List<EmailData>();
        sentEmails = new List<EmailData>();

        inboxEmails.AddRange(Resources.LoadAll<EmailData>("Emails/Day1/Inbox"));
        sentEmails.AddRange(Resources.LoadAll<EmailData>("Emails/Day1/Sent"));
    }

    void FillEmailList(List<EmailData> listtoLoad)
    {


        emaillist.Clear();
        emaillist.RefreshItems();
        emaillist.itemsSource = listtoLoad;
        emaillist.Rebuild();

        // Set up a make item function for a list entry
        emaillist.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new EmailListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);


            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        emaillist.bindItem = (item, index) =>
        {
            if (index < listtoLoad.Count)
            {
                (item.userData as EmailListEntryController)?.SetEmailData(listtoLoad[index]);
                (item.userData as EmailListEntryController)?.EmailButton.RegisterCallback<ClickEvent, EmailData>(EntryClicked, listtoLoad[index]);

            }
        };


        //// Set a fixed item height
        emaillist.fixedItemHeight = 100;

        //emaillist.itemsSource = listtoLoad;
        
    }

    void InitializeEmailList(VisualElement root, VisualTreeAsset entryTemplate)
    {
        EnumerateAllEmails();

        // Store a reference to the template for the list entries
        m_ListEntryTemplate = entryTemplate;



        

        // Register to get a callback when an item is selected
        //emaillist.selectionChanged += OnCharacterSelected;
    }

    void EntryClicked(ClickEvent evt, EmailData data)
    {
        OnReplyScreen = false;
        readyToSend = false;
        currentEmail = null;
        emailcontainer.Clear();

        var emailBody = m_BodyEntryTemplate.Instantiate();
        var newBodyEntryLogic = new EmailBodyTemplateController();

        emailBody.userData = newBodyEntryLogic;
        newBodyEntryLogic.SetVisualElement(emailBody);
        newBodyEntryLogic.SetBodyData(data);
        emailcontainer.Add(emailBody);

        currentEmail = data;


        if (TabIsCurrentlySelected(inbox) && data.hasReply)
        {
            buttonContainer = emailcontainer.Q<VisualElement>("replybuttoncontainer");
            var replyButton = m_ReplySendButton.Instantiate();
            var replyButtonLogic = new ReplyButtonController();

            replyButton.userData = replyButtonLogic;

            replyButtonLogic.SetVisualElement(replyButton);

            replyButtonLogic.SetButtonLabelText("Reply");

            //replyButtonLogic.replyButton.RegisterCallback<ClickEvent>(replyButtonLogic.ButtonClick);

            buttonContainer.Add(replyButton);
        }

        audioPlayer.PlayOneShot(mouseClick);
    }

    void ReplyButtonWasClicked()
    {
        EmailData inboxData = currentEmail;
        OnReplyScreen = true;
        readyToSend = false;
        if (inboxData.reply == null)
        {
            Debug.Log("EMAIL HAD NO REPLY OBJECT. SOMETHING IS BROKEN");
            return;
        }

        emailcontainer.Clear();

        EmailData replyData = currentEmail.reply;


        var replyBody = m_BodyEntryTemplate.Instantiate();
        var replyBodyEntryLogic = new EmailBodyTemplateController();

        replyBody.userData = replyBodyEntryLogic;
        replyBodyEntryLogic.SetVisualElement(replyBody);
        replyBodyEntryLogic.SetReplyData(replyData);
        emailcontainer.Add(replyBody);
        replyEmailTextLabel = emailcontainer.Q<Label>("text");


        if (TabIsCurrentlySelected(inbox))
        {
            buttonContainer = emailcontainer.Q<VisualElement>("replybuttoncontainer");
            var sendButton = m_ReplySendButton.Instantiate();
            var sendButtonLogic = new ReplyButtonController();

            sendButton.userData = sendButtonLogic;

            sendButtonLogic.SetVisualElement(sendButton);

            sendButtonLogic.SetButtonLabelText("Send");

            //replyButtonLogic.replyButton.RegisterCallback<ClickEvent>(replyButtonLogic.ButtonClick);

            buttonContainer.Add(sendButton);
        }
        audioPlayer.PlayOneShot(mouseClick);
        Debug.Log("Reply Button Was Clicked!");
    }

    void SendButtonWasClicked()
    {
        if (readyToSend)
        {
            emailcontainer.Clear();
            //currentEmail.hasReply = false;
            sentEmails.Insert(0, currentEmail.reply);
            Debug.Log("Send Button Was Clicked!");
            currentEmail = null;
            audioPlayer.PlayOneShot(mouseClick);
            OnReplyScreen = false;
            readyToSend = false;
        }
        
    }

    public void AddLetterToReply(char letter)
    {
        audioPlayer.PlayOneShot(keyPress);
        replyEmailTextLabel.text += letter;  
    }

    void UpdateReadyToSend()
    {
        readyToSend = true;
    }
}
