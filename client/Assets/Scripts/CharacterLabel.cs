using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLabel : MonoBehaviour
{
    [SerializeField]
    private Character target;

    [SerializeField]
    private Image fill;

    private void Start()
    {
        GameCore.Stats.RegisterChange(target.Id, ObjectStats.HP, (v) =>
        {
            short health = (short)v;
            if (health <= 0)
            {
                Destroy(gameObject);
                return;
            }
            fill.fillAmount = (float)health / 100f;
        });
    }

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position + (Vector3.up * 2));
        transform.position = pos;
    }
}
