using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class BulletTime : MonoBehaviour
{
    [SerializeField]
    private float currentSpeed = 0f;

    public float targetSpeed = 1f;
    public float acceleration = 1f;
    public float deceleration = 1f;

    private static BulletTime _instance;

    public static BulletTime Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BulletTime>();
                if (_instance == null)
                {
                    GameObject effectManager = new GameObject(typeof(BulletTime).Name);
                    _instance = effectManager.AddComponent<BulletTime>();

                    DontDestroyOnLoad(effectManager);
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        currentSpeed += acceleration * 1f;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
        }
    }

    private void LateUpdate()
    {
        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, targetSpeed); // �ִ� �ӵ� �ʰ� ����

        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed *= (1f - Time.deltaTime * deceleration);
            if (currentSpeed < 0.01f)
            {
                currentSpeed = 0.01f;
            }
            //currentSpeed = Mathf.Max(currentSpeed, maxSpeed); // �ּ� �ӵ� �̸� ����
        }

    }
    public void DecelerateSpeed()
    {
        targetSpeed = 0.01f;
    }

    public void SetNormalSpeed()
    {
        targetSpeed = 1f;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}