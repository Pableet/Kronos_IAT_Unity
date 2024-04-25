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
    public EnemyController controller { get { return _controller; } }
    public GameObject target { get { return _target; } }
    public Vector3 originalPosition { get; protected set; }
    public TargetDistributor.TargetFollower followerData { get { return _followerInstance; } }

    public TargetScanner playerScanner;
    public float timeToStopPursuit;

    private GameObject _target;
    private EnemyController _controller;
    protected TargetDistributor.TargetFollower _followerInstance;

    void OnEnable()
    {
        _controller = GetComponentInChildren<EnemyController>();

        playerScanner = new TargetScanner(GameObject.FindGameObjectWithTag("Player"));

        playerScanner.heightOffset = 1.6f;
        playerScanner.detectionRadius = 4f;
        playerScanner.detectionAngle = 90f;
        playerScanner.maxHeightDifference = 1.5f;
        playerScanner.viewBlockerLayerMask = 0;

        originalPosition = transform.position;

        SceneLinkedSMB<TestEnemyBehavior>.Initialise(_controller.animator, this);
    }

    // Update is called once per frame
    void Update()
    {
        //_controller.animator.SetFloat("speed", _controller.navmeshAgent.velocity.magnitude / _controller.navmeshAgent.speed);

        //if (_timePassed >= attackCD)
        //{
        //    if (Vector3.Distance(_target.transform.position, transform.position) <= attackRange)
        //    {
        //        TriggerAttack();
        //        _timePassed = 0;
        //    }
        //}
        //_timePassed += Time.deltaTime;

        //if (_newDestinationCD <= 0 && Vector3.Distance(_target.transform.position, transform.position) <= aggroRange)
        //{
        //    _newDestinationCD = 0.5f;
        //    _controller.navmeshAgent.SetDestination(_target.transform.position);
        //}

        //// 플레이어를 향해 바라보기
        //Vector3 direction = _target.transform.position - transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, Time.deltaTime);

        //_newDestinationCD -= Time.deltaTime;
    }

    protected void OnDisable()
    {
        if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
    }

    private void FixedUpdate()
    {
        Vector3 toBase = originalPosition - transform.position;
        toBase.y = 0;

        _controller.animator.SetBool(hashNearBase, toBase.sqrMagnitude < 0.1 * 0.1f);
    }

    protected float _timerSinceLostTarget = 0.0f;
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

    public void StartPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = true;
            RequestTargetPosition();
        }

        _controller.animator.SetBool(hashInPursuit, true);
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
