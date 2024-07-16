using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter(){}
	public override void Tick(){}
	public override void FixedTick()
	{
		stateMachine.Rigidbody.rotation = Quaternion.LookRotation(PlayerStateMachine.GetInstance().Velocity.normalized);
		stateMachine.Rigidbody.AddForce(PlayerStateMachine.GetInstance().Velocity.normalized * 15f);
	}
	public override void LateTick(){}

	public override void Exit(){}
}
	