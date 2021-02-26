using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimationStart : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("FadeInEndCheck");
    }

    IEnumerator FadeInEndCheck()
    {
        while (true)
        {
            if(SceneTransition.fadeController.Pop().type == FadeController.Message.Type.FadeInEnd)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
