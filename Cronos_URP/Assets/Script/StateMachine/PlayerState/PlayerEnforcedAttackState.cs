using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerEnforcedAttackState : PlayerBaseState
{
	AnimatorStateInfo animatorStateInfo;
	private readonly int AttackHash1 = Animator.StringToHash("Attack_3Combo_1");
	private readonly int AttackHash2 = Animator.StringToHash("Attack_3Combo_2");
	private readonly int AttackHash3 = Animator.StringToHash("Attack_3Combo_3");
	private readonly int AttackHash4 = Animator.StringToHash("Attack_4Combo_1B");

	private readonly int chargeAtdtackHash = Animator.StringToHash("Combo_03_4");   // 강화공격

	float normalizedTime = 0f;

	private const float CrossFadeDuration = 0.1f;

	private float chargeAttack = 0f;

	public float startNormalizedTime = 0.3f;    // 시작 지점
	public float endNormalizedTime = 0.99f;     // 종료 지점

	List<int> comboAttack;

	private int comboStack = 0;
	private float exitTime = 0.3f;
	private float duration = 0f;
	private float offset = 0f;

	private bool nextCombo = false;
	private bool isEnforcedAttack = false;      // 강화공격 가능
	private bool isEnforcedAttackDone = false;  // 강화공격이 끝남


	public PlayerEnforcedAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		// 콤보 해쉬리스트
		comboAttack = new List<int>();
		// 애니메이션 해쉬들을 추가한다.
		comboAttack.Add(AttackHash1);
		comboAttack.Add(AttackHash2);
		comboAttack.Add(AttackHash3);
		comboAttack.Add(AttackHash4);

		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(comboAttack[comboStack], CrossFadeDuration);
		// 현재 애니메이션 정보를 받아온다

		stateMachine.InputReader.onLAttackStart += ReadyNextCombo;
		stateMachine.InputReader.onLAttackPerformed += ChargeAttack;
		stateMachine.InputReader.onLAttackCanceled += ResetCharge;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;

		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		// 진행정도를 얻어오자
		animatorStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
		normalizedTime = animatorStateInfo.normalizedTime;

		switch (comboStack)
		{
			case 0:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
			case 1:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
				exitTime = 0.75f;
				duration = 0.25f;
				offset = 0f;
				break;
			case 2:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
				exitTime = 0.75f;
				duration = 0.25f;
				offset = 0f;
				break;
			case 3:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
				exitTime = 0f;
				duration = 31f/33f;
				offset = 0f;
				break;
		}

		// 마우스가 눌려있으면
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			// 차징한다.
			chargeAttack += Time.deltaTime;

			if (chargeAttack > 0.3f)
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
			// 강화공격을 했다면 다음 콤보공격은 들어가지 않는다.
			nextCombo = false;
		}
		// 콤보가 예정되어있고
		// 진행정도가 ~~ 이상이라면
		else if (nextCombo && normalizedTime > exitTime + duration)
		{
			// 새로운 콤보어택을 시전한다.
			NextCombo();
		}


		if (normalizedTime >= 1.0f)
		{
			//stateMachine.SwitchState(new PlayerMoveState(stateMachine));
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
		stateMachine.InputReader.onLAttackStart -= ReadyNextCombo;
		stateMachine.InputReader.onLAttackPerformed -= ChargeAttack;
		stateMachine.InputReader.onLAttackCanceled -= ResetCharge;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;

	}

	// 다음콤보를 준비한다.
	public void ReadyNextCombo()
	{
		// 다음 공격이 예정되어있다면 리턴한다.
		if (nextCombo == true || comboStack == 3)
		{
			return;
		}

		// 만약.. 라면
		// 
		if (normalizedTime > exitTime && normalizedTime < 0.7f)
		{
			// 다음 콤보공격을 true로 한다. 
			nextCombo = true;
		}


	}
	// 강화공격을 실행한다.
	public void EnforcedAttack()
	{
		stateMachine.Animator.Rebind();
		float normalizedStartTime = 0.0f;
		// 애니메이션을 실행하고
		stateMachine.Animator.CrossFadeInFixedTime(chargeAtdtackHash, 0.1f, -1, normalizedStartTime);
		// 차징을 리셋한다.
		ResetCharge();
		// 강화어택은 끝났다.
		isEnforcedAttackDone = true;
	}

	/// 새로운 콤보 애니메이션을 시전한다.
	public void NextCombo()
	{
		// 콤보스택이 저장되어있는 사이즈보다 작다면
		if (comboStack < comboAttack.Count - 1)
		{
			// 콤보 스택을 올린다.
			comboStack++;
		}

		// 콤보스택에 맞는 콤보 애니메이션을 실행한다.
		stateMachine.Animator.CrossFade(comboAttack[comboStack], offset, -1, 0f);
		// 타겟이 있으면
		if (stateMachine.AutoTargetting.Target != null)
		{
			// 타겟 쪽으로 이동하면서 공격
			stateMachine.Rigidbody.AddForce((stateMachine.AutoTargetting.Target.position - stateMachine.transform.position).normalized * 2f);
		}
		// 애니메이션을 실행했다면 콤보 진행을 멈춘다.
		nextCombo = false;

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
