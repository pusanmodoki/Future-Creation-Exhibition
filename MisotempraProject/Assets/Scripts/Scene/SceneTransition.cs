using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class SceneTransition:Singleton.SingletonMonoBehaviour<SceneTransition>
{
    private string m_nextSceneName = "";

    [SerializeField]
    private FadeController m_fadeController = null;

    public static FadeController fadeController { get; private set; } = null;

    protected override void Init()
    {
        if(fadeController != null) { return; }

        fadeController = Instantiate(m_fadeController);
        DontDestroyOnLoad(fadeController);
        fadeController.Init();
    }

    public void LoadScene(string sceneName, float fadeTime)
    {
        fadeController.FadeOut(fadeTime);
        m_nextSceneName = sceneName;
        StartCoroutine("WaitFadeOut");
    }

    private IEnumerator WaitFadeOut()
    {
        while (true)
        {
            if(fadeController.Pop().type == FadeController.Message.Type.FadeOutEnd)
            {
                SceneManager.LoadScene(m_nextSceneName);
                m_nextSceneName = "";
                fadeController.FadeIn();
                break;
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
