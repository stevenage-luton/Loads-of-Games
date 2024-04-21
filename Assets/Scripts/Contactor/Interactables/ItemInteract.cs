using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : Interactable
{
    public ItemData itemDataSO;
    public override void OnInteract()
    {
        ContractorEventSystem.instance.PickUpItemTrigger(this.gameObject);
    }

    public override void OnDrop()
    {
        ContractorEventSystem.instance.PutBackItemTrigger();
    }

}
