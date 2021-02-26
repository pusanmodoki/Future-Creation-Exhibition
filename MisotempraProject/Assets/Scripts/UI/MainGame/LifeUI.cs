using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    private int displayLifeCount { get; set; } = 0;

    private Player.PlayerController player { get; set; } = null;

    private List<RectTransform> icons { get; set; } = new List<RectTransform>();

    [SerializeField]
    private RectTransform m_iconPrefab = null;

    [SerializeField]
    private float m_interval = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.PlayerController.instance;
        CountUpdate();

        InitIcons();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(displayLifeCount != player.armor.stock)
        {
            CountUpdate();
        }
    }

    private void InitIcons()
    {
        displayLifeCount = player.armor.maxStock;

        for(int i = 0; i < displayLifeCount; ++i)
        {
            icons.Add(Instantiate(m_iconPrefab, transform));

            Vector2 vec = Vector2.zero;

            vec.x = -((m_interval + icons[i].sizeDelta.x) * (((float)(displayLifeCount - 1)) / 2.0f)) + i * (icons[i].sizeDelta.x + m_interval);
            icons[i].anchoredPosition = vec;
        }
    }

    private void CountUpdate()
    {
        displayLifeCount = player.armor.stock;
        for(int i = displayLifeCount; i < player.armor.maxStock; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void AddLifeIcon(LifeIcon icon)
    {
        
    }
}
