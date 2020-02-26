using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWindow : MonoBehaviour
{
    private Character target;

    [SerializeField]
    private Image playerIcon;

    [SerializeField]
    private Image mapImage;

    private void Awake()
    {
        TestActorController.Instance.OnPlayerInitialized += Instance_OnPlayerInitialized;
    }

    private void Instance_OnPlayerInitialized(Character player)
    {
        this.target = player;
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(0.3f);
            Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.z, 0);

            Vector3 x = new Vector2(pos.x / 500, pos.y / 500) * mapImage.rectTransform.sizeDelta;
            playerIcon.transform.position = (mapImage.transform.position) + x;
        }
    }
}
