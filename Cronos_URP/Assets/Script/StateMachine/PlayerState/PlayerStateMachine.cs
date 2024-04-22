using UnityEngine;
[RequireComponent(typeof(InputReader))]         // 어트리뷰트를 상속받은 
[RequireComponent(typeof(Animator))]            // 사용지정 어트리뷰트 RequireComponenet
[RequireComponent(typeof(CharacterController))] // 해당컴포넌트를 추가해준다
public class PlayerStateMachine : StateMachine
{
	public Vector3 Velocity;
	public float MovementSpeed { get; private set; } = 5f;
	public float JumpForce { get; private set; } = 10f;
	public float LookRotationDampFactor { get; private set; } = 10f;

	public Transform MainCamera { get; private set; }
	public InputReader InputReader { get; private set; }
	public Animator Animator { get; private set; }
	public CharacterController Controller { get; private set;}

	public Transform PlayerTransform { get; private set; }

	private void Start()
	{
		MainCamera = Camera.main.transform;

		InputReader = GetComponent<InputReader>();
		Animator = GetComponent<Animator>();
		
		Controller = GetComponent<CharacterController>();
		PlayerTransform = GetComponent<Transform>();

		// 시작 상태를 정해준다.
		//SwitchState(new PlayerMoveState(this));
		SwitchState(new PlayerIdleState(this));
	}
}
