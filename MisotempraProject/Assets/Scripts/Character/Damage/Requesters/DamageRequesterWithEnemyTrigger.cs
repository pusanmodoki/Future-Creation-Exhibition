////作成者 : 植村将太
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Damage
//{
//	/// <summary>
//	/// Colliderに当たっていた場合ダメージをリクエストするDamageRequesterWithEnemyTrigger
//	/// </summary>
//	public class DamageRequesterWithEnemyTrigger : MonoBehaviour
//	{
//		/// <summary>this EnemyStatus</summary>
//		[SerializeField, Tooltip("this EnemyStatus")]
//		EnemyStatus m_enemyStatus = null;
//		/// <summary>ヒットするLayerMask</summary>
//		[SerializeField, Tooltip("ヒットするLayerMask")]
//		LayerMaskEx m_hitLayers = int.MaxValue;
//		/// <summary>除外するTag</summary>
//		[SerializeField, Tooltip("除外するTag")]
//		string m_excludeTag = "";

//		/// <summary>HitしたオブジェクトのInstanceID</summary>
//		List<int> m_instanceIDs = new List<int>();
//		/// <summary>攻撃カウンタを保存</summary>
//		int m_oldCounter = -1;

//		/// <summary>[OnTriggerEnter]</summary>
//		void OnTriggerEnter(Collider other)
//		{
//			//攻撃カウンタが変わっている場合リストを再生成
//			if (m_oldCounter != m_enemyStatus.numAttackCounter)
//				m_instanceIDs = new List<int>();

//			//攻撃している & 除外リスト該当なし & 該当レイヤー
//			if (m_enemyStatus.isAttackNow && other.gameObject.tag != m_excludeTag
//				&& !m_instanceIDs.Contains(other.transform.GetInstanceID())
//				&& m_hitLayers.EqualBitsForGameObject(other.gameObject))
//			{
//				//除外リスト追加
//				m_instanceIDs.Add(other.transform.GetInstanceID());
//				//カウンタ保存
//				m_oldCounter = m_enemyStatus.numAttackCounter;

//				//GetComponent
//				var request = other.transform.root.GetComponent<DamageReceiver>();
//				//失敗したらGetComponentInParent
//				if (request == null)
//					request = other.gameObject.transform.GetComponentInParent<DamageReceiver>();
//				//それでも取得できなければ終了
//				if (request == null) return;

//				//DamageRequest
//				request.Request(transform.root.gameObject, Vector3.zero, m_enemyStatus.attack, m_enemyStatus.blowAwayType);
//			}
//		}
//	}
//}