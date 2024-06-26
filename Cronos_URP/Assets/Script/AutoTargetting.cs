using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal.Internal;
using System.Data.SqlTypes;
using UnityEngine.Purchasing.Extension;
using Unity.Mathematics;
using System.Security;
using System.Data;
public class AutoTargetting : MonoBehaviour
{
	static AutoTargetting instance;
	public static AutoTargetting GetInstance() { return instance; }

	//public CinemachineFreeLook freeLookCamera;
	public CinemachineVirtualCamera PlayerCamera;
	public CinemachinePOV CinemachinePOV;

	public float horizontalSpeed = 10.0f; // 수평 회전 속도
	public float verticalSpeed = 3f;    // 수직 회전 속도

	public GameObject Player;       // 플레이어
	public Transform Target = null;     // Player가 바라볼 대상
	public Transform PlayerObject; // 플레이어 오브젝트 
	/*public */
	Transform maincamTransform;

	public float AixsDamp = 0.99f;  // 어느정도까지 따라갈 것인가!

	private PlayerStateMachine stateMachine;

	private Vector3 direction;
	private float xDotResult;
	private float yDotResult;
	bool isTargetting;


	// 몬스터리스트
	List<GameObject> MonsterList;
	float? min = null;

	private void Awake()
	{
		instance = this;
		MonsterList = new List<GameObject>();
	}

	public Transform GetTarget()
	{
		return Target;
	}

	// Start is called before the first frame update
	public void OnEnable()
	{
		stateMachine = Player.GetComponent<PlayerStateMachine>();
		maincamTransform = Camera.main.transform;
		CinemachinePOV = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
	}

	private void OnTriggerEnter(Collider other)
	{
		// 콜라이더에 몬스터가 들어오면 리스트에 추가한다.
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Add(FindChildRecursive(other.gameObject.transform, "Spine1").gameObject);
			//MonsterList.Add(other.gameObject);
			Debug.Log("monster in");
		}

	}

    Transform FindChildRecursive(Transform parent, string childName)
    {
        // 현재 부모의 하위 오브젝트들을 순회합니다.
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            // 재귀적으로 하위 오브젝트들을 탐색합니다.
            Transform found = FindChildRecursive(child, childName);
            if (found != null)
            {
                return found;
            }
        }

        // 이름에 맞는 하위 오브젝트를 찾지 못하면 null을 반환합니다.
        return null;
    }

    private void OnTriggerExit(Collider other)
	{
		// 콜라이더에서 몬스터가 나가면 리스트에서 제거한다.
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Remove(other.gameObject);
			Debug.Log("monster out");
		}
	}

	// Update is called once per frame
	void Update()
	{

		// Player가 바라볼 방향을 정한다.
		if (Target != null)
		{
			direction = Target.position - PlayerObject.position;
			direction.y = 0;
		}

		xDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.right, PlayerObject.right));
		yDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.up, Vector3.Cross(Vector3.right, direction.normalized).normalized));

		/// 공격이 일어났을때 
		/// 제대로 쓸거면 inputsystem을 사용하는 방식으로 고치자
		if (Input.GetButton("Fire1"))
		{
			FindTarget();

			if (Target == null)
			{
				return;
			}
			else
			{
				isTargetting = true;
			}
		}
		else if (!stateMachine.Player.IsLockOn)
		{
			Target = null;
			isTargetting = false;
		}

		if (isTargetting)
		{
			AutoTarget();
		}
	}
	private void FixedUpdate()
	{
		// Player가 몬스터 방향으로 몸을 돌린다.
		if (isTargetting || stateMachine.Player.IsLockOn)
		{
			stateMachine.Rigidbody.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.1f);

		}
	}
	private void LateUpdate() { }

	private void AutoTarget()
	{
		// 타겟이 Player보다 왼쪽에 있는지 오른쪽에 있는지 검사한다.
		if (Target != null)
		{
			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);

			// 내적값이 99 보다 작으면 더한다.
			if (Mathf.Abs(xDotResult) < AixsDamp)
			{
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1 - xDotResult));
			}
			else
			{
				isTargetting = false;
			}
		}

	}

	// 카메라를 돌린다
	private void TurnCamHorizontal(float value)
	{
		CinemachinePOV.m_HorizontalAxis.Value += value;

	}
	private void TurnCamVertical(float value)
	{
		CinemachinePOV.m_VerticalAxis.Value -= value;
	}

	// 특정 포지션을 특정 트렌스폼에서 바라본다.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}
	public bool FindTarget()
	{
		// 몬스터리스트가 없거나 사이즈가 0이라면 false
		if (MonsterList == null || MonsterList.Count == 0)
		{
			return false;
		}
		// 현재 몬스터 목록을 순회한다.
		for (int i = 0; i < MonsterList.Count; i++)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.Remove(MonsterList[i]);
			}
			else
			{
				// 몬스터와 플레이어 사이의 거리벡터의 크기를 구한다.
				/// 몬스터의 트랜스폼을 
				/// 매번 컴포넌트 접근으로 찾아야하는 점은 
				/// 너무 아쉬운 일이다. 방법을 찾아볼까?
				/// 1. findTarget기능의 위치를 바꾼다.
				float value = Mathf.Abs((PlayerObject.position - MonsterList[i].GetComponent<Transform>().position).magnitude);

				// 현재 min값보다 value가 작다면 혹은 min에 값이 들어있지 않다면 
				if (min > value || min == null)
				{
					// min 값을 교체하고 
					min = value;

					// 가장 작은 값을 가진 트랜스폼을 타겟으로 설정한다.
					Target = MonsterList[i].GetComponent<Transform>();
				}
			}

		}

		min = null;
		return true;
	}

	public void LockOn()
	{
		// 타겟이 Player보다 왼쪽에 있는지 오른쪽에 있는지 검사한다.
		if (Target != null)
		{
			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);

			// 내적값이 99 보다 작으면 더한다.
			if (xDotResult < AixsDamp)
			{
				// targetpos로 좌우를 구분해서 돌린다.
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1f - xDotResult));
			}
			if (yDotResult < AixsDamp)
			{
				TurnCamVertical(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)) * (1f - yDotResult));
			}
		}

	}

	public void LockOff()
	{
		// 락온을 해제한다.
		Target = null;
	}

	public void SwitchTarget()
	{

		// 몬스터리스트를 거리 순으로 정렬한다.
		MonsterList.Sort((x, y) =>
			(PlayerObject.position - x.transform.position).sqrMagnitude
			.CompareTo((PlayerObject.position - y.transform.position).sqrMagnitude));

		// 정렬된 몬스터리스트에서 가장 가까운 몬스터를 Target과 비교한다
		if (MonsterList.Count > 0)
		{
			// 정렬된 가장 가까운몬스터와 target이 일치하면 다음으로 가까운 몬스터를 타겟팅한다.
			if (Target != MonsterList[0].transform)
			{
				Target = MonsterList[0].transform;
			}
			else
			{
				Target = MonsterList[1]?.transform;
			}
		}
	}

}
