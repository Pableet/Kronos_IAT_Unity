using System.Runtime.InteropServices;
using UnityEngine;


// 기본상태
// 애니메이션 : idle
// 끝
public class PlayerIdleState : PlayerBaseState
{
	private readonly int idleHash = Animator.StringToHash("Idle");
	private readonly float duration = 0.3f;
	private bool isMove = false;

	public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	
	public override void Enter()
	{
		// 1. Idle 애니메이션을 재생할것
		stateMachine.Animator.CrossFadeInFixedTime(idleHash, duration);

		stateMachine.InputReader.onMove += IsMove;

    }
	public override void Tick()
	{
		// playerComponent기준으로 땅에 닿아있지 않다면
		if (!stateMachine.Controller.isGrounded)
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // 상태를 생성해서 접근한다.
		}
		// 움직이면 == 이동키입력을 받으면
		if (isMove)
		{
			// 이동상태로 바뀐다
			SwitchToMoveState();
		}
	}
	public override void Exit()
	{
		stateMachine.InputReader.onMove -= IsMove;
	}

	private void IsMove()
	{
		isMove = true;
	}

	private void SwitchToMoveState()
	{
		stateMachine.SwitchState(new PlayerMoveState(stateMachine));
	}
}
