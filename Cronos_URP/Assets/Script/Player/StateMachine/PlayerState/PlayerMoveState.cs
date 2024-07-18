using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

// �÷��̾� �⺻���¸� ��ӹ��� movestate
public class PlayerMoveState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int SideWalkHash = Animator.StringToHash("SideWalk");
	private readonly int moveXHash = Animator.StringToHash("moveX");
	private readonly int moveYHash = Animator.StringToHash("moveY");
	private const float AnimationDampTime = 0.1f;

	float moveSpeed = 0.5f;
	public float targetSpeed = 0.5f;

	float releaseLockOn = 0f;

	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.InputReader.onSwitchingStart += Deceleration;
	}

	// state�� update�� �� �� ����
	public override void Tick()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			stateMachine.Player.CP += 1f;
		}

		// �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		//moveSpeed = 0.5f;
		if (Input.GetButton("Run"))
		{
			moveSpeed = 1f;
		}
		else
		{
			stateMachine.StartCoroutine(SmoothChangeSpeed());
			//moveSpeed = 0.5f; 
		}

		stateMachine.Player.SetSpeed(moveSpeed);

		if (Input.GetMouseButtonDown(2))
		{
			// ���� ���°� �ƴ϶��
			if (!stateMachine.Player.IsLockOn)
			{
				// ����� ã��
				stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			}
			// ���»��¶�� ������ �����Ѵ�.
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

		// �ִϸ����� movespeed�� �Ķ������ ���� ���Ѵ�.
		// ���� �����϶� && �޸��Ⱑ �ƴҶ�
		if (stateMachine.Player.IsLockOn && moveSpeed < 0.6f)
		{
			// moveSpeed�� y�������ؼ� �����̵����� �Ĺ��̵����� Ȯ���Ѵ�.
			stateMachine.Animator.SetFloat(MoveSpeedHash,
											/*Mathf.Abs(stateMachine.InputReader.moveComposite.y) > 0f ? moveSpeed :*/ 
											(moveSpeed * stateMachine.InputReader.moveComposite.y), AnimationDampTime, Time.deltaTime);
		}
		else
		{
			stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);
		}

		if (stateMachine.Player.IsLockOn && moveSpeed < 0.7f)
		{
			float side = 0f;
			side = stateMachine.InputReader.moveComposite.x * 0.75f;
			stateMachine.Animator.SetFloat(SideWalkHash, side, AnimationDampTime, Time.deltaTime);
		}
		else
		{
			stateMachine.Animator.SetFloat(moveXHash, stateMachine.InputReader.moveComposite.x, AnimationDampTime, Time.deltaTime);
			stateMachine.Animator.SetFloat(moveYHash, stateMachine.InputReader.moveComposite.y, AnimationDampTime, Time.deltaTime);
			stateMachine.Animator.SetFloat(SideWalkHash, stateMachine.InputReader.moveComposite.x, AnimationDampTime, Time.deltaTime);
		}
		CalculateMoveDirection();   // ������ ����ϰ�

	}
	public override void FixedTick()
	{

		if (stateMachine.Player.IsLockOn)
		{
			if (moveSpeed > 0.5f)
			{
				FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
			}
		}
		else
		{
			FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
		}
		Move();                     // �̵��Ѵ�.	
	}

	public override void LateTick() { }

	public override void Exit()
	{
		stateMachine.InputReader.onSwitchingStart -= Deceleration;
	}

	private void Deceleration()
	{
		if (stateMachine.Player.CP >= 100)
		{
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}

	}

	// �� ��ȭ�� �ε巴�� ����
	IEnumerator SmoothChangeSpeed()
	{
		float startSpeed = moveSpeed;
		float elapsedTime = 0.0f;

		while (elapsedTime < 0.1f)
		{
			moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / 1f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		moveSpeed = targetSpeed; // Ensure it reaches the target value at the end
	}


}



