using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseDecorator
		{
			public static ReadOnlyDictionary<string, string> jsonData = null;
			static Dictionary<string, string> m_jsonData = new Dictionary<string, string>();

			public System.Type thisType { get; private set; } = null;
			public string guid { get; private set; } = null;
			public bool isEnableQuitDuringRun { get { return m_isEnableQuitDuringRun; } }

			[SerializeField, Tooltip("true: 実行中でもDecoratorがfalseになるとQuitする")]
			bool m_isEnableQuitDuringRun = false;

			public abstract bool IsPredicate(AIAgent agent, Blackboard blackboard);
			public virtual void OnCreate() { }

			public void LoadBase(CashContainer.Detail.DecoratorInfomations infomations, System.Type thisType)
			{
				this.thisType = thisType;
				this.guid = infomations.guid;

				if (jsonData == null) jsonData = new ReadOnlyDictionary<string, string>(m_jsonData);
				m_jsonData.Add(this.guid, infomations.jsonData);
			}
			public void CloneBase(BaseDecorator decorator)
			{
				thisType = decorator.thisType;
			}
		}
	}
}