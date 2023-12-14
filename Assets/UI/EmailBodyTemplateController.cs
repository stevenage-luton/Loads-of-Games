using UnityEngine.UIElements;
using UnityEngine;

public class EmailBodyTemplateController
{
    Label SubjectLabel;
    Label FromLabel;
    Label FromAddress;
    Label ToLabel;
    Label TextLabel;
    Label ReplyText;
    public VisualElement rootElement;

    //This function retrieves a reference to the 
    //character name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        SubjectLabel = visualElement.Q<Label>("subject");
        FromLabel = visualElement.Q<Label>("fromlabel");
        FromAddress = visualElement.Q<Label>("fromaddress");
        ToLabel = visualElement.Q<Label>("tolabel");
        TextLabel = visualElement.Q<Label>("text");
        ReplyText = visualElement.Q<Label>("replytext");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetBodyData(EmailData emailData)
    {
        SubjectLabel.text = emailData.Subject;
        FromLabel.text = "From: " + emailData.EmailUserName;
        FromAddress.text = "<" + emailData.EmailAddress + ">";
        ToLabel.text = "To: " + emailData.RecipientAddress;
        TextLabel.text = emailData.EmailBody;
    }

    public void SetReplyData(EmailData replyData)
    {
        SubjectLabel.text = replyData.Subject;
        FromLabel.text = "From: Clyde Thunderpants";
        FromAddress.text = "<clydethunderpants@midway.admin.net>";
        ToLabel.text = "To: " + replyData.RecipientAddress;
        TextLabel.text = "";
        ReplyText.text = replyData.EmailBody;
    }
}
