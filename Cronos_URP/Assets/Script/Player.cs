using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Player가 갖는 정보를 한 눈에 볼 수 있는 플레이어 스크립트
/// 1. status
/// 2. Item
/// 3. 등등
/// </summary>
public class Player : MonoBehaviour
{
	PlayerStateMachine PlayerFSM;

	[Header("State")]
	[SerializeField] private string CurrentState;
	[SerializeField] private string Attribute;

	[Header("Move Option")]
	[SerializeField] private float Speed = 5f;
	[SerializeField] private float JumpForce = 10f;
	[SerializeField] private float LookRotationDampFactor = 10f;

	public float moveSpeed { get { return Speed; } }
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }

	// chronos in game Option
	private float CP { get; set; }
	private float TP { get; set; }


	bool isAccel = false;

	private void Start()
	{
		PlayerFSM = GetComponent<PlayerStateMachine>();

		// 감속/가속 변경함수를 임시로 사용해보자
		// 반드시 지워져야할 부분이지만 임시로 넣는다
		Attribute = "Is Noting";
		PlayerFSM.InputReader.onSwitching += Switching;
	}

	public void FixedUpdate()
	{
		// 현재 상태를 표시하기 위한 무언가
		// string으로 뽑으면 좀 그럴까? 
		// 성능적인 이슈가 없을거라고생각되지가 않는다
		CurrentState = PlayerFSM.GetState().GetType().Name;
	}

	// 감속, 가속 변화를 위한 임시함수 
	public void Switching()
	{
		isAccel = !isAccel;

		if (isAccel)
		{
			Attribute = "가속";
		}
		else
		{
			Attribute = "감속";
		}

	}



}
