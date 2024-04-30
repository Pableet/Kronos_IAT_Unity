using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterSelector : MonoBehaviour
{

	public Transform Monsetr0;
	public Transform Monsetr1;
	public Transform Monsetr2;
	public Transform Monsetr3;

	// 몬스터를 관리할 목록
	List<Transform> monsters = new List<Transform>();

	// 오토 타겟에 그걸 해주자
	public AutoTargetting autoTargetor;


	float? min = null;
	int indexNum;


	// Start is called before the first frame update
	void Start()
	{
		// 일단 담아
		monsters.Add(Monsetr0);
		monsters.Add(Monsetr1);
		monsters.Add(Monsetr2);
		monsters.Add(Monsetr3);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			FindTarget();

		}
	}

	public void FindTarget()
	{
		// 현재 몬스터 목록을 순회한다.
		for (int i = 0; i < monsters.Count; i++)
		{
			// 몬스터와 플레이어 사이의 거리벡터의 크기를 구한다.
			float value = Mathf.Abs((autoTargetor.PlayerObject.position - monsters[i].position).magnitude);

			// 현재 min값보다 value가 작다면 혹은 min에 값이 들어있지 않다면 
			if (min > value || min == null)
			{
				// min 값을 교체하고 
				/// 문제발견, 한 번 min으로 등록된 값이 새로 바뀌지 않는다.
				min = value;
				// 가장 작은 값을 가진 트랜스폼을 타겟으로 설정한다.
				autoTargetor.Target = monsters[i];
			}
		}

		min = null;
	}
}
