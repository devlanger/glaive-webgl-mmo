using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Actor actor;

    public void SetTarget(Actor target)
    {
        this.actor = target;
        camera.GetComponent<IsometricCameraController>().SetTarget(target);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray r = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r, out RaycastHit hit))
            {
                if(hit.collider != null)
                {
                    Actor a = hit.collider.GetComponent<Actor>();
                    if(a != null)
                    {
                        if (a == actor)
                        {

                        }
                        else
                        {
                            actor.Move(a.transform.position - Vector3.left);
                        }
                    }
                    else
                    {
                        actor.Move(hit.point);
                    }
                }
            }
        }
    }
}
