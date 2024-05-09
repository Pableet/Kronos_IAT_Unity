using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.WSA;

/// <summary>
/// TestEnemy 의 행동을 정의한다.
/// </summary>
[DefaultExecutionOrder(100)]
public class TestEnemyBehavior : MonoBehaviour
{
    // 애니메이터의 파라미터 
    public static readonly int hashAttack = Animator.StringToHash("attack");
    public static readonly int hashInPursuit = Animator.StringToHash("inPursuit");
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");

    public MeleeWeapon meleeWeapon;
    public TargetScanner playerScanner = new TargetScanner();
    public float timeToStopPursuit;

    [System.NonSerialized]
    public float attackDistance = 2;

    public GameObject target { get { return _target; } }
    public Vector3 originalPosition { get; protected set; }
    public EnemyController controller { get { return _controller; } }
    public TargetDistributor.TargetFollower followerData { get { return _followerInstance; } }

    private GameObject _target;
    private EnemyController _controller;
    protected TargetDistributor.TargetFollower _followerInstance;

    protected float _timerSinceLostTarget = 0.0f;

    void Awake()
    {
        _controller = GetComponentInChildren<EnemyController>();

        meleeWeapon.SetOwner(gameObject);
    }

    void OnEnable()
    {
        SceneLinkedSMB<TestEnemyBehavior>.Initialise(_controller.animator, this);

        playerScanner.target = _controller.player;

        originalPosition = transform.position;
    }

    protected void OnDisable()
    {
        if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
    }

    private void FixedUpdate()
    {
        LookAtTarget();

        Vector3 toBase = originalPosition - transform.position;
        toBase.y = 0;

        SetNearBase(toBase.sqrMagnitude < 0.1 * 0.1f);
    }

    public void FindTarget()
    {
        // 타겟이 이미 보이는 경우 높이 차이를 무시한다.
        var target = playerScanner.Detect(transform, _target == null);

        if (_target == null)
        {
            // 플레이어를 처음 본 경우, 주변의 빈 지점을 선택하여 타겟팅.
            // (직접 플레이어 위치에 이동하지 않고 플레이어 주변에 군집을 이루도록)
            if (target != null)
            {
                _target = target;
                TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                if (distributor != null)
                {
                    _followerInstance = distributor.RegisterNewFollower();
                }
            }
        }
        else
        {
            // 플레이어가 감지 범위를 벗어나고 일정 시간 동안 플레이어가 보이지 않아도
            // 감지 각도에 벗어난 게 아니면 계속 감지 한다.
            if (target == null)
            {
                _timerSinceLostTarget += Time.deltaTime;

                if (_timerSinceLostTarget >= timeToStopPursuit)
                {
                    Vector3 toTarget = _target.transform.position - transform.position;

                    if (toTarget.sqrMagnitude > playerScanner.detectionRadius * playerScanner.detectionRadius)
                    {
                        if (_followerInstance != null)
                            _followerInstance.distributor.UnregisterFollower(_followerInstance);

                        // 타겟이 탐지 범위를 벗어나면 타겟을 재설정한다.
                        _target = null;
                    }
                }
            }
            else
            {
                if (target != _target)
                {
                    if (_followerInstance != null)
                    {
                        _followerInstance.distributor.UnregisterFollower(_followerInstance);
                    }

                    _target = target;

                    TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                    if (distributor != null)
                    {
                        _followerInstance = distributor.RegisterNewFollower();
                    }
                }

                _timerSinceLostTarget = 0.0f;
            }
        }
    }

    public void LookAtTarget()
    {
        if (_target == null) return;

        _controller.SetForwardToTarget(_target.transform.position);

    }

    public void StartLookAtTarget()
    {
        _controller.SetRotationLerpSeedSlow();
    }

    public void StopLookAtTarget()
    {
        _controller.SetRotationLerpSeedZero();
    }

    public void StartPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = true;
            RequestTargetPosition();
        }

        SetInPursuit(true);

        _controller.SetRotationLerpSeedNormal();
    }

    public void StopPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = false;
        }

        SetInPursuit(false);
    }

    public void RequestTargetPosition()
    {
        Vector3 fromTarget = transform.position - _target.transform.position;
        fromTarget.y = 0;

        _followerInstance.requiredPoint = _target.transform.position + fromTarget.normalized * attackDistance * 0.9f;
    }

    public void WalkBackToBase()
    {
        if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
        _target = null;
        StopPursuit();
        _controller.SetTarget(originalPosition);
        _controller.SetFollowNavmeshAgent(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print(true);
            _target = collision.gameObject;
        }
    }

    public void TriggerAttack()
    {
        _controller.animator.SetTrigger(hashAttack);
    }
    public void AttackBegin()
    {
        meleeWeapon.BeginAttack(false);
    }

    public void AttackEnd()
    {
        meleeWeapon.EndAttack();
    }

    public void SetNearBase(bool nearBase)
    {
        _controller.animator.SetBool(hashNearBase, nearBase);
    }

    public void SetInPursuit(bool inPursuit)
    {
        _controller.animator.SetBool(hashInPursuit, inPursuit);
    }

    private void OnDrawGizmos()
    {
        if (playerScanner != null)
        {
            playerScanner.EditorGizmo(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
