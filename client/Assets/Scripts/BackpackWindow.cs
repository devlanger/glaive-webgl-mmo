using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackWindow : MonoBehaviour
{
    [SerializeField]
    private ButtonInventory button;

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private ushort slots = 64;

    private void Awake()
    {
        for (int i = 0; i < slots; i++)
        {
            ButtonInventory btInst = ButtonInventory.Instantiate(button, parent);
        }
    }
}
