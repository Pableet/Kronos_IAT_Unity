using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

[DefaultExecutionOrder(100)]
public class TestEnemyBehavior : MonoBehaviour
{
    // 애니메이터의 파라미터 
    public static readonly int hashAttack = Animator.StringToHash("attack");
    public static readonly int hashInPursuit = Animator.StringToHash("inPursuit");
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");

    [Header("Player Scan")]
    public float heightOffset = 1.6f;
    public float detectionRadius = 4f;
    public float detectionAngle = 90f;
    public float maxHeightDifference = 1.5f;
    public int viewBlockerLayerMask = 0;

    public EnemyController controller { get { return _controller; } }
    public GameObject target { get { return _target; } }
    public Vector3 originalPosition { get; protected set; }
    public TargetDistributor.TargetFollower followerData { get { return _followerInstance; } }

    private TargetScanner playerScanner;
    public float timeToStopPursuit;

    private GameObject _target;
    private EnemyController _controller;
    protected TargetDistributor.TargetFollower _followerInstance;

    protected float _timerSinceLostTarget = 0.0f;

    void OnEnable()
    {
        _controller = GetComponentInChildren<EnemyController>();

        playerScanner = new TargetScanner(_controller.player);

        playerScanner.heightOffset = heightOffset;
        playerScanner.detectionRadius = detectionRadius;
        playerScanner.detectionAngle = detectionAngle;
        playerScanner.maxHeightDifference = maxHeightDifference;
        playerScanner.viewBlockerLayerMask = viewBlockerLayerMask;

        originalPosition = transform.position;

        SceneLinkedSMB<TestEnemyBehavior>.Initialise(_controller.animator, this);
    }

    protected void OnDisable()
    {
        if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
    }

    private void Update()
    {
        LookAtTarget();

        playerScanner.heightOffset = heightOffset;
        playerScanner.detectionRadius = detectionRadius;
        playerScanner.detectionAngle = detectionAngle;
        playerScanner.maxHeightDifference = maxHeightDifference;
        playerScanner.viewBlockerLayerMask = viewBlockerLayerMask;
    }

    private void FixedUpdate()
    {
        Vector3 toBase = originalPosition - transform.position;
        toBase.y = 0;

        _controller.animator.SetBool(hashNearBase, toBase.sqrMagnitude < 0.1 * 0.1f);
    }

    public void FindTarget()
    {
        //we ignore height difference if the target was already seen
        var target = playerScanner.Detect(transform, _target == null);

        if (_target == null)
        {
            //we just saw the player for the first time, pick an empty spot to target around them
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
            //we lost the target. But chomper have a special behaviour : they only loose the player scent if they move past their detection range
            //and they didn't see the player for a given time. Not if they move out of their detectionAngle. So we check that this is the case before removing the target
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

                        //the target move out of range, reset the target
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

        //Vector3 targetPosition = _target.transform.position;
        //targetPosition.y = 0f; 
        //this.transform.LookAt(targetPosition); 

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

        _controller.animator.SetBool(hashInPursuit, true);

        _controller.SetRotationLerpSeedNormal();
    }

    [System.NonSerialized]
    public float attackDistance = 3;
    public void StopPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = false;
        }

        _controller.animator.SetBool(hashInPursuit, false);

        //StopLookAtTarget();
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

    private void OnDrawGizmosSelected()
    {
        playerScanner.EditorGizmo(transform);
    }
}
