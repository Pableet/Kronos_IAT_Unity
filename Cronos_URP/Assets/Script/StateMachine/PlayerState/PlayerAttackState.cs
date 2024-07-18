using UnityEngine;
public class PlayerAttackState : PlayerBaseState
{
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		CalculateMoveDirection();   // 방향을 계산하고
	}
	public override void FixedTick()
	{
		stateMachine.Rigidbody.AddForce(stateMachine.transform.forward * stateMachine.MoveForce * stateMachine.Animator.speed * Time.fixedDeltaTime, ForceMode.Impulse);
	}
	public override void LateTick(){}
	public override void Exit()
	{ 
		stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
	}
}
