using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySMBAttack : SceneLinkedSMB<TestEnemyBehavior>
{
    protected Vector3 m_AttackPosition;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        _monoBehaviour.controller.SetFollowNavmeshAgent(false);

        m_AttackPosition = _monoBehaviour.target.transform.position;
        Vector3 toTarget = m_AttackPosition - _monoBehaviour.transform.position;
        toTarget.y = 0;

        _monoBehaviour.transform.forward = toTarget.normalized;
        _monoBehaviour.controller.SetForward(_monoBehaviour.transform.forward);

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        _monoBehaviour.controller.SetFollowNavmeshAgent(true);
    }
}
