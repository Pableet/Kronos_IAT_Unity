using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 모든 stateMachine의 기반이되는 SateMachine 클래스
public abstract class StateMachine : MonoBehaviour
{
	private State currentState; // 상태
    
	// 상태를 변경하는 함수
	public void SwitchState(State state)
	{
		currentState?.Exit();	// 현재 상태를 탈출합니다.
		currentState = state;	// 새로운 상태로 변경합니다.
		currentState.Enter();	// 새로운 상태로 돌입합니다.
	}

    // Update is called once per frame
    private void Update()
    {
		// 현재 상태를 점검한다.
        currentState?.Tick();
    }
}
