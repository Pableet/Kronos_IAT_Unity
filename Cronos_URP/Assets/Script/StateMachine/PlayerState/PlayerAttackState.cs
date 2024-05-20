using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : PlayerBaseState
{
    AnimatorStateInfo stateInfo;

    private readonly int AttackHash1 = Animator.StringToHash("Combo_01_1");
    private readonly int AttackHash2 = Animator.StringToHash("Combo_01_2");
    private readonly int AttackHash3 = Animator.StringToHash("Combo_01_3");
    private readonly int AttackHash4 = Animator.StringToHash("Combo_01_4");
    private const float CrossFadeDuration = 0.1f;

    public float startNormalizedTime = 0.3f;    // 시작 지점
    public float endNormalizedTime = 0.99f;     // 종료 지점

    List<int> comboAttack;

    int comboStack = 0;

    private bool nextCombo = false;
    private bool nextJab = false;
    private bool nextPunch = false;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
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
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
	}
    public override void Tick()
    {
        AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        // 히트스탑 타이밍
        if (stateInfo.normalizedTime >= stateMachine.Player.stopTiming)
        {
            stateMachine.HitStop.isHit = true;
            stateMachine.HitStop.StartCoroutine(stateMachine.HitStop.HitStopCoroutine());
        }
        float testtime = 0.99f;
        switch (comboStack)
        {
            case 1:
                testtime = 0.7f;
                break;
            case 2:
                testtime = 0.6f;
                break;
            case 3:
                testtime = 0.7f;
                break;
            default:
                testtime = 0.99f;
                break;
        }
        if (stateInfo.normalizedTime >= testtime && stateInfo.normalizedTime <= 1.1f)
        {
            // 다음 콤보어택이 예정되어있다면
            if (nextCombo)
            {
                // 새로운 콤보어택을 시전한다.
                NextCombo();

            }
        }
        // 애니메이션이 종료되고
        if (stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }

    }
    public override void Exit()
    {
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
		stateMachine.InputReader.onLAttackStart -= ReadyNextCombo;
    }


    public void ReadyNextCombo()
    {
        // 이 함수가 호출되었을때
        // 다음공격이 예정되어있다면 리턴한다.
        if (nextCombo == true || comboStack == 3)
        {
            return;
        }

        AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= startNormalizedTime && stateInfo.normalizedTime <= endNormalizedTime)
        {
            // 다음 콤보공격을 true로 하고
            nextCombo = true;

            // 콤보스택이 저장되어있는 사이즈보다 작다면
            if (comboStack < comboAttack.Count - 1)
            {
                comboStack++;
            }

        }

    }
    // 새로운 콤보 애니메이션을 시전한다.
    public void NextCombo()
    {
        stateMachine.Animator.Rebind();
        float normalizedStartTime = 0.0f;
        switch (comboStack)
        {
            //             case 1:
            //                 normalizedStartTime = 0.1f; // 애니메이션을 0.3의 시점에서 시작
            //                 break;
            case 2:
                normalizedStartTime = 0.2f; // 애니메이션을 0.3의 시점에서 시작
                break;
            case 3:
                normalizedStartTime = 0.3f; // 애니메이션을 0.3의 시점에서 시작
                break;
        }

        stateMachine.Animator.CrossFadeInFixedTime(comboAttack[comboStack], 0.1f, -1, normalizedStartTime);
        
        nextCombo = false;
    }


	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}


}
