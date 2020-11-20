//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
	/// <summary>
	/// BoxCastに当たっていた場合ダメージをリクエストするDamageRequesterBoxCast
	/// </summary>
	public class DamageRequesterBoxCast : Detail.BaseRequester
	{
		/// <summary>this RaycastFlags</summary>
		[SerializeField, Tooltip("this BoxCastFlags")]
		BoxCastFlags m_boxCastFlags = null;

		/// <summary>LateUpdate</summary>
		void LateUpdate()
		{
			if (attackInfo == null) return;

			//RaycastFlagsがHit
			if (m_boxCastFlags.isStay)
			{
				//Rootオブジェクト取得
				Transform root = m_boxCastFlags.boxCastResult.transform.root;

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