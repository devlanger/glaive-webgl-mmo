using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonInventory : MonoBehaviour
{
    public int Slot { get; set; }

    public Image Icon;

    protected virtual void Awake()
    {
        ItemDragHandler dragHandler = GetComponentInChildren<ItemDragHandler>();
        dragHandler.GetComponent<CanvasGroup>().alpha = 0;
    }

    public virtual void Fill(Item item)
    {
        if (item != null)
        {
            Icon.sprite = item.Base.local.GetIcon();
            Icon.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Icon.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public virtual void Click()
    {
        PacketsSender.ItemAction(RecordType.BACKPACK, PacketsSender.ActionType.USE, (ushort)Slot, 0);
    }
}