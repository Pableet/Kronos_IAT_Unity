using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerParryState : PlayerBaseState
{
	//private readonly int JumpHash = Animator.StringToHash("Parry");
	private readonly int JumpHash = Animator.StringToHash("Dodge");
	private const float CrossFadeDuration = 0.3f;

	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
		stateMachine.Player._damageable.isInvulnerable = true;
	}
	public override void Tick()
	{
		
		AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

		
		stateMachine.Rigidbody.AddForce(stateMachine.transform.forward* 5);

// 		stateMachine.GetComponentInChildren<Transform>().position +=
// 	GameObjec t.Find("PlayerObj").GetComponent<Transform>().forward.normalized * 4 * Time.deltaTime;

		if (stateInfo.IsName("Dodge") && stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
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
		stateMachine.Player._damageable.isInvulnerable = false;
	}
}
