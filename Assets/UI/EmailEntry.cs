using UnityEngine.UIElements;

public class EmailListEntryController
{
    Label SenderLabel;
    Label SubjectLabel;
    Label TimeLabel;
    VisualElement Picture;
    public Button EmailButton;

    //This function retrieves a reference to the 
    //character name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        SenderLabel = visualElement.Q<Label>("sender");
        SubjectLabel = visualElement.Q<Label>("subject");
        Picture = visualElement.Q<VisualElement>("pictureoverlay");
        TimeLabel = visualElement.Q<Label>("time");
        EmailButton = visualElement.Q<Button>("emailcontainer");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetEmailData(EmailData emailData)
    {
        SenderLabel.text = emailData.EmailUser.EmailUserName;
        SubjectLabel.text = emailData.Subject;
        Picture.style.backgroundImage = new StyleBackground(emailData.EmailUser.PortraitImage);
        TimeLabel.text = emailData.time;
    }
}