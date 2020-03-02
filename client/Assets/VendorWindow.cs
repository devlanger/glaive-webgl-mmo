using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : MonoBehaviour
{
    public class ShopItem
    {
        public int itemId;
        public int price;
        public int amount;
    }

    [SerializeField]
    private ButtonVendor button;

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private ushort slots = 64;

    [SerializeField]
    private Dictionary<int, ButtonVendor> buttons = new Dictionary<int, ButtonVendor>();

    public int shopCharacterId;

    private void Awake()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        for (int i = 0; i < slots; i++)
        {
            ButtonVendor btInst = ButtonVendor.Instantiate(button, parent);
            int slot = i;
            btInst.Slot = slot;

            buttons.Add(slot, btInst);
        }

        yield return new WaitForEndOfFrame();

        parent.GetComponent<GridLayoutGroup>().enabled = false;
    }

    public void Fill(int shopCharacterId, Dictionary<ushort, ShopItem> shop)
    {
        this.shopCharacterId = shopCharacterId;

        foreach (var item in shop)
        {
            buttons[item.Key].Fill(item.Value);
        }
    }
}
