using System.Collections.Generic;
using System.Data.Common;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerAttackState : PlayerBaseState
{
	private readonly int subAttack = Animator.StringToHash("Attack");
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int AttackHash1 = Animator.StringToHash("Nor_Attack01"); // 콤보시작
	private readonly int chargeAtdtackHash = Animator.StringToHash("Nor_S_Attack");   // 강화공격

	private const float CrossFadeDuration = 0.1f;

	public float startNormalizedTime = 0.3f;    // 시작 지점
	public float endNormalizedTime = 0.99f;     // 종료 지점

	private bool isEnforcedAttack = false;      // 강화공격 가능
	private bool isEnforcedAttackDone = false;  // 강화공격이 끝남


	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		//stateMachine.Animator.Rebind();
		//stateMachine.Animator.CrossFadeInFixedTime(AttackHash1, CrossFadeDuration);
		//stateMachine.Animator.CrossFadeInFixedTime(subAttack, CrossFadeDuration);

// 		stateMachine.InputReader.onLAttackStart += ReadyNextCombo;
// 		stateMachine.InputReader.onLAttackPerformed += ChargeAttack;
// 		stateMachine.InputReader.onLAttackCanceled += ResetCharge;

		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;

		stateMachine.Player.ChargeAttack = 0f;

		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.SetBool(nextComboHash, false);
	}
	public override void Tick()
	{
		/// 2024.7.4
		/// 인풋시스템이 고장났으니 인풋매니저를 사용한다... ㅠㅠㅠ 
		/// 주말에는 고칠 수 있겠지? 고칠 수 있다고 해줘 해강아
		/// 

// 		if (Input.GetKeyDown(KeyCode.Mouse0))
// 		{
// 			stateMachine.Animator.SetBool(nextComboHash, true);
// 		}
// 		if (Input.GetKeyUp(KeyCode.Mouse0))
// 		{
// 			stateMachine.Player.ChargeAttack = 0f;
// 		}
// //		if (stateMachine.InputReader.IsLAttackPressed) 
// 		if (Input.GetKey(KeyCode.Mouse0))
// 		{
// 			// 차징한다.
// 			stateMachine.Player.ChargeAttack += Time.deltaTime;
// 
// 			if (stateMachine.Player.ChargeAttack >= 0.3f)
// 			{
// 				// 강화공격을 true로 해준다
// 				isEnforcedAttack = true;
// 			}
// 		}

		// 강화공격이 실행가능하다면 (강화공격이 끝나지 않았다면)
		if (isEnforcedAttack && !isEnforcedAttackDone)
		{
			// 강화공격을 실행한다.
			EnforcedAttack();
		}

	}

	public override void FixedTick(){}
	public override void LateTick(){}

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
		Debug.Log("다음공격준비");
		// 다음 콤보를 준비한다.
		stateMachine.Animator.SetBool(nextComboHash, true);
		

	}

	// 강화공격을 실행한다.
	public void EnforcedAttack()
	{
		//stateMachine.Animator.Rebind();
		// 애니메이션을 실행하고
		stateMachine.Animator.CrossFadeInFixedTime(chargeAtdtackHash, 0.1f, -1, 0.3f);
		// 차징을 리셋한다.
		ResetCharge();
		// 강화어택은 끝났다.
		isEnforcedAttackDone = true;
	}

	public void ChargeAttack()
	{
		// 차징시간을 정한다.
		//stateMachine.Player.ChargeAttack += Time.deltaTime;

		// 마우스 왼버튼 눌림을 true로 한다
		stateMachine.InputReader.IsLAttackPressed = true;
	}

	// 차징을 리셋한다.
	public void ResetCharge()
	{
		// 차징을 리셋한다.
		stateMachine.InputReader.IsLAttackPressed = false;
		stateMachine.Player.ChargeAttack = 0f;

		Debug.Log("리셋");
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
}
