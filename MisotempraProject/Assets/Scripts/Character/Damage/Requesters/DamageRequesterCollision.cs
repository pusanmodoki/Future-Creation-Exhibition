using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
	public class DamageRequesterCollision : Damage.Detail.BaseRequester
	{
		/// <summary>OnCollisionEnter</summary>
		void OnCollisionEnter(Collision collision)
		{
			if (attackInfo == null) return;
			//Rootオブジェクト取得
			Transform root = collision.transform.root;

			//GetComponent
			var request = root.GetComponent<DamageController>();
			//取得できなければ終了
			if (request == null) return;

			//DamageRequest
			request.receiver.Request(transform.root.gameObject, attackInfo);
		}
	}
}