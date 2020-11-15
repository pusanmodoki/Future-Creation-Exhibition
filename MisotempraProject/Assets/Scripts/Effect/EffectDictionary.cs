using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDictionary : MonoBehaviour
{

    [System.Serializable]
    public struct Effect
    {
        public ParticleSystem m_particle;
        public string m_name;
    }

    [SerializeField]
    private List<Effect> m_effects = new List<Effect>();

    public Dictionary<string, ParticleSystem> effectDictionary { get; } = new Dictionary<string, ParticleSystem>();

    public ParticleSystem this[string name]
    {
        get
        {
            return effectDictionary[name];
        }
    }

    public readonly struct Message
    {
        public enum Command
        {
            Play,
            Stop
        }

        private string effectName { get; }

        private Command command { get; }
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach(var effect in m_effects)
        {
            effectDictionary.Add(effect.m_name, effect.m_particle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffect(string name)
    {
        ProcessingLoadManager.instance.AddProcessingGauge(10.0f);
        effectDictionary[name].Play();
    }

    public void StopEffect(string name)
    {
        effectDictionary[name].Stop();
    }

    public bool IsPlaying(string name)
    {
        return effectDictionary[name].isPlaying;
    }

    public void PlayEffectOnWorld(string name)
    {
        effectDictionary[name].Play();
        effectDictionary[name].transform.SetParent(null);
    }
}
