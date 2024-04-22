using UnityEngine;

// 플레이어 기본상태를 상속받은 movestate
public class PlayerMoveState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
	private const float AnimationDampTime = 0.1f;
	private const float CrossFadeDuration = 0.1f;

	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.Velocity.y = Physics.gravity.y;
		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);
		
		stateMachine.InputReader.onJumpPerformed += SwitchToJumpState;	// 스테이트에 돌입할때 input에 맞는 함수를 넣어준다
		stateMachine.InputReader.onLAttackPerformed += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackPerformed += SwitchToRAttackState;
	}

	// state의 update라 볼 수 있지
	public override void Tick()
	{
		// playerComponent기준으로 땅에 닿아있지 않다면
		if (!stateMachine.Controller.isGrounded)
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // 상태를 생성해서 접근한다.
		}


		CalculateMoveDirection();	// 방향을 계산하고
		FaceMoveDirection();		// 캐릭터 방향을 바꾸고
		Move();						// 이동한다.

		float moveSpeed = 0.5f;

		if(Input.GetButton("Run"))
		{
			moveSpeed *= 2;
		}
		else { moveSpeed = 0.5f; }

		// 애니메이터 movespeed의 파라메터의 값을 정한다.
		stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);
	}
	
	public override void Exit()
	{
		// 상태를 탈출할때는 jump의 대한 Action을 제거해준다.
		stateMachine.InputReader.onJumpPerformed -= SwitchToJumpState;
		stateMachine.InputReader.onLAttackPerformed -= SwitchToLAttackState;
		stateMachine.InputReader.onRAttackPerformed -= SwitchToRAttackState;

	}

	// 점프상태로 바꾸는 함수
	private void SwitchToJumpState()
	{
		stateMachine.SwitchState(new PlayerJumpState(stateMachine));
	}

	private void SwitchToLAttackState()
	{
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}
	private void SwitchToRAttackState()
	{
		stateMachine.SwitchState(new PlayerPunchState(stateMachine));
	}
}


	

