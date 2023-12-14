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

}
