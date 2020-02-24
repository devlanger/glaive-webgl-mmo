using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GameCoreEngine
{
    public class SpawnEffectBehaviour : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private EffectPlayableBehaviour.Data data;

        ClipCaps ITimelineClipAsset.clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var p = ScriptPlayable<EffectPlayableBehaviour>.Create(graph);
            var behaviour = p.GetBehaviour();
            behaviour.owner = owner;
            behaviour.data = data;

            return p;
        }
    }

    [System.Serializable]
    public class EffectPlayableBehaviour : ActorPlayableBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public GameObject effect;
            public Vector3 offset;
        }

        public Data data;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);

            if (Application.isPlaying)
            {
                GameObject eff = GameObject.Instantiate(data.effect, owner.transform.position + (owner.transform.rotation * data.offset), owner.transform.rotation);
            }
        }

        public void SpawnOnTarget(Transform target)
        {
            GameObject eff = GameObject.Instantiate(data.effect, target.transform.position + (target.transform.rotation * data.offset), owner.transform.rotation);
        }
    }
}