
using System;
using FluidHTN;
using FluidHTN.Conditions;

public class HasWorldStateGreaterThanCondition : ICondition
{
    public string Name { get; }
    public AIWorldState State { get; }
    public byte Value { get; }

    public HasWorldStateGreaterThanCondition(AIWorldState state, byte value)
    {
        Name = $"HasStateGreaterThan({state})";
        State = state;
        Value = value;
    }

    public bool IsValid(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            return c.GetState(State) > Value;
        }

        throw new Exception("Unexpected context type!");
    }
}