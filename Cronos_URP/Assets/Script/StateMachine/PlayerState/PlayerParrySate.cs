using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	//private readonly int JumpHash = Animator.StringToHash("Parry");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private const float CrossFadeDuration = 0.3f;

	Vector3 direction;

	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.SetTrigger(dodgeHash);
		stateMachine.Player._damageable.isInvulnerable = true;


		// 키입력이 있다면
		if(stateMachine.InputReader.moveComposite.magnitude > 0f)
		{
			direction = stateMachine.Velocity.normalized;
		}
		else // 없다면
		{
			// 카메라의 전방벡터
			Vector3 temp = Camera.main.transform.forward;
			temp .y = 0f;
			direction = temp.normalized;
		}

	}
	public override void Tick()
	{
		// 회피방향은
		// 1. 키입력
		// 2. 카메라 front
	
		//CalculateMoveDirection();
	}
	public override void FixedTick()
	{
		//FaceMoveDirection();
		stateMachine.Rigidbody.rotation = Quaternion.LookRotation(direction);
		stateMachine.Rigidbody.AddForce(direction * 15f);
	}
	public override void LateTick()
	{
	}

	public override void Exit()
	{
		stateMachine.Player._damageable.isInvulnerable = false;
	}
}
	