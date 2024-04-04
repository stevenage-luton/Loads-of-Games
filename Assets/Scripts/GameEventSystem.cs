using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem instance;
    private void Awake()
    {
        instance = this;
    }
    public event Action onReplyButtonTrigger;
    public void replyButtonTrigger()
    {
        if (onReplyButtonTrigger != null)
        {
            onReplyButtonTrigger.Invoke();
        }

    }

    public event Action onSendButtonTrigger;
    public void sendButtonTrigger()
    {
        if (onSendButtonTrigger != null)
        {
            onSendButtonTrigger.Invoke();
        }

    }

    public event Action onReadyToSendTrigger;
    public void ReadyToSendTrigger()
    {
        if (onReadyToSendTrigger != null)
        {
            onReadyToSendTrigger.Invoke();
        }

    }

    public event Action onComputerInteract;
    public void ComputerInteractTrigger()
    {
        if (onComputerInteract != null)
        {
            onComputerInteract.Invoke();
        }

    }

    public event Action onEndInteract;
    public void EndInteractTrigger()
    {
        if (onEndInteract != null)
        {
            onEndInteract.Invoke();
        }

    }

    public event Action onDayEnd;
    public void DayEndTrigger()
    {
        if (onDayEnd != null)
        {
            onDayEnd.Invoke();
        }

    }

    public event Action<int> onDayBegin;
    public void DayBeginTrigger(int day)
    {
        if (onDayBegin != null)
        {
            onDayBegin.Invoke(day);
        }

    }

    public event Action<EmailData> midDayEmailRecieve;

    public void RecieveEmail(EmailData data)
    {
        if (midDayEmailRecieve != null)
        {
            midDayEmailRecieve.Invoke(data);
        }

    }

    public event Action<EmailData> awaitNextEmailInChain;

    public void WaitForEmailReply(EmailData data)
    {
        if (awaitNextEmailInChain != null)
        {
            awaitNextEmailInChain.Invoke(data);
        }

    }

}
