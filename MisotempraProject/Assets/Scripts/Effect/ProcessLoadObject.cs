using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessLoadObject : MonoBehaviour
{
    [SerializeField]
    float m_addGauge = 5.0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        ProcessingLoad.ProcessingLoadManager.instance.AddProcessingGauge(m_addGauge);
    }
}
