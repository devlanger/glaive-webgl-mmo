using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackWindow : MonoBehaviour, IDropHandler
{
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
        }
        else
        {
            ButtonInventory dragButton = eventData.pointerDrag.GetComponentInParent<ButtonInventory>();
            ButtonInventory enterButton = eventData.pointerEnter.GetComponentInParent<ButtonInventory>();
            if(dragButton != null && enterButton != null)
            {
                Debug.Log(dragButton.Slot + " enters: " + enterButton.Slot);
            }
        }
    }

    private void Awake()
    {
        for (int i = 0; i < slots; i++)
        {
            ButtonInventory btInst = ButtonInventory.Instantiate(button, parent);
            int slot = i;
            btInst.Slot = slot;

            buttons.Add(slot, btInst);
        }

        Inventory.Instance.backpack.OnRecordChanged += Backpack_OnRecordChanged;
    }

    private void Backpack_OnRecordChanged(int slot, Item item)
    {
        buttons[slot].Fill(item);
    }
}
