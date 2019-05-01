
using System;
using FluidHTN;
using FluidHTN.Conditions;

public class HasWorldStateCondition : ICondition
{
    public string Name { get; }
    public AIWorldState State { get; }

    public HasWorldStateCondition(AIWorldState state)
    {
        Name = $"HasState({state})";
        State = state;
    }

    public bool IsValid(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            return c.HasState(State);
        }

        throw new Exception("Unexpected context type!");
    }
}