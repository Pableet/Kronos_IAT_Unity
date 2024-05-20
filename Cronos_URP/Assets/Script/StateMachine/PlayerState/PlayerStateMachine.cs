using UnityEditor.Tilemaps;
using UnityEditorInternal;

using UnityEngine;
[RequireComponent(typeof(InputReader))]         // 어트리뷰트를 상속받은 
[RequireComponent(typeof(Animator))]            // 사용지정 어트리뷰트 RequireComponenet
[RequireComponent(typeof(CharacterController))] // 해당컴포넌트를 추가해준다
[RequireComponent(typeof(Player))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    public Player Player { get; private set; }
    public Transform MainCamera { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public Transform PlayerTransform { get; private set; }

    public HitStop HitStop { get; private set; }

    private void OnEnable()
    {
    }
    public void Start()
    {
        MainCamera = Camera.main.transform;

        Player = GetComponent<Player>();

        InputReader = GetComponent<InputReader>();
        Animator = GetComponent<Animator>();

        Controller = GetComponent<CharacterController>();
        PlayerTransform = GetComponent<Transform>();
        HitStop = GetComponent<HitStop>();

        // 시작 상태를 정해준다.
        SwitchState(new PlayerMoveState(this));
    }

    void OnSlashEvent()
    {
        EffectManager.Instance.PlayerSlash();
    }

}
