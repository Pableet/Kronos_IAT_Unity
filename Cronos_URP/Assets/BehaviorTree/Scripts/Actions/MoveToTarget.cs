using UnityEngine;

public class MoveToTarget : ActionNode
{
    public float speed = 3.5f;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 100f;

    protected override void OnStart()
    {
        if (blackboard.target == null)
        {
            Debug.Log("타깃을 찾을 수 없음");
        }
        else
        {
            blackboard.moveToPosition = blackboard.target.transform.position;
        }

        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < stoppingDistance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }
}