using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	namespace TypeName
	{
		public class TypeNameDetailWindow : EditorWindow
		{
			TypeNameEditor m_editor = null;

			public void Initialize(TypeNameEditor editor)
			{
				m_editor = editor;
			}

			void Awake()
			{
				position = new Rect(new Vector2(-10, -10), new Vector2(10, 10));
				wantsMouseMove = true;
				wantsMouseEnterLeaveWindow = true;
			}
			void OnGUI()
			{
				var searchWindowProvider = UnityEngine.ScriptableObject.CreateInstance<TypeNameProvider>();
				searchWindowProvider.Initialize(m_editor);
				SearchWindow.Open(new SearchWindowContext(Event.current.mousePosition),
					searchWindowProvider);
				Close();
			}
		}
	}
}