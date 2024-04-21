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

    public event Action onSpineModeButton;
    public void SpineModeTrigger()
    {
        if (onSpineModeButton != null)
        {
            onSpineModeButton.Invoke();
        }

    }

    public event Action onConfirmSpineMode;

    public void ConfirmSpineModeButton()
    {
        if (onEndSpineModeButton != null)
        {
            onEndSpineModeButton.Invoke();
        }
        if (onConfirmSpineMode != null)
        {
            onConfirmSpineMode.Invoke();
        }
    }

    public event Action onEndSpineModeButton;
    public void EndSpineModeTrigger()
    {
        if (onEndSpineModeButton != null)
        {
            onEndSpineModeButton.Invoke();
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
    public event Action onGenericDayStart;
    public event Action<int> onDayBegin;
    public void DayBeginTrigger(int day)
    {
        if (onDayBegin != null)
        {
            onDayBegin.Invoke(day);
            onGenericDayStart.Invoke();
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

    public event Action<int> updateArmPosition;

    public void ChangeArmTrigger(int val)
    {
        if (updateArmPosition != null)
        {
            updateArmPosition.Invoke(val);
        }

    }

    public event Action<int> updateTorsoPosition;

    public void ChangeTorsoTrigger(int val)
    {
        if (updateTorsoPosition != null)
        {
            updateTorsoPosition.Invoke(val);
        }

    }

    public event Action<int> updateLegPosition;

    public void ChangeLegTrigger(int val)
    {
        if (updateLegPosition != null)
        {
            updateLegPosition.Invoke(val);
        }

    }

    public event Action<ComboData> onUpdateComboAnimation;

    public void UpdateComboAnimation(ComboData data)
    {
        if (onUpdateComboAnimation != null)
        {
            onUpdateComboAnimation.Invoke(data);
        }

    }

    public event Action onUpdateComboUI;

    public void UpdateComboUI()
    {
        if (onUpdateComboUI != null)
        {
            onUpdateComboUI.Invoke();
        }
    }

    public event Action onRecieveSpineReadySignal;

    public void SignalToggleSpineUpdateReady()
    {
        if (onRecieveSpineReadySignal != null)
        {
            onRecieveSpineReadySignal.Invoke();
        }
    }

    public event Action onBeginScoliosisMode;

    public void BeginScoliosisMode()
    {
        if (onBeginScoliosisMode != null)
        {
            onBeginScoliosisMode.Invoke();
        }
    }

    public event Action onEndScoliosisMode;

    public void EndScoliosisMode()
    {
        if (onEndScoliosisMode != null)
        {
            onEndScoliosisMode.Invoke();
        }
    }

}
