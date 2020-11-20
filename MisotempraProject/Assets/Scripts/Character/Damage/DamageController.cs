using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Damage
{
	[DefaultExecutionOrder(-1)]
	public class DamageController : MonoBehaviour
	{
		public DamageSender sender { get { return m_sender; } }
		public DamageReceiver receiver { get { return m_receiver; } }
		public ReadOnlyCollection<AttackInfo> attackInfos { get; private set; } = null;
		public ReadOnlyDictionary<string, AttackInfo> attackInfoDictionary { get; private set; } = null;

		[SerializeField, Tooltip("Attack infos")]
		List<AttackInfo> m_attackInfos = new List<AttackInfo>();
		[SerializeField]
		DamageSender m_sender = new DamageSender();
		[SerializeField]
		DamageReceiver m_receiver = new DamageReceiver();

		Dictionary<string, AttackInfo> m_attackInfoDictionary = new Dictionary<string, AttackInfo>();

		void Awake()
		{
			foreach (var e in m_attackInfos)
			{
				if (!m_attackInfoDictionary.ContainsKey(e.key))
					m_attackInfoDictionary.Add(e.key, e);
				else
				{
#if UNITY_EDITOR
					Debug.LogError(gameObject.name + "<DamageController:Attack infos> keyが重複しています");
#endif
					return;
				}
			}

			m_sender.Awake(gameObject, m_attackInfos, m_attackInfoDictionary);
			m_receiver.Awake(gameObject);
			attackInfos = new ReadOnlyCollection<AttackInfo>(m_attackInfos);
			attackInfoDictionary = new ReadOnlyDictionary<string, AttackInfo>(m_attackInfoDictionary);
		}

		void Update()
		{
			m_sender.Update();
			m_receiver.Update();
		}
	}
}