using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GameCoreEngine
{
    public class HitBehaviour : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private HitPlayableBehaviour.Data data;

        ClipCaps ITimelineClipAsset.clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var p = ScriptPlayable<HitPlayableBehaviour>.Create(graph);
            var behaviour = p.GetBehaviour();
            behaviour.owner = owner;
            behaviour.data = data;

            return p;
        }
    }

    [System.Serializable]
    public class HitPlayableBehaviour : ActorPlayableBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public float radius;
            public Type type;
            public EffectPlayableBehaviour hitEffect;

            public enum Type
            {
                SINGLE_TARGET = 1,
                AOE = 2,
            }
        }


        public Data data;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);

            if (Application.isPlaying)
            {
                data.hitEffect.owner = owner;
                switch (data.type)
                {
                    case Data.Type.AOE:
                        foreach (var item in Physics.OverlapSphere(owner.transform.position, data.radius))
                        {
                            Actor a = item.GetComponent<Actor>();
                            if (a != null)
                            {
                                data.hitEffect.SpawnOnTarget(a.transform);
                            }
                        }
                        break;
                    case Data.Type.SINGLE_TARGET:

                        break;
                }
            }
        }
    }
}