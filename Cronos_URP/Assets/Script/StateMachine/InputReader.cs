using System;
using UnityEditor.Build;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

/// <summary>
/// 이 코드는 Unity 게임 엔진을 사용하여 플레이어 입력을 관리하는 클래스를 정의합니다.
/// InputReader 클래스는 Unity의 새로운 InputSystem을 활용하여 마우스와 키보드 입력을 처리합니다.
/// 
/// Player의 인풋 액션의 대한 트리거를 정의 합니다.
/// </summary>
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
	public Vector2 mouseDelta; // 마우스 이동정보를 받아온다
	public Vector2 moveComposite;

	public Action onMove;
	public Action onJumpPerformed;		// 점프의 대한 액션을 담기 위한 변수
	public Action onLAttackPerformed;	// 공격의 대한 액션을 담기 위한 변수
	public Action onRAttackPerformed;	// 공격의 대한 액션을 담기 위한 변수
	
	private Controls controls;

	private void OnEnable()
	{
		if(controls != null)
		{
			return;
		}
		controls = new Controls();
		controls.Player.SetCallbacks(this); // InputReader는 IPlayerActions를 상속받았다.
											// Actions을 세팅한다.
		controls.Player.Enable();		// 사용가능한 형태로 만든다.
	}

	public void OnDisable()
	{
		// 플레이어의 disable 함수를 호출한다.
		controls.Player.Disable();
	}

	// 카메라 이동을 담당하는! 
	public void OnLook(InputAction.CallbackContext context)
	{
		mouseDelta = context.ReadValue<Vector2>();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveComposite = context.ReadValue<Vector2>();
		onMove?.Invoke(); // 이동 발생여부를 검증한다.
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if(!context.performed)
		{
			return;
		}

		onJumpPerformed?.Invoke();// onJump가 null 이 아니라면 실행한다.
	}
	public void OnLAttack(InputAction.CallbackContext context)
	{
		onLAttackPerformed?.Invoke();
	}
	public void OnRAttack(InputAction.CallbackContext context)
	{
		onRAttackPerformed?.Invoke();
	}


}
