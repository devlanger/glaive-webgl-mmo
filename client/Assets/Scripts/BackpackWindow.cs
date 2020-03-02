using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackpackWindow : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Text goldText;

    [SerializeField]
    private ButtonInventory button;

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private ushort slots = 64;

    [SerializeField]
    private Dictionary<int, ButtonInventory> buttons = new Dictionary<int, ButtonInventory>();

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform window = transform as RectTransform;

        if(!RectTransformUtility.RectangleContainsScreenPoint(window, Input.mousePosition))
        {
            Debug.Log("Drop item");
            ButtonInventory dragButton = eventData.pointerDrag.GetComponentInParent<ButtonInventory>();
            PacketsSender.ItemAction(RecordType.BACKPACK, PacketsSender.ActionType.DELETE, (ushort)dragButton.Slot, 0);
        }
        else
        {
            ButtonInventory dragButton = eventData.pointerDrag.GetComponentInParent<ButtonInventory>();
            ButtonInventory enterButton = eventData.pointerEnter.GetComponentInParent<ButtonInventory>();
            if(dragButton != null && enterButton != null)
            {
                Debug.Log(dragButton.Slot + " enters: " + enterButton.Slot);
                PacketsSender.ItemAction(RecordType.BACKPACK, PacketsSender.ActionType.MOVE, (ushort)dragButton.Slot, (ushort)enterButton.Slot);
            }
        }
    }

    private void Awake()
    {
        TestActorController.Instance.OnPlayerInitialized += Instance_OnPlayerInitialized;

        StartCoroutine(Initialize());
    }

    private void Instance_OnPlayerInitialized(Character actor)
    {
        StatChanged(ObjectStats.GOLD, GameCore.Stats.GetProperty<uint>(actor.Id, ObjectStats.GOLD));

        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.GOLD, (val) =>
        {
            StatChanged(ObjectStats.GOLD, val);
        });
    }
    private void StatChanged(ObjectStats stat, object value)
    {
        if (value == null)
        {
            return;
        }

        switch (stat)
        {
            case ObjectStats.GOLD:
                goldText.text = value.ToString();
                break;
        }
    }

    private IEnumerator Initialize()
    {
        for (int i = 0; i < slots; i++)
        {
            ButtonInventory btInst = ButtonInventory.Instantiate(button, parent);
            int slot = i;
            btInst.Slot = slot;

            buttons.Add(slot, btInst);
        }

        Inventory.Instance.backpack.OnRecordChanged += Backpack_OnRecordChanged;
        Inventory.Instance.backpack.OnRecordRemoved += Backpack_OnRecordRemoved;

        yield return new WaitForEndOfFrame();

        parent.GetComponent<GridLayoutGroup>().enabled = false;
    }

    private void Backpack_OnRecordRemoved(int slot)
    {
        buttons[slot].Fill(null);
    }

    private void Backpack_OnRecordChanged(int slot, Item item)
    {
        buttons[slot].Fill(item);
    }
}
