using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultKey : MonoBehaviour
{
    [SerializeField]
    private bool m_isAccept = false;

    public bool isAccept { get { return m_isAccept; } set { m_isAccept = value; } }


    private void Start()
    {
        ResultManager.instance.resultKeys.Add(this);
    }

    public void Accept()
    {
        m_isAccept = true;
    }
}
