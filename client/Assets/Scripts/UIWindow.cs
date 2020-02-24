using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public bool IsHidden
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Toggle()
    {
        if(!IsHidden)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
