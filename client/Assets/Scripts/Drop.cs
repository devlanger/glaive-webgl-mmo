using System.Collections;
using System.Collections.Generic;
using GameCoreEngine;
using UnityEngine;

public class Drop : WorldObject
{
    public override void SetModel(ushort actorModelId)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = Vector3.one * 0.3f;
        go.GetComponent<Collider>().isTrigger = true;
        go.transform.position = transform.position;
        go.transform.parent = transform;
    }
}
