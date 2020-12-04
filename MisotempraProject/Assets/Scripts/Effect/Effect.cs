using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> m_particles = new List<ParticleSystem>();

    public bool isPlaying
    {
        get
        {
            foreach(var particle in m_particles)
            {
                if (particle.isPlaying)
                {
                    return true;
                }
            }
            return false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
