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
// 		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
// 		if (stateInfo.normalizedTime > 0.3)
// 		{
// 			stateMachine.Animator.speed = 0.0001f;
// 		}

		ApplyGravity();
		Move();
		if (stateMachine.Controller.isGrounded)
		{
			stateMachine.Animator.speed = 1f;
			stateMachine.SwitchState(new PlayerMoveState(stateMachine));
		}
	}
	public override void FixedTick()
	{
	}
	public override void LateTick()
	{
	}
	public override void Exit()
	{

	}

}
