////作成者 : 植村将太
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Damage
//{
//	/// <summary>
//	/// Colliderに当たっていた場合ダメージをリクエストするDamageRequesterWithTrigger
//	/// </summary>
//	public class DamageRequesterWithTrigger : MonoBehaviour
//	{
//		/// <summary>ヒットするLayerMask</summary>
//		public LayerMaskEx hitLayers { get { return m_hitLayers; } set { m_hitLayers = value; } }
//		/// <summary>吹っ飛びタイプ</summary>
//		public BlowAwayType blowAwayType { get { return m_blowAwayType; } set { m_blowAwayType = value; } }
//		/// <summary>攻撃力</summary>
//		public float attack { get { return m_attack; } set { m_attack = value; } }

//		/// <summary>ヒットするLayerMask</summary>
//		[SerializeField, Tooltip("ヒットするLayerMask")]
//		LayerMaskEx m_hitLayers = int.MaxValue;
//		/// <summary>吹っ飛びタイプ</summary>
//		[SerializeField, Tooltip("吹っ飛びタイプ")]
//		BlowAwayType m_blowAwayType = BlowAwayType.Null;
//		/// <summary>攻撃力</summary>
//		[SerializeField, Tooltip("攻撃力")]
//		float m_attack = 0.0f;
//		/// <summary>一度当たったら除外します</summary>
//		[SerializeField, Tooltip("一度当たったら除外します")]
//		bool m_isExclude = true;

//		/// <summary>HitしたオブジェクトのInstanceID</summary>
//		List<int> m_instanceIDs = new List<int>();

//		/// <summary>
//		/// [ResetExclude]
//		/// 除外リストをクリアする
//		/// </summary>
//		public void ResetExclude()
//		{
//			m_instanceIDs.Clear();
//		}

//		void OnTriggerEnter(Collider other)
//		{
//			//ヒットしたオブジェクトが該当レイヤー & 除外リスト該当なし
//			if (m_hitLayers.EqualBitsForGameObject(other.gameObject) && !m_instanceIDs.Contains(other.gameObject.transform.GetInstanceID()))
//			{
//				//除外設定がある場合除外
//				if (m_isExclude) m_instanceIDs.Add(other.gameObject.transform.GetInstanceID());

//				//GetComponent
//				var request = transform.root.GetComponent<DamageReceiver>();
//				//失敗したらGetComponentInParent
//				if (request == null)
//					request = other.gameObject.transform.GetComponentInParent<DamageReceiver>();
//				//それでも取得できなければ終了
//				if (request == null)
//					return;

//				//DamageRequest
//				request.Request(transform.root.gameObject, Vector3.zero, m_attack, m_blowAwayType);
//			}
//		}
//		void OnCollisionEnter(Collision other)
//		{
//			//ヒットしたオブジェクトが該当レイヤー & 除外リスト該当なし
//			if (m_hitLayers.EqualBitsForGameObject(other.gameObject) && !m_instanceIDs.Contains(other.gameObject.transform.GetInstanceID()))
//			{
//				//除外設定がある場合除外
//				if (m_isExclude) m_instanceIDs.Add(other.gameObject.transform.GetInstanceID());

//				//GetComponent
//				var request = transform.root.GetComponent<DamageReceiver>();
//				//失敗したらGetComponentInParent
//				if (request == null)
//					request = other.gameObject.transform.GetComponentInParent<DamageReceiver>();
//				//それでも取得できなければ終了
//				if (request == null)
//					return;

//				//DamageRequest
//				request.Request(transform.root.gameObject, Vector3.zero, m_attack, m_blowAwayType);
//			}
//		}
//	}
//}