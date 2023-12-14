using UnityEngine.UIElements;
using UnityEngine;


public class ReplyButtonController
{
    public Button replyButton;
    bool replySend = true;

    public void SetVisualElement(VisualElement visualElement)
    {
        replyButton = visualElement.Q<Button>("reply-send-button");
        replyButton.RegisterCallback<ClickEvent>(ButtonClick);
    }

    public void SetButtonLabelText(string text)
    {
        replyButton.text = text;
        if (text == "Send")
        {
            replySend = false;
        }
    }

    public void ButtonClick(ClickEvent evt)
    {
        if (replySend)
        {
            GameEventSystem.instance.replyButtonTrigger();
        }
        else
        {
            GameEventSystem.instance.sendButtonTrigger();
        }
    }
    
}
