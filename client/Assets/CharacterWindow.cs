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
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.LVL, (val) =>
        {
            ushort value = (ushort)val;
            lvlText.text = value.ToString();
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.STR, (val) =>
        {
            ushort value = (ushort)val;
            strText.text = value.ToString();
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.DEX, (val) =>
        {
            ushort value = (ushort)val;
            dexText.text = value.ToString();
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.VIT, (val) =>
        {
            ushort value = (ushort)val;
            vitText.text = value.ToString();
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.INT, (val) =>
        {
            ushort value = (ushort)val;
            intText.text = value.ToString();
        });
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.STATPOINTS, (val) =>
        {
            ushort value = (ushort)val;
            pointsText.text = value.ToString();

            if(value > 0)
            {
                ActivateButtons();
            }
            else
            {
                DeactivateButtons();
            }
        });
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
