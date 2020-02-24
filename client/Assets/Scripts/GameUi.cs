using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField]
    private Actor target;

    [SerializeField]
    private Text coordsText;

    private Vector3 lastPos;

    private void FixedUpdate()
    {
        Vector3 pos = target.GetPosition();
        if (pos != lastPos)
        {
            coordsText.text = string.Format("{0}x {1}y", pos.x, pos.z);
            lastPos = pos;
        }
    }
}
