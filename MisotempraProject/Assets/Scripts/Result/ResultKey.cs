using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResultKey : MonoBehaviour
{
    [SerializeField, NonEditable]
    protected bool m_isAccept = false;

    public bool isAccept { get { return m_isAccept; } set { m_isAccept = value; } }


    private void Start()
    {
        Init();
        ResultManager.instance.resultKeys.Add(this);
    }

    private void Update()
    {
        if (!isAccept)
        {
            m_isAccept = CheckAccept();
        }
    }

    protected abstract bool CheckAccept();

    protected virtual void Init() { }

    public void Accept()
    {
        m_isAccept = true;
    }
}
