using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class ResultManager : Singleton.SingletonMonoBehaviour<ResultManager>
{
    [SerializeField]
    private List<ResultKey> m_resultKeys = new List<ResultKey>();

    [SerializeField]
    private float m_checkTime = 0.05f;

    [SerializeField]
    private GameObject clearProduction = null;

    public List<ResultKey> resultKeys { get { return m_resultKeys; } set { m_resultKeys = value; } }


    protected override void Init()
    {

    }

    private IEnumerator ResultCheck()
    {
        bool isLoop = true;
        while (isLoop)
        {
            for(int i = 0; i < resultKeys.Count; ++i)
            {
                if (isLoop = !ClearCheck()) { }
            }

            yield return new WaitForSeconds(m_checkTime);
        }
    }

    private bool ClearCheck()
    {
        for (int i = 0; i < resultKeys.Count; ++i)
        {
            if (!resultKeys[i].isAccept) return false;
        }
        return true;
    }
}
