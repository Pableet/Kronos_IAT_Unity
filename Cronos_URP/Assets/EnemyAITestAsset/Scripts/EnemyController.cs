using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// 모든 적 몬스터는 얘를 컴포넌트로 가질 것이다.
/// </summary>
[DefaultExecutionOrder(-1)] // 다른 스키립트보다 먼저 실행(실행 주문 값이 낮을 수록 먼저 실행)
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public bool interpolateTurning = false;
    public bool applyAnimationRotation = false;

    public Animator animator { get { return _animator; } }
    public Vector3 externalForce { get { return _externalForce; } }
    public NavMeshAgent navmeshAgent { get { return _navMeshAgent; } }
    public bool followNavmeshAgent { get { return _followNavmeshAgent; } }
    public bool grounded { get { return _grounded; } }

    protected NavMeshAgent _navMeshAgent;
    protected bool _followNavmeshAgent;
    protected Animator _animator;
    protected bool _underExternalForce;
    protected bool _externalForceAddGravity = true;
    protected Vector3 _externalForce;
    protected bool _grounded;

    protected Rigidbody _rigidbody;

    const float _groundedRayDistance = .8f;

    void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

        _navMeshAgent.updatePosition = false;

        _rigidbody = GetComponentInChildren<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        _followNavmeshAgent = true;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (_underExternalForce)
        {
            ForceMovement();
        }
    }

    // 지면 위에 있는지 검사한다.
    void CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * _groundedRayDistance * 0.5f, -Vector3.up);
        _grounded = Physics.Raycast(ray, out hit, _groundedRayDistance, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// 외부에 힘을 받으면 느려진다.
    /// </summary>
    void ForceMovement()
    {
        if (_externalForceAddGravity)
        {
            _externalForce += Physics.gravity * Time.deltaTime;
        }

        RaycastHit hit;
        Vector3 movement = _externalForce * Time.deltaTime;
        if (!_rigidbody.SweepTest(movement.normalized, out hit, movement.sqrMagnitude))
        {
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

        _navMeshAgent.Warp(_rigidbody.position);
    }

    private void OnAnimatorMove()
    {
        if (_underExternalForce)
            return;

        if (_followNavmeshAgent)
        {
            _navMeshAgent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude;
            transform.position = _navMeshAgent.nextPosition;
        }
        else
        {
            RaycastHit hit;
            if (!_rigidbody.SweepTest(_animator.deltaPosition.normalized, out hit,
                _animator.deltaPosition.sqrMagnitude))
            {
                _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
            }
        }

        if (applyAnimationRotation)
        {
            transform.forward = _animator.deltaRotation * transform.forward;
        }
    }

    /// <summary>
    /// 내비메시를 비활성화 한다.
    /// </summary>
    /// <param name="follow"></param>
    public void SetFollowNavmeshAgent(bool follow)
    {
        if (!follow && _navMeshAgent.enabled)
        {
            _navMeshAgent.ResetPath();
        }
        else if (follow && !_navMeshAgent.enabled)
        {
            _navMeshAgent.Warp(transform.position);
        }

        _followNavmeshAgent = follow;
        _navMeshAgent.enabled = follow;
    }

    public void AddForce(Vector3 force, bool useGravity = true)
    {
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.ResetPath(); 
        }

        _externalForce = force;
        _navMeshAgent.enabled = false;
        _underExternalForce = true;
        _externalForceAddGravity = useGravity;
    }

    public void ClearForce()
    {
        _underExternalForce = false;
        _navMeshAgent.enabled = true;
    }

    public void SetForward(Vector3 forward)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward);

        if (interpolateTurning)
        {
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                _navMeshAgent.angularSpeed * Time.deltaTime);
        }

        transform.rotation = targetRotation;
    }

    public bool SetTarget(Vector3 position)
    {
        return _navMeshAgent.SetDestination(position);
    }
}
