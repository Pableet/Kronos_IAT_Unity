using UnityEngine;

public class PlayerBuffState : PlayerBaseState
{

	private readonly int MoveBlendTreeHash = Animator.StringToHash("Com_Attack_Ready");
	private const float CrossFadeDuration = 0.1f;
	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);
		stateMachine.InputReader.onLAttackStart += SwitchEnforcedAttackState;
	}
	public override void Tick()
	{

		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

		if (stateInfo.normalizedTime >= 0.7f) 
				stateMachine.Animator.speed = 0f;

		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
// 			stateMachine.Animator.StartPlayback();
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
		//stateMachine.Animator.StartPlayback();
		stateMachine.Animator.speed = 1f;
		stateMachine.InputReader.onLAttackStart -= SwitchEnforcedAttackState;
	}

	protected void SwitchEnforcedAttackState()
	{
		stateMachine.SwitchState(new PlayerEnforcedAttackState(stateMachine));
	}

}
