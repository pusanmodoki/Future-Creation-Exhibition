using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResultKey : MonoBehaviour
{
    [SerializeField]
    protected bool m_isAccept = false;

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
