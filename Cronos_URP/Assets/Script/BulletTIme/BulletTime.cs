using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    [SerializeField]
    private float currentSpeed = 0f;

    public float maxSpeed = 1f;
    public float acceleration = 1f;
    public float deceleration = 1f;

    public static BulletTime Instance { get; private set; }

    private void Start()
    {
        currentSpeed += acceleration * 1f;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void LateUpdate()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed); // 최대 속도 초과 방지

        }
        else if (currentSpeed > maxSpeed)
        {
            currentSpeed *= (1 - Time.deltaTime * deceleration);
            currentSpeed = Mathf.Max(currentSpeed, maxSpeed); // 최소 속도 미만 방지
        }
    }
    public void DecelerateSpeed()
    {
        maxSpeed = 0;
    }

    public void SetNormalSpeed()
    {
        maxSpeed = 1f;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
