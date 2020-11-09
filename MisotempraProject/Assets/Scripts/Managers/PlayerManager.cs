using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
	public GameObject playerInstance { get; private set; } = null;

	protected override void Init()
	{
		playerInstance = GameObject.Find("Player");
	}
}
