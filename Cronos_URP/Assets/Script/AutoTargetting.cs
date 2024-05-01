using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal.Internal;
using System.Data.SqlTypes;
using UnityEngine.Purchasing.Extension;
using Unity.Mathematics;
public class AutoTargetting : MonoBehaviour
{

	public CinemachineFreeLook freeLookCamera;
	public float horizontalSpeed = 10.0f; // 수평 회전 속도
	public float verticalSpeed = 5.0f;    // 수직 회전 속도

	public GameObject Player;       // 플레이어
	public Transform Target;       // Player가 바라볼 대상
	public Transform PlayerObject; // 플레이어 오브젝트 
	public Transform maincamTransform;

	public float AixsDamp = 0.99f;  // 어느정도까지 따라갈 것인가!

	private PlayerStateMachine stateMachine;
	private MonsterSelector monsterSelector;

	private Vector3 direction;
	private float xDotResult;
	bool istargetting;

	// Start is called before the first frame update
	void Start()
	{
		stateMachine = Player.GetComponent<PlayerStateMachine>();

	}

	// Update is called once per frame
	void Update()
	{
		if (monsterSelector == null)
		{
			monsterSelector = GetComponent<MonsterSelector>();

		}

		// Player가 바라볼 방향을 정한다.
		direction = Target.position - PlayerObject.position;
		direction.y = 0;    // y축으로는 회전하지 않는다.

		xDotResult = Vector3.Dot(maincamTransform.right, PlayerObject.right);

		// 공격이 일어났을때 
		if (Input.GetButton("Fire1"))
		{
			istargetting = true;
			monsterSelector.FindTarget();
		}
		if (istargetting)
		{
			AutoTarget();
		}
	}

	private void AutoTarget()
	{
		// Player가 몬스터 방향으로 몸을 돌린다.
		stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 1f);

		// 타겟이 Player보다 왼쪽에 있는지 오른쪽에 있는지 검사한다.
		float targetPos = TransformPosition(maincamTransform, Target.position).x;
		if (xDotResult < AixsDamp)
		{
			// targetpos로 좌우를 구분해서 돌린다.
			TurnCam(horizontalSpeed * Time.deltaTime * (targetPos/math.abs(targetPos)));
		}
		else
		{
			istargetting = false;
		}
	}

	// 카메라를 돌린다
	private void TurnCam(float value)
	{
		freeLookCamera.m_XAxis.Value += value;
	}

	// 특정 포지션을 특정 트렌스폼에서 바라본다.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}

}
