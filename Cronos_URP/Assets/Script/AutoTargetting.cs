using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal.Internal;
using System.Data.SqlTypes;
public class AutoTargetting : MonoBehaviour
{

    public CinemachineFreeLook freeLookCamera;
    public float horizontalSpeed = 10.0f; // 수평 회전 속도
    public float verticalSpeed = 5.0f;    // 수직 회전 속도

    public GameObject Cam;
    public GameObject Target;       // Player가 바라볼 대상
    public GameObject Player;       // 플레이어
    public GameObject PlayerObject; // 플레이어 오브젝트 

    public float AixsDamp = 0.99f;  // 어느정도까지 따라갈 것인가!

    Camera mainCam;

    Transform targetTransfrom;
    Transform PlayerObjectTransfrom;
    Transform maincamTransform;

    PlayerStateMachine stateMachine;

    float xDotResult;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        stateMachine = Player.GetComponent<PlayerStateMachine>();

        targetTransfrom = Target.GetComponent<Transform>();
        PlayerObjectTransfrom = PlayerObject.GetComponent<Transform>();
        maincamTransform = mainCam.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 캐릭터가 바라볼 방향을 정한다.
        Vector3 direction = targetTransfrom.position - PlayerObjectTransfrom.position;
        direction.y = 0;    // y축으로는 회전하지 않는다.

        xDotResult = Vector3.Dot(maincamTransform.right, PlayerObjectTransfrom.right);

        // 공격이 일어났을때 캐릭터가 몬스터 방향으로 몸을 돌린다.
        if (Input.GetButton("Fire1"))
        {
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 1f);

            float targetPos = TransformPosition(maincamTransform, targetTransfrom.position).x;

            if (targetPos > 0)
            {
                // 카메라가 플레이어 뒤에서 몬스터를 바라본다
                if (xDotResult < AixsDamp)
                {
                    TurnCam(horizontalSpeed * Time.deltaTime);
                }
            }
            else // 오른쪽에 있다면
            {
                if (xDotResult < AixsDamp)
                {
                    TurnCam(horizontalSpeed * Time.deltaTime * -1f);
                }
            }
        }
    }

    // 카메라를 돌린다
    private void TurnCam(float value)
    {
        freeLookCamera.m_XAxis.Value += value;
    }

    // 카메라위치에서 타겟을 바라본다
    private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
    {
        return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
    }

}
