//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// LayerMaskを拡張したLayerMaskEx class
/// </summary>
[System.Serializable]
public struct LayerMaskEx
{
    /// <summary>
    /// LayerMask getter
    /// </summary>
    public LayerMask layerMask { get { return m_layerMask; } private set { m_layerMask = value; } }
    /// <summary>
    /// Layer Bit Mask Getter
    /// </summary>
    public int value { get { return layerMask.value; } }

    [SerializeField, Tooltip("LayerMask")]
    LayerMask m_layerMask;

    /// <summary>
    /// [コンストラクタ]
    /// 引数1: レイヤーマスク
    /// </summary>
    public LayerMaskEx(int layerMask)
    {
        m_layerMask = layerMask;
    }
    /// <summary>
    /// [コンストラクタ]
    /// 引数1: レイヤーマスク
    /// </summary>
    public LayerMaskEx(LayerMask layerMask)
    {
        m_layerMask = layerMask;
    }
    /// <summary>
    /// [SetValue]
    /// 引数1: レイヤーマスク
    /// </summary>
    public void SetValue(int layerMask)
    {
        m_layerMask = layerMask;
    }
    /// <summary>
    /// [SetValue]
    /// 引数1: レイヤーマスク
    /// </summary>
    public void SetValue(LayerMask layer)
    {
        m_layerMask = layerMask;
    }

    /// <summary>
    /// [EqualBitsForGameObject]
    /// return: equal bits = true
    /// 引数1: レイヤーマスク
    /// </summary>
    public bool EqualBitsForGameObject(GameObject gameObject)
    {
        return (value & 1 << gameObject.layer) != 0;
    }

    /// <summary>
    /// operator to int
    /// </summary>
    public static implicit operator int(LayerMaskEx mask)
    {
        return mask.value;
    }
    /// <summary>
    /// operator to LayerMaskEx
    /// </summary>
    public static implicit operator LayerMaskEx(int mask)
    {
        return new LayerMaskEx(mask);
    }
    /// <summary>
    /// operator to LayerMask
    /// </summary>
    public static implicit operator LayerMask(LayerMaskEx mask)
    {
        return mask.layerMask;
    }
    /// <summary>
    /// operator to LayerMaskEx
    /// </summary>
    public static implicit operator LayerMaskEx(LayerMask mask)
    {
        return new LayerMaskEx(mask);
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LayerMaskEx))]
public class LayerMaskExDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_layerMask"), label);
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PropertyField(position, property.FindPropertyRelative("m_layerMask"), label);
	}
}
#endif