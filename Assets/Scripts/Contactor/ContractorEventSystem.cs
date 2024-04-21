using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractorEventSystem : MonoBehaviour
{
    /// <summary>
    /// We use the observer pattern with a singleton observer
    /// </summary>
    public static ContractorEventSystem instance;

    public bool targetKilled;

    private void Awake()
    {
        instance = this;
        targetKilled = false;
    }

    public event Action<GameObject> onPickUpItemTrigger;
    public event Action onPickUpGeneric;
    public void PickUpItemTrigger(GameObject target)
    {
        if (onPickUpItemTrigger != null && onPickUpGeneric != null)
        {
            onPickUpItemTrigger.Invoke(target);
            onPickUpGeneric.Invoke();
        }

    }

    public event Action<GameObject> onPickUpGunTrigger;
    public void EquipGun(GameObject target)
    {
        if (onPickUpGunTrigger != null)
        {
            onPickUpGunTrigger.Invoke(target);
        }

    }

    public event Action onDropItemTrigger;
    public event Action onDropGeneric;
    public void PutBackItemTrigger()
    {
        if (onDropItemTrigger != null)
        {
            onDropItemTrigger.Invoke();
            onDropGeneric.Invoke();
        }

    }

    public void OnlyGenericDropItem()
    {
        if (onDropGeneric != null)
        {
            onDropGeneric.Invoke();
        }
    }

    public event Action onHoverInteractableTrigger;
    public void HoverOverInteractable()
    {
        if (onHoverInteractableTrigger != null)
        {
            onHoverInteractableTrigger.Invoke();
        }

    }

    public event Action onStopHoverTrigger;
    public void StopHovering()
    {
        if (onStopHoverTrigger != null)
        {
            onStopHoverTrigger.Invoke();
        }

    }

    public event Action<GameObject> eatItemTrigger;
    public void EatItem(GameObject target)
    {
        if (eatItemTrigger != null && onPickUpGeneric != null)
        {
            eatItemTrigger.Invoke(target);
        }

    }

    public event Action onRemoveGunTrigger;

    public void RemoveGunFromToilet()
    {
        if (onRemoveGunTrigger != null )
        {
            onRemoveGunTrigger.Invoke();
        }
    }

    public event Action onPlayerRemainStationary;
    public void RemainStationary()
    {
        if (onPlayerRemainStationary != null)
        {
            onPlayerRemainStationary.Invoke();
        }

    }

    public event Action onPlayerBeginMoving;

    public void BeginMoving()
    {
        if (onPlayerBeginMoving != null)
        {
            onPlayerBeginMoving.Invoke();
        }

    }

    public event Action onPlayerTeleportToToilet;
    public event Action onTeleportFinish;

    public void ToiletTeleport()
    {
        if (onPlayerTeleportToToilet != null)
        {
            onPlayerTeleportToToilet.Invoke();
        }

    }

    public event Action onPlayerTeleportToElevator;
    public void ElevatorTeleport()
    {
        if (onPlayerTeleportToToilet != null)
        {
            onPlayerTeleportToElevator.Invoke();
        }
    }

    public event Action onPlayerTeleportToBike;

    public void BikeTeleport()
    {
        if (onPlayerTeleportToBike != null)
        {
            onPlayerTeleportToBike.Invoke();
        }
    }

    public event Action onGunFireTrigger;

    public void GunFired()
    {
        if (onGunFireTrigger != null)
        {
            onGunFireTrigger.Invoke();
        }
    }

    public event Action onDoorKickTrigger;

    public void KickDoor()
    {
        if (onDoorKickTrigger != null)
        {
            onDoorKickTrigger.Invoke();
        }
    }

    public event Action onEnableLeaveTrigger;

    public void EnableLeaving()
    {
        if (onEnableLeaveTrigger != null)
        {
            onEnableLeaveTrigger.Invoke();
        }
    }

    public event Action onKillTarget;

    public void KillTarget()
    {
        targetKilled = true;
        if (onEnableLeaveTrigger != null)
        {
            onEnableLeaveTrigger.Invoke();
        }
        if (onKillTarget != null)
        {
            onKillTarget.Invoke();
        }
    }

    public event Action onExitLevelTrigger;

    public void ExitLevel()
    {
        if (onExitLevelTrigger != null)
        {
            onExitLevelTrigger.Invoke();
        }
    }
}
