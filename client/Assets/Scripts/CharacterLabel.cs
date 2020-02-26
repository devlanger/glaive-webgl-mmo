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
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position + (Vector3.up * 2));
        transform.position = pos;
    }

    private void OnDestroy()
    {
        GameCore.Stats.UnregisterChange(target.Id, ObjectStats.HP, OnHealthChanged);
    }

    public void Fill(Character arg2)
    {
        target = arg2;
        nameText.text = arg2.name;
        GameCore.Stats.RegisterChange(target.Id, ObjectStats.HP, OnHealthChanged);
    }

    private void OnHealthChanged(object v)
    {
        int health = (int)v;
        int maxHealth = GameCore.Stats.GetProperty<int>(target.Id, ObjectStats.MAX_HP);
        fill.fillAmount = (float)health / (float)maxHealth;
    }
}
