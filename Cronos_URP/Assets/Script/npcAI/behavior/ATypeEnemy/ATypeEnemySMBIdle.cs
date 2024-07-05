using UnityEngine;

public class ATypeEnemySMBIdle : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("IDLE");
        _monoBehaviour.StopPursuit();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        // Ÿ�� �߰� ��
        GameObject currentTarget = _monoBehaviour.CurrentTarget;
        if (currentTarget != null)
        {
            // ���� ���·� ����
            _monoBehaviour.StartPursuit();
        }
        else
        {
            // Idle - ������ ����
            if (_monoBehaviour.IsNearBase() == false)
            {
                _monoBehaviour.TriggerReturn();
            }
        }
    }
}