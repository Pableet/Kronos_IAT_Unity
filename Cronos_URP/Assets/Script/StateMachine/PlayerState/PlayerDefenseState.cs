using UnityEngine;

public class PlayerDefenceState : PlayerBaseState
{

	private readonly int DefenceHash = Animator.StringToHash("Guard");
	private const float CrossFadeDuration = 0.3f;

	private bool isdefence = false;
	public PlayerDefenceState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		isdefence = true;
		stateMachine.Player._defnsible.isDefending = true;
		//stateMachine.Animator.Rebind();
		//stateMachine.Animator.CrossFadeInFixedTime(DefenceHash, CrossFadeDuration);

// 		stateMachine.InputReader.onRAttackPerformed += isDefencing;
// 		stateMachine.InputReader.onRAttackCanceled += isNotDefencing;

		stateMachine.Rigidbody.velocity = Vector3.zero;
	}

	public override void Tick()
	{

// 		if(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f&& isdefence)
// 		{
// 			stateMachine.Animator.speed = 0f;
// 		}
		if(Input.GetKey(KeyCode.Mouse1))
		{
			isDefencing();
		}
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			stateMachine.Animator.speed = 1f;
			isNotDefencing();
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
// 		stateMachine.InputReader.onRAttackCanceled -= isNotDefencing;
// 		stateMachine.InputReader.onRAttackPerformed -= isDefencing;
	}

	private void isDefencing()
	{
		isdefence = true;
		stateMachine.Player._defnsible.isDefending = true;
		stateMachine.Animator.SetBool("isGuard", true);
		//stateMachine.Animator.Rebind();
		//stateMachine.Animator.CrossFadeInFixedTime(DefenceHash, CrossFadeDuration);

		if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f && isdefence)
		{
			stateMachine.Animator.speed = 0f;
		}
	}

	private void isNotDefencing()
	{
		//stateMachine.Animator.speed = 1f;
		stateMachine.Animator.SetBool("isGuard", false);
		//stateMachine.Animator.StartPlayback();
		//stateMachine.Animator.speed = 1f;
		stateMachine.Player._defnsible.isDefending = false;
		isdefence = false;
	}



}
