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

    private void Awake()
    {
        ItemDragHandler dragHandler = GetComponentInChildren<ItemDragHandler>();
        dragHandler.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Fill(Item item)
    {
        Icon.sprite = item.Base.local.GetIcon();
        Icon.GetComponent<CanvasGroup>().alpha = 1;
    }
}