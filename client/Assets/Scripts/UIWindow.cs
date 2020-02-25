using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    private Canvas canvas;

    public bool IsHidden
    {
        get
        {
            if (canvas == null)
            {
                return gameObject.activeInHierarchy;
            }
            else
            {
                return canvas.enabled;
            }
        }
    }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Hide()
    {
        if (canvas == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            canvas.enabled = false;
        }
    }

    public void Show()
    {
        if (canvas == null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            canvas.enabled = true;
        }
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
