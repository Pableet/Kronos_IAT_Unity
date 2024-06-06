using UnityEngine;

// 맞았을 때
public class PlayerDamagedState : PlayerBaseState
{
	AnimatorStateInfo stateInfo;
	public PlayerDamagedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public static readonly int hashDamageBase = Animator.StringToHash("hit01");
	public override void Enter()
	{
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(hashDamageBase, 0.1f);
	}
	public override void Tick()
	{
		stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
		Debug.Log($"{stateInfo.normalizedTime}");
		// 애니메이션이 종료되고
		if (stateInfo.normalizedTime >= 1.0f)
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
	}

}
