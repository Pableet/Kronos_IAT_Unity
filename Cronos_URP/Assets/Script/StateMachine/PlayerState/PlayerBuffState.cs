using Cinemachine;
using System.Resources;
using UnityEngine;

public class PlayerBuffState : PlayerBaseState
{

	private readonly int MoveBlendTreeHash = Animator.StringToHash("Buff_01");
	private const float CrossFadeDuration = 0.1f;
	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);
		stateMachine.InputReader.onLAttackStart += SwitchEnforcedAttackState;
	}
	public override void Tick()
	{
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
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
		stateMachine.InputReader.onLAttackStart -= SwitchEnforcedAttackState;
	}

	protected void SwitchEnforcedAttackState()
	{
		stateMachine.SwitchState(new PlayerEnforcedAttackState(stateMachine));
	}

}
