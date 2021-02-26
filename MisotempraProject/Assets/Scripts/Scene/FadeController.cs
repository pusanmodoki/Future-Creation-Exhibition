using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public struct Message
    {
        public enum Type
        {
            None,
            FadeOutEnd,            
            FadeInEnd,
        }

        public Type type { get; }

        public Message(Type t)
        {
            type = t;
        }
    }

    public List<Message> messages { get; private set; } = new List<Message>();

    public bool isActiveFade { get; private set; } = true;

    private Canvas m_fadePanel = null;

    private float m_alpha = 1.0f;

    private float m_fadeTime = 1.0f;

    private Image m_panelImage = null;

    [SerializeField]
    private Canvas m_fadePanelPrefab = null;


    public void Init()
    {
        if(m_fadePanel != null) { return; }
        m_fadePanel = Instantiate(m_fadePanelPrefab);
        m_panelImage = m_fadePanel.transform.GetChild(0).GetComponent<Image>();

        DontDestroyOnLoad(m_fadePanel);
        FadeIn();
    }

    public Message Pop()
    {
        if(messages.Count > 0)
        {
            var result = messages[0];
            messages.RemoveAt(0);
            return result;
        }
        return default;
    }

    public void FadeIn()
    {
        StartCoroutine("FadeInUpdate");
    }

    public void FadeOut(float fadeTime)
    {
        m_fadeTime = fadeTime;
        StartCoroutine("FadeOutUpdate");
    }

    private IEnumerator FadeOutUpdate()
    {
        bool isLoop = true;
        while (isLoop)
        {
            Color color = m_panelImage.color;
            m_alpha += m_fadeTime * Time.fixedDeltaTime;
            if (m_alpha >= 1.0f)
            {
                m_alpha = 1.0f;
                color.a = m_alpha;
                m_panelImage.color = color;
                messages.Add(new Message(Message.Type.FadeOutEnd));
                break;
            }

            color.a = m_alpha;
            m_panelImage.color = color;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private IEnumerator FadeInUpdate()
    {
        bool isLoop = true;
        while (isLoop)
        {
            Color color = m_panelImage.color;

            m_alpha -= m_fadeTime * Time.fixedDeltaTime;
            if(m_alpha <= 0.0f)
            {
                m_alpha = 0.0f;
                color.a = m_alpha;
                m_panelImage.color = color;
                messages.Add(new Message(Message.Type.FadeInEnd));

                m_fadeTime = 0.0f;

                isActiveFade = false;
                break;
            }

            color.a = m_alpha;
            m_panelImage.color = color;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }


}
