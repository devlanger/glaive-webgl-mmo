using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : MonoBehaviour
{
    [SerializeField]
    private Text lvlText;

    [SerializeField]
    private Text vitText;
    [SerializeField]
    private Text strText;
    [SerializeField]
    private Text dexText;
    [SerializeField]
    private Text intText;
    [SerializeField]
    private Text pointsText;

    [SerializeField]
    private Button vitButton;
    [SerializeField]
    private Button strButton;
    [SerializeField]
    private Button dexButton;
    [SerializeField]
    private Button intButton;

    private void Awake()
    {
        DeactivateButtons();

        TestActorController.Instance.OnPlayerInitialized += Instance_OnPlayerInitialized;
        vitButton.onClick.AddListener(() => { AddStatClick(ObjectStats.VIT); });
        strButton.onClick.AddListener(() => { AddStatClick(ObjectStats.STR); });
        dexButton.onClick.AddListener(() => { AddStatClick(ObjectStats.DEX); });
        intButton.onClick.AddListener(() => { AddStatClick(ObjectStats.INT); });
    }

    private void AddStatClick(ObjectStats stat)
    {
        PacketsSender.AddStat(stat);
    }

    private void Instance_OnPlayerInitialized(Character actor)
    {
        StatChanged(ObjectStats.LVL, GameCore.Stats.GetProperty<object>(actor.Id, ObjectStats.LVL));
        StatChanged(ObjectStats.VIT, GameCore.Stats.GetProperty<object>(actor.Id, ObjectStats.VIT));
        StatChanged(ObjectStats.STR, GameCore.Stats.GetProperty<object>(actor.Id, ObjectStats.STR));
        StatChanged(ObjectStats.DEX, GameCore.Stats.GetProperty<object>(actor.Id, ObjectStats.DEX));
        StatChanged(ObjectStats.INT, GameCore.Stats.GetProperty<object>(actor.Id, ObjectStats.INT));
        StatChanged(ObjectStats.STATPOINTS, GameCore.Stats.GetProperty<object>(actor.Id, ObjectStats.STATPOINTS));

        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.LVL, (val) =>
        {
            StatChanged(ObjectStats.LVL, val);
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.STR, (val) =>
        {
            StatChanged(ObjectStats.STR, val);
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.DEX, (val) =>
        {
            StatChanged(ObjectStats.DEX, val);
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.VIT, (val) =>
        {
            StatChanged(ObjectStats.VIT, val);
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.INT, (val) =>
        {
            StatChanged(ObjectStats.INT, val);
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.STATPOINTS, (val) =>
        {
            StatChanged(ObjectStats.STATPOINTS, val);
        });
    }

    private void StatChanged(ObjectStats stat, object value)
    {
        if(value == null)
        {
            return;
        }
        switch(stat)
        {
            case ObjectStats.LVL:
                lvlText.text = value.ToString();
                break;
            case ObjectStats.STR:
                strText.text = value.ToString();
                break;
            case ObjectStats.DEX:
                dexText.text = value.ToString();
                break;
            case ObjectStats.VIT:
                vitText.text = value.ToString();
                break;
            case ObjectStats.INT:
                intText.text = value.ToString();
                break;
            case ObjectStats.STATPOINTS:
                pointsText.text = value.ToString();

                if ((ushort)value > 0)
                {
                    ActivateButtons();
                }
                else
                {
                    DeactivateButtons();
                }
                break;
        }
    }

    private void ActivateButtons()
    {
        vitButton.gameObject.SetActive(true);
        strButton.gameObject.SetActive(true);
        dexButton.gameObject.SetActive(true);
        intButton.gameObject.SetActive(true);
    }

    private void DeactivateButtons()
    {
        vitButton.gameObject.SetActive(false);
        strButton.gameObject.SetActive(false);
        dexButton.gameObject.SetActive(false);
        intButton.gameObject.SetActive(false);
    }
}
