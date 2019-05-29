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

    public AIDomainBuilder HasState(AIWorldState state, byte value)
    {
        var condition = new HasWorldStateCondition(state, value);
        Pointer.AddCondition(condition);
        return this;
    }

    public AIDomainBuilder HasStateGreaterThan(AIWorldState state, byte value)
    {
        var condition = new HasWorldStateGreaterThanCondition(state, value);
        Pointer.AddCondition(condition);
        return this;
    }

    public AIDomainBuilder SetState(AIWorldState state, EffectType type)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new SetWorldStateEffect(state, type);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder SetState(AIWorldState state, bool value, EffectType type)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new SetWorldStateEffect(state, value, type);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder SetState(AIWorldState state, byte value, EffectType type)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new SetWorldStateEffect(state, value, type);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder IncrementState(AIWorldState state, EffectType type)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new IncrementWorldStateEffect(state, type);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder IncrementState(AIWorldState state, byte value, EffectType type)
    {
        if (Pointer is IPrimitiveTask task)
        {
            var effect = new IncrementWorldStateEffect(state, value, type);
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
        SetState(AIWorldState.HasReceivedDamage, false, EffectType.PlanAndExecute);
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
        IncrementState(AIWorldState.Stamina, EffectType.PlanAndExecute);
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
        SetState(AIWorldState.HasEnemyInMeleeRange, EffectType.PlanAndExecute);
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

    public AIDomainBuilder BeTired(float restTime)
    {
        Action("Be Tired");
        HasStateGreaterThan(AIWorldState.Stamina, 2);
        if (Pointer is IPrimitiveTask task)
        {
            task.SetOperator(new WaitOperator(restTime));
        }
        SetState(AIWorldState.Stamina, 0, EffectType.PlanAndExecute);
        End();
        return this;
    }
}
