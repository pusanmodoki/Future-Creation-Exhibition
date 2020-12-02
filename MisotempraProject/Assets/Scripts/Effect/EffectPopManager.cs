using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPopManager : Singleton.SingletonMonoBehaviour<EffectPopManager>
{
    [SerializeField]
    private List<GameObject> m_effects = new List<GameObject>();

    private Queue<GameObject> m_instancedEffects = new Queue<GameObject>();

    public Dictionary<string, GameObject> m_dictionary { get; private set; } = new Dictionary<string, GameObject>();


    protected override void Init() { }


    private void Start()
    {
        foreach(var obj in m_effects)
        {
            m_dictionary.Add(obj.name, obj);
        }

    }

    private void Update()
    {
    }

    public void PopEffect(in string name, in Vector3 pos)
    {
        m_instancedEffects.Enqueue(GameObject.Instantiate(m_dictionary[name], pos, Quaternion.identity));        
    }
}
