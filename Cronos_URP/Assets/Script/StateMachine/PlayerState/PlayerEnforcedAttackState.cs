using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerEnforcedAttackState : PlayerBaseState
{
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int AttackHash1 = Animator.StringToHash("Com_Attack_01");
	private readonly int chargeAtdtackHash = Animator.StringToHash("Com_S Attack");   // 강화공격

	private const float CrossFadeDuration = 0.1f;

	private float chargeAttack = 0f;

	public float startNormalizedTime = 0.3f;    // 시작 지점
	public float endNormalizedTime = 0.99f;     // 종료 지점

	private bool isEnforcedAttack = false;      // 강화공격 가능
	private bool isEnforcedAttackDone = false;  // 강화공격이 끝남


	public PlayerEnforcedAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(AttackHash1, CrossFadeDuration);

// 		stateMachine.InputReader.onLAttackStart += ReadyNextCombo;
// 		stateMachine.InputReader.onLAttackPerformed += ChargeAttack;
// 		stateMachine.InputReader.onLAttackCanceled += ResetCharge;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;

		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		/// 2024.7.4
		/// 인풋시스템이 고장났으니 인풋매니저를 사용한다... ㅠㅠㅠ 
		/// 주말에는 고칠 수 있겠지? 고칠 수 있다고 해줘 해강아
		/// 

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			stateMachine.Animator.SetBool(nextComboHash, true);
		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			stateMachine.Player.ChargeAttack = 0f;
		}
		//		if (stateMachine.InputReader.IsLAttackPressed) 
		if (Input.GetKey(KeyCode.Mouse0))
		{
			// 차징한다.
			stateMachine.Player.ChargeAttack += Time.deltaTime;

			if (stateMachine.Player.ChargeAttack >= 0.3f)
			{
				// 강화공격을 true로 해준다
				isEnforcedAttack = true;
			}
		}

		// 강화공격이 실행가능하다면 (강화공격이 끝나지 않았다면)
		if (isEnforcedAttack && !isEnforcedAttackDone)
		{
			// 강화공격을 실행한다.
			EnforcedAttack();
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
		//stateMachine.InputReader.onLAttackStart -= ReadyNextCombo;
		//stateMachine.InputReader.onLAttackPerformed -= ChargeAttack;
		//stateMachine.InputReader.onLAttackCanceled -= ResetCharge;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;

	}

	// 다음콤보를 준비한다.
	public void ReadyNextCombo()
	{
		// 다음 콤보를 준비한다.
		stateMachine.Animator.SetBool(nextComboHash, true);
	}

	// 강화공격을 실행한다.
	public void EnforcedAttack()
	{
		//stateMachine.Animator.Rebind();
		//float normalizedStartTime = 0.0f;
		// 애니메이션을 실행하고
		stateMachine.Animator.CrossFadeInFixedTime(chargeAtdtackHash, 0.1f, -1, 0.3F);
		// 차징을 리셋한다.
		ResetCharge();
		// 강화어택은 끝났다.
		isEnforcedAttackDone = true;
	}

	public void ChargeAttack()
	{
		// 차징시간을 정한다.
		chargeAttack += Time.deltaTime;

		// 마우스 왼버튼 눌림을 true로 한다
		stateMachine.InputReader.IsLAttackPressed = true;
	}

	// 차징을 리셋한다.
	public void ResetCharge()
	{
		// 마우스 클릭을 false로 한다.
		stateMachine.InputReader.IsLAttackPressed = false;
		// 차징을 리셋한다.
		chargeAttack = 0f;
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
}
