using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforcedComboBehaviour : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool("NextCombo", false);
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
		{
			// nextcombo 가 예정되어있다면
			if (animator.GetBool("NextCombo"))
			{
				// 리턴
				return;
			}
			else // 그렇지 않다면
			{
				PlayerStateMachine.GetInstance().SwitchState(new PlayerMoveState(PlayerStateMachine.GetInstance()));
				return;
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// 		// nextcombo 가 예정되어있다면
		// 		if (animator.GetBool("NextCombo"))
		// 		{
		// 			// 리턴
		// 			return;
		// 		}
		// 		else // 그렇지 않다면
		// 		{
		// 			PlayerStateMachine.GetInstance().SwitchState(new PlayerBuffState(PlayerStateMachine.GetInstance()));
		// 			return;
		// 		}
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
