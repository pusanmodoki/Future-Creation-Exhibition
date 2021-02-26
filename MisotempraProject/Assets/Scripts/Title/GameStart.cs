using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    private string m_nextSceneName = "";

    [SerializeField]
    private float m_fadeTime = 1.0f;
    public void StageStart()
    {
        SceneTransition.instance.LoadScene(m_nextSceneName, m_fadeTime);
    }
}
