using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	// 필요한 변수
	public float speed;
	float hAxis;
	float vAxis;
	bool wRun;
	Transform PlayerCAMRoot;	// 카메라루트를 담을 ? 
	Animator m_animator;		// 애니메이터정보를 가져와서 반영하자

	Vector3 moveVec;
	Animator anim;

	private void Awake()
	{
		// 플레이어 카메라의 트랜스폼을 가져온다.
		PlayerCAMRoot = GetComponentsInChildren<Transform>()[0];
	}

	void Start()
	{
		anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		// GetAxisRaw는 -,0,+ 값만 가져온다(방향만 가져옴)
		hAxis = Input.GetAxisRaw("Horizontal");	// x축에 수평으로 움직인다.
		vAxis = Input.GetAxisRaw("Vertical");	// x축에 수직으로 움직인다.

		wRun = Input.GetButton("Run"); // 동작여부를 받는다.

		// 높이의 대한 이동이 없으니 0을 넣는다.
		moveVec = new Vector3(hAxis, 0, vAxis).normalized;


		transform.position += moveVec * speed */* (wDown ? 1f : 0.5f) **/ Time.deltaTime;

		//anim.SetBool("isWalk", moveVec != Vector3.zero);
		//anim.SetBool("isRun", wDown);

		transform.LookAt(transform.position + moveVec); // 우리가 나아가는 방향으로 바라본다.

	}
}

