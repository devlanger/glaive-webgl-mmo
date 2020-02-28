using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : Singleton<CharactersManager>
{
    public Dictionary<int, WorldObject> characters = new Dictionary<int, WorldObject>();

    [SerializeField]
    private GameObject characterBase;

    public GameObject[] models;

    public event Action<int, WorldObject> OnCharacterAdded = delegate { };
    public event Action<int> OnCharacterRemoved = delegate { };

    private void Awake()
    {
        foreach (var item in FindObjectsOfType<Character>())
        {
            AddCharacter(item.Id, item);
        }
    }

    public bool GetCharacter<T>(int id, out T c) where T : WorldObject
    {
        if(characters.ContainsKey(id))
        {
            c = characters[id] as T;
            return true;
        }
        else
        {
            c = null;
            return false;
        }
    }

    public bool AddCharacter(int id, WorldObject c)
    {
        if (characters.ContainsKey(id))
        {
            return false;
        }
        else
        {
            characters.Add(id, c);
            OnCharacterAdded(id, c);
            return true;
        }
    }

    public class SpawnData
    {
        public SpawnType type;
        public int id;
        public string name;
        public int health;
        public int maxHealth;
        public ushort baseId;
        public ushort posX;
        public ushort posZ;

        public enum SpawnType
        {
            CHARACTER = 1,
            DROP = 2
        }
    }

    public void SpawnCharacter<T>(SpawnData data) where T : WorldObject
    {
        Vector3 pos = new Vector3(data.posX, 0.5f, data.posZ);

        GameObject actorBaseGameObject = Instantiate(characterBase.gameObject, pos, Quaternion.identity);
        T actorBase = actorBaseGameObject.AddComponent<T>();
        actorBase.Id = data.id;
        actorBase.name = data.name;
        GameCore.Stats.SetProperty<int>(data.id, ObjectStats.HP, data.health);
        GameCore.Stats.SetProperty<int>(data.id, ObjectStats.MAX_HP, data.maxHealth);

        actorBase.SetModel(data.baseId);
        AddCharacter(data.id, actorBase);
    }

    public void DespawnCharacter(int id)
    {
        if(GetCharacter(id, out WorldObject c))
        {
            Destroy(c.gameObject);
        }

        characters.Remove(id);
        OnCharacterRemoved(id);
    }
}
