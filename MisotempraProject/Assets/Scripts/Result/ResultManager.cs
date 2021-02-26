using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField, NonEditable]
    private List<ResultKey> m_resultKeys = new List<ResultKey>();


    [SerializeField]
    private GameObject m_clearProduction = null;

    [SerializeField]
    private GameObject m_gameOverProduction = null;

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
        bool isClear = ClearCheck();
        if (isClear)
        {
            Player.PlayerController.instance.isControll = false;
            m_clearProduction.SetActive(true);
        }
        bool isGameOver = GameOverCheck();
        if (isGameOver)
        {
            Player.PlayerController.instance.isControll = false;
            m_gameOverProduction.SetActive(true);
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

    private bool GameOverCheck()
    {
        if (Player.PlayerController.instance.armor.stock <= 0)
        {
            return true;
        }
        return false;
    }
}
