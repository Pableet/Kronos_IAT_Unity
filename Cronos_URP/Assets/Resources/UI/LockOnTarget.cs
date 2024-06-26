using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Camera playerCam;
    [SerializeField]
    GameObject targetUI;
    [SerializeField]
    Player player;

    AutoTargetting atTgt;
    bool isTgt;

    public float yUp = 0.0f;
    public float uiScaler = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        atTgt = AutoTargetting.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        target = atTgt.GetTarget();
        isTgt = player.IsLockOn;

        if (target && isTgt)
        {
            targetUI.SetActive(true);
            targetUI.transform.position = playerCam.WorldToScreenPoint(target.position) + new Vector3(0, yUp, 0);
            targetUI.transform.localScale = new Vector3(uiScaler / targetUI.transform.position.z, uiScaler / targetUI.transform.position.z, 0f);
        }
        else
        {
            targetUI.SetActive(false);
        }
    }

    // 일단 적의 자식 오브젝트 중 Spine을 찾아서 그 트랜스폼을 뱉는다.
    //Transform SearchSpine(Transform transform)
    //{



    //}
}
