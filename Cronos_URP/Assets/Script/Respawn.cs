using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어를 리스폰하는 친구 임시로 만듦
public class Respawn : MonoBehaviour
{
	GameObject playerObj;
	Player player;

	private void Awake()
	{
		//DontDestroyOnLoad(this.gameObject);
	}
	private void Start()
	{
		//0. 플레이어가 필요하다.
		playerObj = GameObject.Find("Player");
		player = playerObj.GetComponent<Player>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			RespawnPlayer();
		}
	}
	public void RespawnPlayer()
	{
		//1. 플레이어를 inactive 한다.
		playerObj.SetActive(false);
		//2. 플레이어의 데이터를 덧씌운다.
		player.PlayerRespawn();
		//3. 플레이어를 active 한다.
		playerObj.SetActive(true);
	}
}
