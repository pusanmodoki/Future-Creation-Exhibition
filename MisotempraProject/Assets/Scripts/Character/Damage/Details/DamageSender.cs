using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Damage
{
	[System.Serializable]
	public class DamageSender 
	{
		static int m_idCounter = 0;

		public GameObject gameObject { get; private set; } = null;
		public Transform transform { get; private set; } = null;

		/// <summary>ヒットするLayerMask</summary>
		[SerializeField, Tooltip("ヒットするLayerMask")]
		LayerMaskEx m_hitLayers = int.MaxValue;
		[SerializeField, Tooltip("除外タグ")]
		List<TagEx> m_excludeTags = new List<TagEx>();

		List<AttackInfo> m_attackInfos = null;
		Dictionary<string, AttackInfo> m_attackInfoDictionary = null;
		Dictionary<string, float> m_disableSeconds = new Dictionary<string, float>();

		public void EnableAction(string key, float scale = 1.0f)
		{
			if (m_attackInfoDictionary.ContainsKey(key) && m_attackInfoDictionary[key].id < 0)
			{
				m_attackInfoDictionary[key].SetID(++m_idCounter);
				m_attackInfoDictionary[key].SetScale(scale);
			}
		}
		public void DisableAction(string key)
		{
			if (m_attackInfoDictionary.ContainsKey(key))
				m_attackInfoDictionary[key].SetID(-1);
		}
		public void DisableActionDelay(string key, float delaySeconds)
		{
			if (m_attackInfoDictionary.ContainsKey(key) && m_attackInfoDictionary[key].id >= 0)
			{
				if (!m_disableSeconds.ContainsKey(key))
					m_disableSeconds.Add(key, 0.0f);
				m_disableSeconds[key] = delaySeconds;
			}
		}
		public void DisableActionAll()
		{
			m_disableSeconds.Clear();
			foreach (var e in m_attackInfos)
				e.SetID(-1);
		}

		public void DoRequest(DamageReceiver receiver, string key)
		{
			if (m_attackInfoDictionary.ContainsKey(key) && m_attackInfoDictionary[key].id >= 0
				&& !m_excludeTags.Contains(receiver.gameObject.tag)
				&& m_hitLayers.EqualBitsForGameObject(receiver.gameObject))
			{
				var info = m_attackInfoDictionary[key];
				receiver.Request(transform.root.gameObject, info);
			}
		}

		public virtual void Awake(GameObject gameObject, List<AttackInfo> attackInfos, 
			Dictionary<string, AttackInfo> attackInfoDictionary)
		{
			this.gameObject = gameObject;
			this.transform = gameObject.transform;
			m_attackInfos = attackInfos;
			m_attackInfoDictionary = attackInfoDictionary;
		}

		public virtual void Update()
		{
			if (m_disableSeconds.Count == 0) return;

			var keys = m_disableSeconds.Keys;
			foreach (var key in keys)
			{
				m_disableSeconds[key] -= Time.deltaTime;
				if (m_disableSeconds[key] <= 0.0f)
				{
					m_attackInfoDictionary[key].SetID(-1);
					m_disableSeconds.Remove(key);
				}
			}
		}
	}
}