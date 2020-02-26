using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    private Actor target;

    [SerializeField]
    private Text coordsText;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Image manaBar;

    [SerializeField]
    private Image expBar;

    private Vector3 lastPos;

    private void Awake()
    {
        TestActorController.Instance.OnPlayerInitialized += Instance_OnPlayerInitialized;
        //Instance_OnPlayerInitialized((Character)TestActorController.Instance.Actor); 
    }

    private void Instance_OnPlayerInitialized(Character actor)
    {
        target = actor;
        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.EXPERIENCE, (val) =>
        {
            uint experience = (uint)val;
            uint maxExperience = GameCore.Stats.GetProperty<uint>(actor.Id, ObjectStats.MAX_EXPERIENCE);
            expBar.fillAmount = (float)((float)experience / (float)maxExperience);
        });

        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.HP, (val) =>
        {
            int health = (int)val;
            int maxHealth = GameCore.Stats.GetProperty<int>(actor.Id, ObjectStats.MAX_HP);
            healthBar.fillAmount = (float)((float)health / (float)maxHealth);
        });

        GameCore.Stats.RegisterChange(actor.Id, ObjectStats.MANA, (val) =>
        {
            uint mana = (uint)val;
            uint maxMana = GameCore.Stats.GetProperty<uint>(actor.Id, ObjectStats.MAX_MANA);
            manaBar.fillAmount = (float)((float)mana / (float)maxMana);
        });

    }

    private void FixedUpdate()
    {
        if(target == null)
        {
            return;
        }

        Vector3 pos = target.GetPosition();
        if (pos != lastPos)
        {
            coordsText.text = string.Format("{0}x {1}y", pos.x, pos.z);
            lastPos = pos;
        }
    }
}
