using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSceneTrans : MonoBehaviour
{
    [SerializeField]
    float interval = 1.0f;

    [SerializeField]
    string sceneName = "";

    bool isInput = false;



    void Start()
    {
        StartCoroutine("Interval");
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManagement.GameInput.GetButtonDown("Fire1") && isInput)
        {
            SceneTransition.instance.LoadScene(sceneName, 1.0f);
        }
    }

    IEnumerator Interval()
    {
        yield return new WaitForSeconds(interval);

        isInput = true;
    }
}
