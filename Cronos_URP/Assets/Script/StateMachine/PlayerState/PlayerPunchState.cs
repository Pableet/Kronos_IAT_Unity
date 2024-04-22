using UnityEngine;

public class PlayerPunchState : PlayerBaseState
{
	private readonly int AttackHash = Animator.StringToHash("Hook");
	private const float CrossFadeDuration = 0.1f;

	public PlayerPunchState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
	}
	public override void Tick()
	{
		// 현재 애니메이션 정보를 받아온다
		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

		// 애니메이션이 끝났다면
		if (stateInfo.IsName("Hook") && stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
		{
			stateMachine.SwitchState(new PlayerMoveState(stateMachine));
		}
	}
	public override void Exit()
	{
	}

}
