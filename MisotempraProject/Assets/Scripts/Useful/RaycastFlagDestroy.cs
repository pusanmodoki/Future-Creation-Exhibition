//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RaycastがHitしたらDestoryするRaycastFlagDestroy
/// </summary>
public class RaycastFlagDestroy : MonoBehaviour
{
	/// <summary>this RaycastFlags</summary>
	[SerializeField, Tooltip("this RaycastFlags")]
    RaycastFlags m_raycastFlags = null;
	/// <summary>Enabled RaycastFlags->isHitEnter</summary>
	[SerializeField, Tooltip("Enabled RaycastFlags->isHitEnter")]
    bool m_isOnHitEnter = true;
	/// <summary>Enabled RaycastFlags->isOtherObjectHit</summary>
	[SerializeField, Tooltip("Enabled RaycastFlags->isOtherObjectHit")]
    bool m_isOnOtherObjectHit = true;
	/// <summary>Destroy objects</summary>
	[SerializeField, Tooltip("Destroy objects")]
    List<GameObject> m_destroyObjects = new List<GameObject>();

	/// <summary>[LateUpdate]</summary>
    void LateUpdate()
    {
		//Hit & Enabled->削除
        if ((m_raycastFlags.isEnter & m_isOnHitEnter) 
			| (m_raycastFlags.isOtherObjectHit & m_isOnOtherObjectHit))
        {
            foreach (var e in m_destroyObjects)
                Destroy(e);
			m_destroyObjects.Clear();
        }
    }
}
