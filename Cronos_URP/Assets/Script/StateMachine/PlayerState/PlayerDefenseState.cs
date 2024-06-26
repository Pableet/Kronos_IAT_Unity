using UnityEditor;
using UnityEngine;

public class PlayerDefenceState : PlayerBaseState
{

	private readonly int DefenceHash = Animator.StringToHash("defence");
	private const float CrossFadeDuration = 0.1f;

	private bool isdefence = false;
	public PlayerDefenceState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Player._defnsible.isDefending = true;
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(DefenceHash, CrossFadeDuration);
		
		stateMachine.InputReader.onRAttackPerformed += isDefencing;
		stateMachine.InputReader.onRAttackCanceled += isNotDefencing;

		stateMachine.Rigidbody.velocity = Vector3.zero;
	}

	public override void Tick()
	{
		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

		if(isdefence)
		{
			if (stateInfo.normalizedTime >= 0.2f)
				stateMachine.Animator.speed = 0f;
		}
		else if (stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
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
		stateMachine.InputReader.onRAttackCanceled -= isNotDefencing;
		stateMachine.InputReader.onRAttackPerformed -= isDefencing;
	}

	private void isDefencing()
	{
		isdefence = true;
		stateMachine.Player._defnsible.isDefending = true;
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(DefenceHash, CrossFadeDuration);
	}

	private void isNotDefencing()
	{
		stateMachine.Animator.StartPlayback();
		stateMachine.Animator.speed = 1f;
		stateMachine.Player._defnsible.isDefending = false;
		isdefence = false;
	}



}
