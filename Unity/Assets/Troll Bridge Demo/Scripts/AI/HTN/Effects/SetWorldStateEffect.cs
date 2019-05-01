
using System;
using FluidHTN;

public class SetWorldStateEffect : IEffect
{
    public string Name { get; }
    public EffectType Type { get; }
    public AIWorldState State { get; }
    public byte Value { get; }

    public SetWorldStateEffect(AIWorldState state)
    {
        Name = $"SetState({state})";
        State = state;
        Value = 1;
    }

    public SetWorldStateEffect(AIWorldState state, bool value)
    {
        Name = $"SetState({state})";
        State = state;
        Value = (byte) (value ? 1 : 0);
    }

    public void Apply(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            c.SetState(State, Value, Type);
            return;
        }

        throw new Exception("Unexpected context type!");
    }
}