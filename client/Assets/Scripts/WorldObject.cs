using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public int Id = 0;

    public ActorModel model;

    public virtual void SetModel(ushort actorModelId)
    {
        GameObject actorModelGo = GameObject.Instantiate(CharactersManager.Instance.models[actorModelId], transform.position, transform.rotation);
        ActorModel actorModel = actorModelGo.GetComponent<ActorModel>();

        if (actorModel)
        {
            this.model = actorModel;
            model.transform.parent = transform;
        }
    }
}
