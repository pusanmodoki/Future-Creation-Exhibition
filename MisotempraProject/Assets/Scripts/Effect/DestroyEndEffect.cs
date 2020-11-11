using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEndEffect : MonoBehaviour
{
    [SerializeField]
    private float m_destroyInterval = 1.0f;

    [SerializeField]
    private ParticleSystem m_particle = null;

    void Start()
    {
        StartCoroutine("DestroyCheck");
    }

    private IEnumerator DestroyCheck()
    {
        while (true)
        {
            if (!m_particle.isPlaying)
            {
                GameObject.Destroy(gameObject);
            }
            yield return new WaitForSeconds(m_destroyInterval);
        }
    }
}
