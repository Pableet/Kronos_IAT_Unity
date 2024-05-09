using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// 모든 stateMachine의 기반이되는 SateMachine 클래스
public abstract class StateMachine : MonoBehaviour
{
	private State currentState; // 상태
    
	// 상태를 변경하는 함수
	public void SwitchState(State state)
	{
        // 마우스가 UI를 클릭 중이라면 상태 변경을 막습니다... by mic
        // 이걸 UI 밖 클릭 > UI로 마우스를 옮기면 멈춰버린다...
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;

        currentState?.Exit();   // 현재 상태를 탈출합니다.
        currentState = state;   // 새로운 상태로 변경합니다.
        currentState.Enter();   // 새로운 상태로 돌입합니다.
	}

    // Update is called once per frame
    private void Update()
    {
        // 현재 상태를 점검한다.
        currentState?.Tick();
    }

	public State GetState()	// 현재상태를 반환해준다
	{
		return currentState;
	}


}
