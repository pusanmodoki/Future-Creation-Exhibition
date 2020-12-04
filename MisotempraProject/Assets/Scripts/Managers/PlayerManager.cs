using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerManager : Singleton.DontDestroySingletonMonoBehaviour<PlayerManager>
//{
//	public GameObject playerInstance { get; private set; } = null;

//    public Player.PlayerController playerController { get; private set; } = null;

//	protected override void Init()
//	{
//		playerInstance = GameObject.Find("Player");
//        if (!playerInstance)
//        {
//#if UNITY_EDITOR
//            Debug.LogError("Player Not Found");
//#endif
//        }
//        playerController = playerInstance.GetComponent<Player.PlayerController>();
//        if (!playerController)
//        {
//#if UNITY_EDITOR
//            Debug.LogError("PlayerController Not Attach");
//#endif
//        }
//    }
//}
