using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class TrailEffect : EffectBase
    {
        public TrailRenderer trailRenderer { get; private set; }

        public override bool isPlaying
        {
            get
            {
                return trailRenderer.emitting;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if(!(trailRenderer = GetComponent<TrailRenderer>()))
            {
                trailRenderer = gameObject.AddComponent<TrailRenderer>();
            }
        }

        public override void OnEffect()
        {
            trailRenderer.emitting = true;
        }

        public override void OffEffect()
        {
            trailRenderer.emitting = false;
        }
    }
}
