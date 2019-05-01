using FluidHTN;
using System.Collections;
using System.Collections.Generic;
using FluidHTN.Factory;
using FluidHTN.PrimitiveTasks;
using UnityEngine;

public class AIDomainBuilder : BaseDomainBuilder<AIDomainBuilder, AIContext>
{
    public AIDomainBuilder(string domainName) : base(domainName, new DefaultFactory())
    {
    }

    public AIDomainBuilder HasState(AIWorldState state)
    {
        var condition = new HasWorldStateCondition(state);
        Pointer.AddCondition(condition);
        return this;
    }

    public AIDomainBuilder SetState(AIWorldState state)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new SetWorldStateEffect(state);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder SetState(AIWorldState state, bool value)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new SetWorldStateEffect(state, value);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder ReceivedDamage()
    {
        Action("Received damage");
        HasState(AIWorldState.HasReceivedDamage);
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new TakeDamageOperator());
        }
        SetState(AIWorldState.HasReceivedDamage, false);
        End();
        return this;
    }

    public AIDomainBuilder FindEnemy()
    {
        Action("Find enemy");
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new FindEnemyOperator());
        }
        End();
        return this;
    }

    public AIDomainBuilder AttackEnemy()
    {
        Action("Attack enemy");
        HasState(AIWorldState.HasEnemyInMeleeRange);
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new AttackOperator());
        }
        End();
        return this;
    }

    public AIDomainBuilder MoveToEnemy()
    {
        Action("Move to enemy");
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new MoveToOperator(AIDestinationTarget.Enemy));
        }
        SetState(AIWorldState.HasEnemyInMeleeRange);
        End();
        return this;
    }

    public AIDomainBuilder FindBridge()
    {
        Action("Find bridge");
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new FindBridgeOperator());
        }
        End();
        return this;
    }

    public AIDomainBuilder MoveToBridge()
    {
        Action("Move to bridge");
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new MoveToOperator(AIDestinationTarget.Bridge));
        }
        End();
        return this;
    }

    public AIDomainBuilder Wait(float waitTime)
    {
        Action("Wait");
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new WaitOperator(waitTime));
        }
        End();
        return this;
    }
}
