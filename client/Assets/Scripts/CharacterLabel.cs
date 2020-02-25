using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLabel : MonoBehaviour
{
    private Character target;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Image fill;

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position + (Vector3.up * 2));
        transform.position = pos;
    }

    public void Fill(Character arg2)
    {
        target = arg2;
        nameText.text = arg2.name;
        GameCore.Stats.RegisterChange(target.Id, ObjectStats.HP, (v) =>
        {
            int health = (int)v;
            if (health <= 0)
            {
                Destroy(gameObject);
                return;
            }
            fill.fillAmount = (float)health / 100f;
        });
    }
}
