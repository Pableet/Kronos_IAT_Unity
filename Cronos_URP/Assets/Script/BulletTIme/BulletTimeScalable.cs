using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BulletTimeScalable : MonoBehaviour
{
    public bool active = true;

    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnAnimatorMove()
    {
        if (active != true) return;

        _animator.speed *= BulletTime.Instance.GetCurrentSpeed();
        _navMeshAgent.speed *= BulletTime.Instance.GetCurrentSpeed();
    }
}
