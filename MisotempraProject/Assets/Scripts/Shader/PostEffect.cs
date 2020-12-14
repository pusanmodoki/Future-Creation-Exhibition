//----------------------------------------------------------------
// 制作者　：綾野
// アタッチ：カメラ 
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffect : MonoBehaviour
{

    [SerializeField]
    private Material _material = default;

    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        Graphics.Blit(source, dest, _material);
    }
}