using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Damage
{
	namespace Detail
	{
		public abstract class BaseRequester : MonoBehaviour
		{
			public string attackKey { get { return m_attackKey; } }
			public DamageController controller { get { return m_controller; } }
			public AttackInfo attackInfo { get; private set; } = null;

			[SerializeField, Tooltip("This contoroller")]
			DamageController m_controller = null;
			[SerializeField]
			string m_attackKey = "";

			void Awake()
			{
				if (!m_controller.attackInfoDictionary.ContainsKey(m_attackKey))
				{
#if UNITY_EDITOR
					Debug.LogError(gameObject.name + "<Requester:Attack infos> keyが見つかりません");
#endif
					return;
				}
				else
					attackInfo = m_controller.attackInfoDictionary[m_attackKey];
			}
		}
	}
}