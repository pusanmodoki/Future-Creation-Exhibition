using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class EffectDictionary : MonoBehaviour
    {
        [SerializeField]
        private List<EffectBase> m_effects = new List<EffectBase>();

        public Dictionary<string, EffectBase> effectDictionary { get; private set; } = new Dictionary<string, EffectBase>();


        // Start is called before the first frame update
        void Start()
        {
            foreach (var effect in m_effects)
            {
                Debug.Log(effect);
                effectDictionary.Add(effect.effectName, effect);
            }
        }

        public void PlayEffect(string name)
        {
            // ProcessingLoad.ProcessingLoadManager.instance.AddProcessingGauge(10.0f);
            effectDictionary[name].OnEffect();
        }

        public void StopEffect(string name)
        {
            effectDictionary[name].OffEffect();
        }

        public bool IsPlaying(string name)
        {
            return effectDictionary[name].isPlaying;
        }

        public void PlayEffectOnWorld(string name)
        {
            effectDictionary[name].OnEffect();
            effectDictionary[name].transform.SetParent(null);
        }
    }

}
