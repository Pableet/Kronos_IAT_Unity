using UnityEditor.Tilemaps;
using UnityEditorInternal;

using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(InputReader))]         // 어트리뷰트를 상속받은 
[RequireComponent(typeof(Animator))]            // 사용지정 어트리뷰트 RequireComponenet
[RequireComponent(typeof(Rigidbody))] // 해당컴포넌트를 추가해준다

public class PlayerStateMachine : StateMachine
{
	

    public Vector3 Velocity;
    public Player Player { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
	public Rigidbody Rigidbody { get; private set; }
    public Transform MainCamera { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public HitStop HitStop { get; private set; }

//  	[field: Header("Collisions")]
//  	[SerializeField] public CapsuleColliderUtility colliderUtility { get; private set; }
//  	[SerializeField] public PlayerLayerData LayerData{ get; private set; }
// 
// 	private void Awake()
// 	{
// 		colliderUtility.Initialize(gameObject);
// 		colliderUtility.CalculateCapsulcolliderDimentsions();
// 	}
// 	private void OnValidate()
// 	{
// 		colliderUtility.Initialize(gameObject);
// 		colliderUtility.CalculateCapsulcolliderDimentsions();
// 	}

	public void OnEnable()
    {

        Player = GetComponent<Player>();
        InputReader = GetComponent<InputReader>();
		Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();

        MainCamera = Camera.main.transform;
        PlayerTransform = GetComponent<Transform>();
        HitStop = GetComponent<HitStop>();


		// 시작 상태를 정해준다.
		SwitchState(new PlayerMoveState(this));
	}

	void OnSlashEvent()
    {
        EffectManager.Instance.PlayerSlash();
    }
	void OnDrawGizmos()
	{
		// 시각적으로 Ray를 확인하기 위한 Gizmo
		if (transform.position != null)
		{
			Gizmos.color = Color.red;
			Vector3 offset = new Vector3(0f, 1f, 0f);
			Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.down);
		}
	}
}
