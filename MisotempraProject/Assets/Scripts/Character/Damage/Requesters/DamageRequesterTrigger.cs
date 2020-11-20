using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
	public class DamageRequesterTrigger : Damage.Detail.BaseRequester
	{
		/// <summary>OnTriggerEnter</summary>
		void OnTriggerEnter(Collider other)
		{
			if (attackInfo == null) return;
			//Rootオブジェクト取得
			Transform root = other.transform.root;

			//GetComponent
			var request = root.GetComponent<DamageReceiver>();
			//取得できなければ終了
			if (request == null) return;

			//DamageRequest
			request.Request(transform.root.gameObject, attackInfo);
		}
	}
}