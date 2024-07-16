using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

//using UnityEditorInternal;
using UnityEngine;

public class ComboBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int chargeHash = Animator.StringToHash("Charge");

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		animator.SetBool(nextComboHash, false);
		//animator.SetFloat(chargeHash, 0);
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		

		// 이동키입력을 받으면
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// 이동중
			animator.SetBool(moveHash, true);
		}
		else// 혹은
		{
			// 이동아님
			animator.SetBool(moveHash, false);
		}

		// 좌클릭시
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			// NEXTCOMBO 활성화
			animator.SetBool(nextComboHash, true);
		}
		// 좌클릭 누르는 중에는 차징
		if (Input.GetKey(KeyCode.Mouse0))
		{
			float current = animator.GetFloat(chargeHash);
			animator.SetFloat(chargeHash, current + Time.deltaTime);
		}
		// 좌클릭땔때 차징 비활성화
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			animator.SetFloat(chargeHash, 0);
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

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
