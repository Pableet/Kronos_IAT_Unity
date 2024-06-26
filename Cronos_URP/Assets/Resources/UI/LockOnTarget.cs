using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Camera playerCam;

    public float yUp = 0.0f;
    public float uiScaler = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = playerCam.WorldToScreenPoint(target.position) + new Vector3(0, yUp, 0);
            transform.localScale = new Vector3(uiScaler / transform.position.z, uiScaler / transform.position.z, 0f);
        }
    }
}
