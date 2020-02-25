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

    private void Instance_OnCharacterRemoved(int obj)
    {
        if (labels[obj].gameObject != null)
        {
            Destroy(labels[obj].gameObject);
        }
        labels.Remove(obj);
    }

    private void Instance_OnCharacterAdded(int arg1, GameCoreEngine.Character arg2)
    {
        CharacterLabel labelInst = CharacterLabel.Instantiate(label, transform);
        labelInst.Fill(arg2);
        labels.Add(arg1, labelInst);
    }
}
