using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField, NonEditable]
    private List<ResultKey> m_resultKeys = new List<ResultKey>();

    [SerializeField]
    private float m_checkTime = 0.05f;

    [SerializeField]
    private GameObject m_clearProduction = null;

    public static ResultManager instance { get; private set; }

    public List<ResultKey> resultKeys { get { return m_resultKeys; } set { m_resultKeys = value; } }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError("2ついじょうあるよ");
        }
#endif
    }

    private void OnDestroy()
    {
        if(instance) instance = null;
    }

    private void Update()
    {
        //bool isLoop = true;
        //while (isLoop)
        //{
            bool isClear = ClearCheck();

            if (isClear)
            {
                //isLoop = !isClear;
                m_clearProduction.SetActive(true);
            }

        //    yield return new WaitForSeconds(m_checkTime);
        //}
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
