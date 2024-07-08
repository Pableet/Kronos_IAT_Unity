using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

// 플레이어 기본상태를 상속받은 movestate
public class PlayerMoveState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
	private const float AnimationDampTime = 0.1f;
	private const float CrossFadeDuration = 0.3f;

	float releaseLockOn = 0f;


	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		//stateMachine.Animator.Rebind();
		//stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // 스테이트에 돌입할때 input에 맞는 함수를 넣어준다
		stateMachine.InputReader.onLAttackStart += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToRAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		//stateMachine.InputReader.onLockOnStart += LockOn;

		stateMachine.InputReader.onSwitchingStart += Deceleration;
	}

	// state의 update라 볼 수 있지
	public override void Tick()
	{
		// 이동하지 않으면 idle
		if (stateMachine.InputReader.moveComposite.magnitude == 0)
		{
			stateMachine.SwitchState(new PlayerIdleState(stateMachine));
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			stateMachine.Player.CP += 1f;
		}

		// 플레이어의 cp 를 이동속도에 반영한다.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		// playerComponent기준으로 땅에 닿아있지 않다면
		if (!IsGrounded())
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // 상태를 생성해서 접근한다.
		}

		float moveSpeed = 0.5f;

		if (Input.GetButton("Run"))
		{
			moveSpeed *= 2;
		}
		else { moveSpeed = 0.5f; }

		stateMachine.Player.SetSpeed(moveSpeed);

		if (Input.GetMouseButtonDown(2))
		{
			// 락온 상태가 아니라면
			if (!stateMachine.Player.IsLockOn)
			{
				// 대상을 찾고
				stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			}
			// 락온상태라면 락온을 해제한다.
			else
			{
				//stateMachine.AutoTargetting.LockOff();
				stateMachine.AutoTargetting.SwitchTarget();
			}
		}

		if (Input.GetMouseButton(2))
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				stateMachine.AutoTargetting.LockOff();
			}
		}
		else
		{
			releaseLockOn = 0f;
		}

		// 애니메이터 movespeed의 파라메터의 값을 정한다.
		stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);

		CalculateMoveDirection();   // 방향을 계산하고

	}
	public override void FixedTick()
	{
		FaceMoveDirection();        // 캐릭터 방향을 바꾸고
		Move();                     // 이동한다.	
	}

	public override void LateTick() { }

	public override void Exit()
	{
		// 상태를 탈출할때는 jump의 대한 Action을 제거해준다.
		stateMachine.InputReader.onJumpPerformed -= SwitchToParryState;
		stateMachine.InputReader.onLAttackStart -= SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToRAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;

		stateMachine.InputReader.onSwitchingStart -= Deceleration;

	}

	private void Deceleration()
	{
		if (stateMachine.Player.CP >= 100)
		{
			Debug.Log("몬스터들이 느려진다");
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}

	}

	private void SwitchToParryState()
	{
		Debug.Log("구른다");
		stateMachine.SwitchState(new PlayerParryState(stateMachine));
	}

	private void SwitchToLAttackState()
	{
		//stateMachine.Animator.SetTrigger("Attack");
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}

	private void SwitchToRAttackState()
	{
		stateMachine.Animator.SetBool("isGuard", true);
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
	private void LockOn()
	{
		if (!stateMachine.Player.IsLockOn)
		{
			// 			// 자동조준을 해제하고 
			// 			stateMachine.AutoTargetting.LockOff();
			// 대상을 찾고
			stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			// lockOn한다.
			//stateMachine.AutoTargetting.LockOn();
		}
		else
		{
			stateMachine.AutoTargetting.LockOff();
			//stateMachine.AutoTargetting.SwitchTarget();
		}

	}
}




