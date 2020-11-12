using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseService
		{
			public static ReadOnlyDictionary<string, string> jsonData = null;
			static Dictionary<string, string> m_jsonData = new Dictionary<string, string>();

			public System.Type thisType { get; private set; } = null;
			public string guid { get; private set; } = null;
			public float callInterval { get; private set; } = 0.0f;

			Timer m_timer = new Timer();

			public abstract void ServiceFunction(AIAgent agent, Blackboard blackboard);

			public void LoadBase(CashContainer.Detail.ServiceInfomations infomations, System.Type thisType)
			{
				this.thisType = thisType;
				this.guid = infomations.guid;
				this.callInterval = infomations.callInterval;

				if (jsonData == null) jsonData = new ReadOnlyDictionary<string, string>(m_jsonData);
				if (!m_jsonData.ContainsKey(guid)) m_jsonData.Add(guid, infomations.jsonData);
			}
			public void CloneBase(BaseService service)
			{
				thisType = service.thisType;
				callInterval = service.callInterval;
			}

			public void OnEnable() { m_timer.Start(); }
			public void Update(AIAgent agent, Blackboard blackboard)
			{
				if (m_timer.elapasedTime >= callInterval)
				{
					m_timer.Start();
					ServiceFunction(agent, blackboard);
				}
			}
		}
	}
}