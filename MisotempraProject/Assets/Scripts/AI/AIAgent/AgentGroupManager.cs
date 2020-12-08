using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AI
{
	public class AgentGroupManager : Singleton.DontDestroySingletonMonoBehaviour<AgentGroupManager>
	{
		public ReadOnlyCollection<AgentGroup> groups { get; private set; } = null;
		public ReadOnlyDictionary<string, List<AgentGroup>> groupsKeyName { get; private set; } = null;
		public ReadOnlyDictionary<int, AgentGroup> groupsKeyID { get; private set; } = null;

		/// <summary>Groop list</summary>
		List<AgentGroup> m_groups = new List<AgentGroup>();
		/// <summary>Groop dectionary</summary>
		Dictionary<string, List<AgentGroup>> m_groupsKeyName = new Dictionary<string, List<AgentGroup>>();
		/// <summary>Groop dectionary</summary>
		Dictionary<int, AgentGroup> m_groupsKeyID = new Dictionary<int, AgentGroup>();

		protected override void Init()
		{
			groups = new ReadOnlyCollection<AgentGroup>(m_groups);
			groupsKeyName = new ReadOnlyDictionary<string, List<AgentGroup>>(m_groupsKeyName);
			groupsKeyID = new ReadOnlyDictionary<int, AgentGroup>(m_groupsKeyID);
		}
		protected override void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
		{
			m_groups.Clear();
			m_groupsKeyID.Clear();
			m_groupsKeyName.Clear();
		}
		public void Add(AgentGroup group)
		{
			m_groups.Add(group);
			m_groupsKeyID.Add(group.instanceGroupID, group);

			if (!m_groupsKeyName.ContainsKey(group.gameObject.name))
				m_groupsKeyName.Add(group.gameObject.name, new List<AgentGroup>());
			m_groupsKeyName[group.gameObject.name].Add(group);
		}
		public void Remove(AgentGroup group)
		{
			m_groups.Remove(group);
			m_groupsKeyID.Remove(group.instanceGroupID);

			if (m_groupsKeyName.ContainsKey(group.gameObject.name))
				m_groupsKeyName[group.gameObject.name].Remove(group);
		}
	}
}