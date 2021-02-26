using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectTrigger : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_effectPrefab = null;

    [SerializeField]
    private bool playOnAwake = true;

    private float checkIntervalSeconds { get; } = 1.0f;

    public bool isEnableTrigger { get; private set; } = true;

    public List<ParticleSystem> instancedEffectObject { get; set; } = new List<ParticleSystem>();

    private void Awake()
    {
        StartCoroutine("ParticleEndCheck");
    }

    private void OnEnable()
    {
        if (playOnAwake)
        {
            isEnableTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnableTrigger)
        {
            instancedEffectObject.Add(Instantiate(m_effectPrefab, transform.position, Quaternion.identity));
        }
    }

    private IEnumerator ParticleEndCheck()
    {
        bool isLoop = true;

        while (isLoop)
        {
            for (int i = 0; i < instancedEffectObject.Count; ++i)
            {
                if (!instancedEffectObject[i].isPlaying)
                {
                    GameObject.Destroy(instancedEffectObject[i].gameObject);
                    instancedEffectObject[i] = null;
                }
            }
            for (int i = 0; i < instancedEffectObject.Count; ++i)
            {
                if(instancedEffectObject[i] == null)
                {
                    instancedEffectObject.RemoveAt(i);
                    --i;
                }
            }

            yield return new WaitForSeconds(checkIntervalSeconds);
        }
    }

    public void OnEffectTrigger()
    {
        isEnableTrigger = true;
    }
    public void OffEffectTrigger()
    {
        isEnableTrigger = false;
    }

    private void OnDestroy()
    {
        StopCoroutine("ParticleEndCheck");
    }
}
