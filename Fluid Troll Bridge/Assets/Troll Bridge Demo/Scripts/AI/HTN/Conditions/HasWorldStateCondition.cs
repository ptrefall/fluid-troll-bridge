
using System;
using FluidHTN;
using FluidHTN.Conditions;

public class HasWorldStateCondition : ICondition
{
    public string Name { get; }
    public AIWorldState State { get; }
    public byte Value { get; }

    public HasWorldStateCondition(AIWorldState state)
    {
        Name = $"HasState({state})";
        State = state;
        Value = 1;
    }

    public HasWorldStateCondition(AIWorldState state, byte value)
    {
        Name = $"HasState({state})";
        State = state;
        Value = value;
    }

    public bool IsValid(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            return c.HasState(State, Value);
        }

        throw new Exception("Unexpected context type!");
    }
}