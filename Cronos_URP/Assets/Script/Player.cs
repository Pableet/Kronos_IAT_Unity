using Cinemachine;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
using UnityEngine.Rendering;
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
	[Header("State")]
	[SerializeField] private string CurrentState;

	[Header("Move Option")]
	[SerializeField] private float Speed = 5f;
	/*[SerializeField] */
	private float JumpForce = 10f; // 점프 만들면 쓰지뭐
	[SerializeField] private float LookRotationDampFactor = 10f;
	//[SerializeField] private float Damage;
	[SerializeField] private float attackCoefficient = 0.1f;
	[SerializeField] private float moveCoefficient = 0.1f;

	public float stopTiming = 0.2f; // 히트스탑은 이런식으로 작동하는게 아닐것이다. 스탑 시간정도로 바꿔서 쓰는게 좋을듯
	[SerializeField] private float maxTP;
	[SerializeField] private float maxCP;

	[SerializeField] private float currentDamage;
	[SerializeField] private float attackSpeed;


	[SerializeField] private float currentTP;
	[SerializeField] private float currentCP;
	[SerializeField] private float chargingCP = 10f;


	[SerializeField] private bool isEnforced = false;



	// Property
	private float totalspeed;
	public float moveSpeed { get { return totalspeed; } }
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }
	public float AttackCoefficient { get { return attackCoefficient; } set { attackCoefficient = value; } }
	public float MoveCoefficient { get { return moveCoefficient; } set { moveCoefficient = value; } }
	// chronos in game Option
	public float MaxCP { get { return maxCP; } set { maxCP = value; } }
	public float MaxTP { get { return maxTP; } set { maxTP = value; } }
	public float CP { get { return currentCP; } set { currentCP = value; } }
	public float TP { get { return currentTP; } set { currentTP = value; } }
	public float ChargingCP { get { return chargingCP; } set { chargingCP = value; } }
	public float CurrentDamage { get { return currentDamage; } set { currentDamage = value; } }
	public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
	public bool IsDecreaseCP { get; set; }
	public bool IsEnforced { get { return isEnforced; } set { isEnforced = value; } }	// 강화상태를 위한 프로퍼티

	// 플레이어 데이터를 저장하고 respawn시 반영하는 데이터
	PlayerData playerData = new PlayerData();
	Transform playerTransform;
	AutoTargetting targetting;

	MeleeTriggerEnterDamager meleeWeapon;
	PlayerStateMachine PlayerFSM;

	public Damageable _damageable;
	public Defensible _defnsible;

	/// 안돼는데 안돼
	private Vector3 lastPosition;
	private Quaternion lastRotation;

	protected void OnDisable()
	{
		_damageable.onDamageMessageReceivers.Remove(this);
	}

	private void OnEnable()
	{
		/// 안돼는데 안돼
		lastPosition = transform.position;
		lastRotation = transform.rotation;

		_damageable = GetComponent<Damageable>();
		_damageable.onDamageMessageReceivers.Add(this);

		_defnsible = GetComponent<Defensible>();

		// 감속/가속 변경함수를 임시로 사용해보자
		// 반드시 지워져야할 부분이지만 임시로 넣는다
		PlayerFSM = GetComponent<PlayerStateMachine>();
		///안돼
		PlayerFSM.Animator.applyRootMotion = true;
		playerTransform = GetComponent<Transform>();

		meleeWeapon = GetComponentInChildren<MeleeTriggerEnterDamager>();
		meleeWeapon.SetOwner(gameObject);
		meleeWeapon.OnTriggerEnterEvent += ChargeCP;

		targetting = GetComponentInChildren<AutoTargetting>();
		totalspeed = Speed;

		if (GameManager.Instance.isRespawn)
		{
			PlayerRespawn();
		}

		_damageable.currentHitPoints += maxTP;

		GameManager.Instance.PlayerDT = playerData;
		GameManager.Instance.PlayerDT.saveScene = SceneManager.GetActiveScene().name;
		
		currentDamage = meleeWeapon.damageAmount;
	}
	private void ChargeCP(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			Debug.Log("cp를 회복한다.");
			if (CP < maxCP && !IsDecreaseCP)
			{
				CP += chargingCP;

				if (CP > maxCP)
				{
					CP = maxCP;
				}
			}
		}
	}

	private void Update()
	{
		CurrentState = PlayerFSM.GetState().GetType().Name;

		// 실시간으로 TP 감소
		_damageable.currentHitPoints -= Time.deltaTime;

		// 실시간으로 CP감소
		if (IsDecreaseCP && CP > 0)
		{
			CP -= Time.deltaTime;
			if (CP <= 0)
			{
				IsDecreaseCP = false;
				CP = 0;
				Debug.Log("몬스터들의 속도가 원래대로 돌아온다.");
				BulletTime.Instance.SetNormalSpeed();
			}
		}

 		TP = _damageable.currentHitPoints;
// 
// 		if (Input.GetKeyDown(KeyCode.UpArrow))
// 		{
// 			if (currentTP + 100f < maxTP)
// 			{
// 				currentTP = currentTP + 100;
// 				_damageable.currentHitPoints = currentTP;
// 			}
// 			else
// 			{
// 				currentTP = maxTP;
// 				_damageable.currentHitPoints = currentTP;
// 			}
// 
// 		}

		if (TP <= 0)
		{
			_damageable.JustDead();
		}


		//MoveTest();

	}
	private void FixedUpdate()
	{

	}

	private void OnAnimatorMove()
	{
// 		Vector3 deltaPosition = PlayerFSM.Animator.deltaPosition;
// 		Quaternion deltaRotation = PlayerFSM.Animator.deltaRotation;
// 
// 		transform.position += deltaPosition;
// 		transform.rotation = deltaRotation * transform.rotation;
// 
// 		// 위치와 회전을 저장하여 애니메이션이 끝난 후에도 적용
// 		lastPosition = transform.position;
// 		lastRotation = transform.rotation;
	}
	private void LateUpdate()
	{
// 		if (!PlayerFSM.Animator.GetCurrentAnimatorStateInfo(0).loop && PlayerFSM.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
// 		{
// 			PlayerFSM.transform.position = lastPosition;
// 			PlayerFSM.transform.rotation = lastRotation;
// 		}
 	}
	private void OnTriggerEnter(Collider other)
	{
	}

	/// 증강
	public void AdjustTP(float value)
	{
		maxTP += value;
		_damageable.currentHitPoints += value;
	}
	public void AdjustAttackPower(float value)
	{
		currentDamage = value;
		meleeWeapon.damageAmount = currentDamage;
	}
	public void AdjustSpeed(float vlaue)
	{
		Speed += vlaue;
	}
	public void AdjustAttackSpeed(float value)
	{
		AttackSpeed = value;
	}
	public void AdjustChargingCP(float value)
	{
		chargingCP = value;
	}

	public void AdjustAttackCoefficient(float value)
	{
		attackCoefficient = value;
	}
	public void AdjustMoveCoefficient(float value)
	{
		moveCoefficient = value;
	}


	public void OnReceiveMessage(MessageType type, object sender, object data)
	{
		switch (type)
		{
			case MessageType.DAMAGED:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					if (true)
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
		//  방어상태가 아닐때만 하자
		if (PlayerFSM.GetState().ToString() != "PlayerDefenceState")
		{
			PlayerFSM.SwitchState(new PlayerDamagedState(PlayerFSM));
		}
	}

	public void Death(Damageable.DamageMessage msg)
	{
		PlayerDeadRespawn();
	}


	public void StartPlayer()
	{
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

// 	void MoveTest()
// 	{
// 		float animationSpeed = PlayerFSM.Animator.deltaPosition.magnitude;
// 
// 		// test
// 		Vector3 direction = PlayerFSM.transform.forward.normalized;
// 
// 		// 새로운 벡터 계산
// 		Vector3 newDeltaPosition = direction * animationSpeed;
// 
// 		PlayerFSM.transform.Translate(newDeltaPosition);
// 	}


}
