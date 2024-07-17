using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class DodgeBehaviour : StateMachineBehaviour
{
	Vector3 direction;
	private readonly int moveHash = Animator.StringToHash("isMove");
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
		// 상태전환
		PlayerStateMachine.GetInstance().SwitchState(new PlayerParryState(PlayerStateMachine.GetInstance()));

		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = true;

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = false;

		// 키입력이 있다면
		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude > 0f)
		{
			animator.SetBool(moveHash, true);
			direction = PlayerStateMachine.GetInstance().Velocity.normalized;
		}
		else // 없다면
		{
			animator.SetBool(moveHash, false);
			// 카메라의 전방벡터
			Vector3 temp = Camera.main.transform.forward;
			temp.y = 0f;
			direction = temp.normalized;
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = false;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
