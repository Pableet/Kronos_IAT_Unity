using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	// 필요한 변수

	public float speed;
	float hAxis;
	float vAxis;
	bool wDown;

	Vector3 moveVec;
	Animator anim;

	void Start()
	{
		// anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		hAxis = Input.GetAxisRaw("Horizontal");	// GetAxisRaw는 -,0,+ 값만 가져온다(방향만 가져옴)
		vAxis = Input.GetAxisRaw("Vertical");
		// wDown = Input.GetButton("Walk"); // 동작여부를 받는다.

		// 높이의 대한 이동이 없으니 0을 넣는다.
		moveVec = new Vector3(hAxis, 0, vAxis).normalized;


		transform.position += moveVec * speed */* (wDown ? 1f : 0.5f) **/ Time.deltaTime;

		//anim.SetBool("isWalk", moveVec != Vector3.zero);
		//anim.SetBool("isRun", wDown);

		transform.LookAt(transform.position + moveVec); // 우리가 나아가는 방향으로 바라본다.

	}
}

