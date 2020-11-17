using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton.DontDestroySingletonMonoBehaviour<PlayerManager>
{
	public GameObject playerInstance { get; private set; } = null;

	protected override void Init()
	{
		playerInstance = GameObject.Find("Player");
	}
}
