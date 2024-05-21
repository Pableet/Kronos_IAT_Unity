using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// Player가 갖는 정보를 한 눈에 볼 수 있는 플레이어 스크립트
/// 1. status
/// 2. Item
/// 3. 등등
/// </summary>
public class Player : MonoBehaviour, IMessageReceiver
{
	public static readonly int hashDamageBase = Animator.StringToHash("hit01");

	[Header("State")]
	[SerializeField] private string CurrentState;

	[Header("Move Option")]
	[SerializeField] private float Speed = 5f;
	[SerializeField] private float JumpForce = 10f;
	[SerializeField] private float LookRotationDampFactor = 10f;
	[SerializeField] private float Damage;
	[SerializeField] private float AttackSpeed = 1f;

	public float stopTiming = 0.2f;

	public float maxTP;
	public float currentTP;
	public float currentDamage;
	public float currentAttackSpeed = 1f;

	float totalspeed;
	MeleeWeapon meleeWeapon;
	PlayerStateMachine PlayerFSM;


	public float moveSpeed { get { return totalspeed; } }
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }

	// chronos in game Option
	public float CP { get; set; }
	public float TP { get { return currentTP; } set => currentTP = value; }

	// 플레이어 데이터를 저장하고 respawn시 반영하는 데이터
	PlayerData playerData = new PlayerData();
	Transform playerTransform;
	AutoTargetting targetting;

	public Damageable _damageable;
	public Defensible _defnsible;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_damageable = GetComponent<Damageable>();
		_defnsible = GetComponent<Defensible>();
		_damageable.onDamageMessageReceivers.Add(this);

	}
	protected void OnDisable()
	{
		_damageable.onDamageMessageReceivers.Remove(this);
	}

	private void Start()
	{
		// 감속/가속 변경함수를 임시로 사용해보자
		// 반드시 지워져야할 부분이지만 임시로 넣는다
		PlayerFSM = GetComponent<PlayerStateMachine>();
		playerTransform = GetComponent<Transform>();
		meleeWeapon = GetComponentInChildren<MeleeWeapon>();
		meleeWeapon.SetOwner(gameObject);
		targetting = GetComponentInChildren<AutoTargetting>();
		totalspeed = Speed;

		if (GameManager.Instance.isRespawn)
		{
			PlayerRespawn();
		}

		AdjustAttackPower(Damage);  // 데미지 설정
		AdjustTP(currentTP); // TP설정

		Debug.Log($"{meleeWeapon.damage}");

		GameManager.Instance.PlayerDT = playerData;
		GameManager.Instance.PlayerDT.saveScene = SceneManager.GetActiveScene().name;

	}
	private void Update()
	{
		CurrentState = PlayerFSM.GetState().GetType().Name;

		// 실시간으로 TP 감소
		_damageable.currentHitPoints -= Time.deltaTime;

		TP = _damageable.currentHitPoints;

		if (TP <= 0)
		{
			//Debug.Log("죽엇당");
			_damageable.JustDead();
		}

		currentDamage = meleeWeapon.damage;

	}

	private void OnTriggerEnter(Collider other)
	{

	}


	public void AdjustTP(float value)
	{
		maxTP += value;
		_damageable.currentHitPoints += value;
	}

	public void AdjustAttackPower(float value)
	{
		currentDamage = value;
		meleeWeapon.damage = currentDamage;
	}
	public void AdjustSpeed(float vlaue)
	{
		Speed += vlaue;
	}

	public void OnReceiveMessage(MessageType type, object sender, object data)
	{
		switch (type)
		{
			case MessageType.DAMAGED:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					if(true)
					{
						damageData.amount = 0;
					}
					Damaged(damageData);
				}
				break;
			case MessageType.DEAD:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					Death(damageData);
				}
				break;
		}
	}



	void Damaged(Damageable.DamageMessage damageMessage)
	{
		PlayerFSM.Animator.CrossFadeInFixedTime(hashDamageBase, 0.1f);
	}


	public void Death(Damageable.DamageMessage msg)
	{
		//Debug.Log("죽었다리");
		PlayerDeadRespawn();
		//var replacer = GetComponent<ReplaceWithRagdoll>();
		//
		//if (replacer != null)
		//{
		//    replacer.Replace();
		//}

		//We unparent the hit source, as it would destroy it with the gameobject when it get replaced by the ragdol otherwise
	}


	public void StartPlayer()
	{
		Start();
		PlayerFSM.Start();
		targetting.Start();
		gameObject.transform.position = new Vector3(0f, 7f, 0f);
	}

	public void SetSpeed(float value)
	{
		totalspeed = value * Speed;
	}


	// 리스폰데이터를 GameManager에 저장하자
	public void SavePlayerData()
	{
		playerData.saveScene = SceneManager.GetActiveScene().name; // 현재 씬의 이름을 가져온다
		playerData.TP = TP;
		playerData.TP = CP;
		playerData.RespawnPos = playerTransform.position;
		// 필요한 데이터를 여기 계속 더하자
		GameManager.Instance.PlayerDT = playerData;
	}

	public void PlayerDeadRespawn()
	{
		if (SceneManager.GetActiveScene().name != GameManager.Instance.PlayerDT.saveScene)
		{
			SceneManager.LoadScene(GameManager.Instance.PlayerDT.saveScene);
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		TP = maxTP;
		CP = GameManager.Instance.PlayerDT.CP;
		if (GameManager.Instance.PlayerDT.RespawnPos.x == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.y == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.z == 0f)
		{
			GameManager.Instance.PlayerDT.RespawnPos = new Vector3(0f, 7f, 0f);
		}
		else
		{

			playerTransform.position = (Vector3)GameManager.Instance.PlayerDT.RespawnPos;
		}
	}


	public void PlayerRespawn()
	{
		if (SceneManager.GetActiveScene().name != GameManager.Instance.PlayerDT.saveScene)
		{
			SceneManager.LoadScene(GameManager.Instance.PlayerDT.saveScene);
		}
		TP = GameManager.Instance.PlayerDT.TP;
		CP = GameManager.Instance.PlayerDT.CP;
		if (GameManager.Instance.PlayerDT.RespawnPos.x == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.y == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.z == 0f)
		{
			GameManager.Instance.PlayerDT.RespawnPos = new Vector3(0f, 7f, 0f);
		}
		else
		{

			playerTransform.position = (Vector3)GameManager.Instance.PlayerDT.RespawnPos;
		}
	}


	// 플레이어를 죽이자
	public void PlayerDeath()
	{
		TP = 0f;
	}

	public void AttackStart()
	{
		meleeWeapon.BeginAttack(false);
	}
	public void AttackEnd()
	{
		meleeWeapon.EndAttack();
	}


}
