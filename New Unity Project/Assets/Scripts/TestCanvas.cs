using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestCanvas : MonoBehaviour
{
    [SerializeField]
    private int m_testCount = 100000;

    [SerializeField]
    private GameObject m_prefab = null;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < m_testCount; ++i)
        {
            GameObject obj = Instantiate(m_prefab, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
