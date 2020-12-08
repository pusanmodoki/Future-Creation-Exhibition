//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
	/// <summary>
	/// Raycastに当たっていた場合ダメージをリクエストするDamageRequesterRaycast
	/// </summary>
	public class DamageRequesterRaycast : Detail.BaseRequester
	{
		/// <summary>this RaycastFlags</summary>
		[SerializeField, Tooltip("this RaycastFlags")]
		RaycastFlags m_raycastFlags = null;
		
		/// <summary>LateUpdate</summary>
		void LateUpdate()
		{
			if (attackInfo == null || attackInfo.id < 0) return;

			//RaycastFlagsがHit
			if (m_raycastFlags.isStay)
			{
				//Rootオブジェクト取得
				Transform root = m_raycastFlags.rayCastResult.transform.root;

				//GetComponent
				var request = root.GetComponent<DamageController>();
				//取得できなければ終了
				if (request == null) return;

				//DamageRequest
				request.receiver.Request(transform.root.gameObject, attackInfo);
			}
		}
	}
}