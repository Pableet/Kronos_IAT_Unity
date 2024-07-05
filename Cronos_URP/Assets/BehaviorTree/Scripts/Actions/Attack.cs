using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Attack : ActionNode
{
    public readonly int hashAttack = Animator.StringToHash("is attack");

    protected override void OnStart()
    {
        context.animator.SetBool(hashAttack, true);
        context.agent.enabled = false;
    }

    protected override void OnStop()
    {
        context.agent.enabled = true;
    }

    protected override State OnUpdate()
    {
        bool isAttack = context.animator.GetBool(hashAttack);

        if(!isAttack)
        {
            return State.Success;
        }

        return State.Running;
    }
}