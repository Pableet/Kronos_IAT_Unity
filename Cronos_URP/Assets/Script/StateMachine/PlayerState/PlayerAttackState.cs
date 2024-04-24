using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
	private readonly int AttackHash = Animator.StringToHash("Jab");
	private const float CrossFadeDuration = 0.1f;

	public float startNormalizedTime = 0.5f;    // 시작 지점
	public float endNormalizedTime = 0.99f;     // 종료 지점



	private bool nextJab = false;
	private bool nextPunch = false;

	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
		stateMachine.InputReader.onLAttackPerformed += NextJab;
		stateMachine.InputReader.onRAttackPerformed += NextPuch;
	}
	public override void Tick()
	{
		// 현재 애니메이션 정보를 받아온다
		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);



		// 애니메이션이 끝났다면
		if (stateInfo.IsName("Jab") && stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
		{
			if (nextJab)
			{
				stateMachine.SwitchState(new PlayerPunchState(stateMachine));
			}
			else if(nextPunch)
			{
				stateMachine.SwitchState(new PlayerPunchState(stateMachine));
			}
			else
			{
				stateMachine.SwitchState(new PlayerMoveState(stateMachine));
			}
		}

	}
	public override void Exit()
	{
		stateMachine.InputReader.onLAttackPerformed -= NextPuch;
	}

	public void NextJab()
	{

		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.IsName("Jab") && stateInfo.normalizedTime >= startNormalizedTime && stateInfo.normalizedTime <= endNormalizedTime)
		{
			nextJab = true;

		}
	}
	public void NextPuch()
	{

		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.IsName("Jab") && stateInfo.normalizedTime >= startNormalizedTime && stateInfo.normalizedTime <= endNormalizedTime)
		{
			nextPunch = true;

		}
	}


}
