using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : Singleton<CharactersManager>
{
    public Dictionary<int, Character> characters = new Dictionary<int, Character>();

    [SerializeField]
    private Character characterBase;

    [SerializeField]
    private GameObject[] models;

    private void Awake()
    {
        foreach (var item in FindObjectsOfType<Character>())
        {
            AddCharacter(item.Id, item);
        }
    }

    public bool GetCharacter(int id, out Character c)
    {
        if(characters.ContainsKey(id))
        {
            c = characters[id];
            return true;
        }
        else
        {
            c = null;
            return false;
        }
    }

    public bool AddCharacter(int id, Character c)
    {
        if (characters.ContainsKey(id))
        {
            return false;
        }
        else
        {
            characters.Add(id, c);
            return true;
        }
    }

    public class SpawnData
    {
        public int id;
        public ushort baseId;
        public ushort posX;
        public ushort posZ;
    }

    public void SpawnCharacter(SpawnData data)
    {
        Vector3 pos = new Vector3(data.posX, 1, data.posZ);
        Character actorBase = Instantiate(characterBase, pos, Quaternion.identity);
        actorBase.Id = data.id;
        GameObject actorModelGo = GameObject.Instantiate(models[data.baseId], pos, actorBase.transform.rotation);
        ActorModel actorModel = actorModelGo.GetComponent<ActorModel>();

        if (actorModel)
        {
            actorBase.SetModel(actorModel);
        }

        AddCharacter(data.id, actorBase);
    }

    public void DespawnCharacter(int id)
    {
        if(GetCharacter(id, out Character c))
        {
            Destroy(c.gameObject);
        }

        characters.Remove(id);
    }
}
