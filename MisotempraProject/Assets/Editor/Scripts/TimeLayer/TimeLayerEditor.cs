//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TimeManagement;
using TimeManagement.Detail;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>TimeLayerのドロワーを変更するTimeLayerEditor class</summary>
	[CustomPropertyDrawer(typeof(TimeLayer))]
	public class TimeLayerEditor : PropertyDrawer
	{
		/// <summary>OnGUI</summary>
		/// <param name = "position" ></ param >
		/// < param name="property"></param>
		/// <param name = "label" ></ param >
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			TimeLayerWindow.IfEmptyLoading();

			var guid = property.FindPropertyRelative("m_guid");
			string stringValue = guid.stringValue;
			int nowValue = 0, result;

			if (TimeLayerWindow.layersKeyGuid.ContainsKey(stringValue))
				nowValue = TimeLayerWindow.layerIndexesKeyGuid[stringValue];

			Rect popUpRect = new Rect(
					EditorGUIUtility.labelWidth,
					position.y,
					position.width - EditorGUIUtility.labelWidth,
					EditorGUIUtility.singleLineHeight);

			EditorGUI.LabelField(position, property.displayName);
			result = EditorGUI.Popup(popUpRect, nowValue, TimeLayerWindow.usePropertyLayerNames);
			guid.stringValue = TimeLayerWindow.layers[TimeLayerWindow.saveLayers[result]].guid;
		}
	}
}