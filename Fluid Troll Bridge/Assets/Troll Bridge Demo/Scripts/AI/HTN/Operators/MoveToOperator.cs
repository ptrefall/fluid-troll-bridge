using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class MoveToOperator : IOperator
{
    public AIDestinationTarget DestinationTarget { get; }

    public MoveToOperator(AIDestinationTarget destinationTarget)
    {
        DestinationTarget = destinationTarget;
    }

    public TaskStatus StartNavigation(AIContext c)
    {
        if (DestinationTarget == AIDestinationTarget.Bridge)
        {
            if (c.CurrentBridge == null)
            {
                return TaskStatus.Failure;
            }

            c.NavAgent.isStopped = false;
            if (c.NavAgent.SetDestination(c.CurrentBridge.transform.position))
            {
                return TaskStatus.Continue;
            }
        }
        else if (DestinationTarget == AIDestinationTarget.Enemy)
        {
            if (c.CurrentEnemy == null)
            {
                return TaskStatus.Failure;
            }

            c.NavAgent.isStopped = false;
            if (c.NavAgent.SetDestination(c.CurrentEnemy.transform.position))
            {
                return TaskStatus.Continue;
            }
        }

        return TaskStatus.Failure;
    }

    public TaskStatus UpdateNavigation(AIContext c)
    {
        if (DestinationTarget == AIDestinationTarget.Bridge)
        {
            if (c.CurrentBridge == null)
            {
                return TaskStatus.Failure;
            }

            if (c.NavAgent.hasPath && c.NavAgent.remainingDistance <= c.NavAgent.stoppingDistance)
            {
                c.CurrentBridge.LastTimeVisited = c.Time;
                c.CurrentBridge = null;
                c.NavAgent.isStopped = true;
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }
        else if (DestinationTarget == AIDestinationTarget.Enemy)
        {
            if (c.CurrentEnemy == null)
            {
                return TaskStatus.Failure;
            }

            if (c.NavAgent.remainingDistance <= c.NavAgent.stoppingDistance)
            {
                c.NavAgent.isStopped = true;
                return TaskStatus.Success;
            }

            if (c.NavAgent.SetDestination(c.CurrentEnemy.transform.position))
            {
                return TaskStatus.Continue;
            }
        }

        return TaskStatus.Failure;
    }

    public TaskStatus Start(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            return StartNavigation(c);
        }

        return TaskStatus.Failure;
    }

    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            return UpdateNavigation(c);
        }

        return TaskStatus.Failure;
    }

    public void Stop(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            if (DestinationTarget == AIDestinationTarget.Bridge)
            {
                c.CurrentBridge = null;
            }

            c.NavAgent.isStopped = true;
        }
    }

    public void Abort(IContext ctx)
    {
        Stop(ctx);
    }
}