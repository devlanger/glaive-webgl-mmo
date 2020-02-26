using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLabels : MonoBehaviour
{
    [SerializeField]
    private CharacterLabel label;

    private Dictionary<int, CharacterLabel> labels = new Dictionary<int, CharacterLabel>();

    void Awake()
    {
        CharactersManager.Instance.OnCharacterAdded += Instance_OnCharacterAdded;
        CharactersManager.Instance.OnCharacterRemoved += Instance_OnCharacterRemoved;
    }

    private void Instance_OnCharacterRemoved(int id)
    {
        if (labels.ContainsKey(id))
        {
            if (labels[id].gameObject != null)
            {
                Destroy(labels[id].gameObject);
            }
            labels.Remove(id);
        }
    }

    private void Instance_OnCharacterAdded(int arg1, GameCoreEngine.Character arg2)
    {
        if(arg1 == TestActorController.Instance.Actor.Id)
        {
            return;
        }

        CharacterLabel labelInst = CharacterLabel.Instantiate(label, transform);
        labelInst.Fill(arg2);
        labels.Add(arg2.Id, labelInst);
    }
}
