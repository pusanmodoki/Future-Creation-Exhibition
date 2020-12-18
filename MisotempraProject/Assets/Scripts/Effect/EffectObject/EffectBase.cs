using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    abstract public class EffectBase : MonoBehaviour
    {
        public string effectName { get { return m_effectName; } }

        public Transform owner { get; private set; } = null;

        abstract public bool isPlaying { get; }

        abstract public void OnEffect();
        abstract public void OffEffect();

        [SerializeField]
        private string m_effectName = "Effect";

        private void Awake()
        {
            if (transform.parent)
            {
                owner = transform.parent;
            }
        }
    }
}
