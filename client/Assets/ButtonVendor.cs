using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVendor : ButtonInventory
{
    public void Fill(VendorWindow.ShopItem item)
    {
        ItemBase itemBase = ItemsManager.Instance.GetItem(item.itemId);

        if (itemBase != null)
        {
            Icon.sprite = itemBase.local.GetIcon();
            Icon.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Icon.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public override void Click()
    {
        PacketsSender.BuyVendorItem(FindObjectOfType<VendorWindow>().shopCharacterId, (ushort)Slot);
    }
}
