using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	namespace TypeName
	{
		public class TypeNameProvider : ScriptableObject, ISearchWindowProvider
		{
			class NameSpaceContainer
			{
				public NameSpaceContainer(string thisName)
				{
					this.thisName = thisName;
					childrens = new Dictionary<string, NameSpaceContainer>();
					classes = new List<ClassName>();
				}
				public string thisName;
				public Dictionary<string, NameSpaceContainer> childrens;
				public List<ClassName> classes;
			}
			struct ClassName
			{
				public ClassName(string name, string fullName)
				{
					this.name = name;
					this.fullName = fullName;
				}
				public string name;
				public string fullName;
			}

			static Dictionary<string, NameSpaceContainer> m_containers = null;
			static HashSet<string> m_globalClasses = null;

			TypeNameEditor m_editor = null;

			public void Initialize(TypeNameEditor editor)
			{
				m_editor = editor;
			}

			List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
			{
				var entries = new List<SearchTreeEntry>();

				if (m_containers == null)
					CreateContainers();

				entries.Add(new SearchTreeGroupEntry(new GUIContent("Select Classes")));

				entries.Add(new SearchTreeGroupEntry(new GUIContent("Global")) { level = 1 });
				foreach (var name in m_globalClasses)
					entries.Add(new SearchTreeEntry(new GUIContent(name, name)) { level = 2 });

				foreach (var container in m_containers)
					AddEntrys(entries, container.Value, 1);

				return entries;
			}

			bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
			{
				m_editor.ChangeName(SearchTreeEntry.content.tooltip);
				return true;
			}

			void CreateContainers()
			{
				var types = TypeExtension.GetAllTypes();

				m_containers = new Dictionary<string, NameSpaceContainer>();
				m_globalClasses = new HashSet<string>();

				foreach (var type in types)
				{
					string[] splits = type.FullName.Split('.');

					if (splits.Length == 1)
					{
						m_globalClasses.Add(splits[0]);
						continue;
					}

					NameSpaceContainer container = null;
					if (m_containers.ContainsKey(splits[0]))
						container = m_containers[splits[0]];
					else
					{
						container = new NameSpaceContainer(splits[0]);
						m_containers.Add(splits[0], container);
					}

					for (int i = 1; i < splits.Length - 1; ++i)
					{
						if (!container.childrens.ContainsKey(splits[i]))
							container.childrens.Add(splits[i], new NameSpaceContainer(splits[i]));

						container = container.childrens[splits[i]];
					}

					container.classes.Add(new ClassName(type.Name, type.FullName));
				}
			}

			void AddEntrys(List<SearchTreeEntry> entries, NameSpaceContainer container, int level)
			{
				entries.Add(new SearchTreeGroupEntry(new GUIContent(container.thisName)) { level = level });

				foreach (var children in container.childrens)
					AddEntrys(entries, children.Value, level + 1);

				foreach (var info in container.classes)
					entries.Add(new SearchTreeEntry(new GUIContent(info.name, info.fullName)) { level = level + 1 });
			}
		}
	}
}