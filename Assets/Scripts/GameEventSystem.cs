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

    public event Action onDayBegin;
    public void DayBeginTrigger()
    {
        if (onDayBegin != null)
        {
            onDayBegin.Invoke();
        }

    }

}
