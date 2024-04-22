using System.Resources;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
	private readonly int FallHash = Animator.StringToHash("Fall");	// 전환될 애니메이션의 해쉬
	private const float CrossFadeDuration = 0.1f;

	public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Velocity.y = 0f;
		stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
	}
	public override void Tick()
	{
		ApplyGravity();
		Move();
		if (stateMachine.Controller.isGrounded)
		{
			stateMachine.SwitchState(new PlayerMoveState(stateMachine));
		}
	}
	public override void Exit()
	{

	}

}
