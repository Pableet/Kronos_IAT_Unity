using System.Globalization;
using System.Resources;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public abstract class PlayerBaseState : State
{
	// 변수를 읽기전용으로 선언
	protected readonly PlayerStateMachine stateMachine;

	protected PlayerBaseState(PlayerStateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	/// <summary>
	/// 카메라 정보를 기반으로
	/// Player의 이동방향을 구한다.
	/// </summary>
	protected void CalculateMoveDirection()
	{
		// 스테이트 머신에 들어있는 카메라 정보를 기반으로
		// 카메라의 전방, 좌우 벡터를 저장한다.
		Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
		Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

		// 이동벡터생성,
		// 카메라의 전방벡터에 인풋의 move.y 수치를 곱한다,
		// 카메라의 좌우벡터에 인풋의 movecomposite.x를 곱한다.
		Vector3 moveDirection	= cameraForward.normalized * stateMachine.InputReader.moveComposite.y	// 전방
								+ cameraRight.normalized * stateMachine.InputReader.moveComposite.x;	// 후방

		// 상태머신의 속도는 이동벡터와 속력의 곱이다.
		stateMachine.Velocity.x = moveDirection.x * stateMachine.MovementSpeed;
		stateMachine.Velocity.z = moveDirection.z * stateMachine.MovementSpeed;
	}

	/// <summary>
	/// 플레이어를 이동방향으로 회전시킨다.
	/// </summary>
	protected void FaceMoveDirection()
	{

		Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

		// 이동속도가 없다면
		if (faceDirection == Vector3.zero)
		{
			// 아무것도 하지 않겠다.
			return;
		}
		// 플레이어의 회전은 구면 선형보간의 형태로 이루어진다. 
		stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRotationDampFactor * Time.deltaTime);
	}

	/// <summary>
	/// 중력을 적용한다.
	/// 좀 잘 모르겠음
	/// </summary>
	protected void ApplyGravity()
	{
		// 플레이어의 Y축으로 향하는 힘이
		// 중력보다 강할 때
		if (stateMachine.Velocity.y > Physics.gravity.y)
		{
			// 플레이어에 역중력을 적용한다.
			stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
		}
	}

	/// <summary>
	/// Player가 갖고 있는 데이터를 종합해서
	/// Player를 이동시킨다
	/// </summary>
	protected void Move()
	{
		// CharacterController컴포넌트를 이용해서 캐릭터를 이동시킨다.
		stateMachine.Controller.Move(stateMachine.Velocity * Time.deltaTime);

	}


}