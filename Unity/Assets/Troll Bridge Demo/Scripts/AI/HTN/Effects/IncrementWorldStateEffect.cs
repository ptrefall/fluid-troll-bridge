
using System;
using FluidHTN;

public class IncrementWorldStateEffect : IEffect
{
    public string Name { get; }
    public EffectType Type { get; }
    public AIWorldState State { get; }
    public byte Value { get; }

    public IncrementWorldStateEffect(AIWorldState state, EffectType type)
    {
        Name = $"IncrementState({state})";
        Type = type;
        State = state;
        Value = 1;
    }

    public IncrementWorldStateEffect(AIWorldState state, byte value, EffectType type)
    {
        Name = $"IncrementState({state})";
        Type = type;
        State = state;
        Value = value;
    }

    public void Apply(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            var currentValue = c.GetState(State);
            c.SetState(State, (byte) (currentValue + Value), Type);
            return;
        }

        throw new Exception("Unexpected context type!");
    }
}