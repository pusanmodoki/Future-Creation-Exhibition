//----------------------------------------------------------------
// 制作者：綾野
// メモ  ：オブジェクトがスーって消えるシェーダです。
//         
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField]
    private float AddThreshold = 0.01f; // 加算するしきい値
    private Material material = null;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_Threshold", material.GetFloat("_Threshold") + AddThreshold);
    }
}
