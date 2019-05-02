
using System;
using FluidHTN;

public class SetWorldStateEffect : IEffect
{
    public string Name { get; }
    public EffectType Type { get; }
    public AIWorldState State { get; }
    public byte Value { get; }

    public SetWorldStateEffect(AIWorldState state, EffectType type)
    {
        Name = $"SetState({state})";
        Type = type;
        State = state;
        Value = 1;
    }

    public SetWorldStateEffect(AIWorldState state, bool value, EffectType type)
    {
        Name = $"SetState({state})";
        Type = type;
        State = state;
        Value = (byte) (value ? 1 : 0);
    }

    public SetWorldStateEffect(AIWorldState state, byte value, EffectType type)
    {
        Name = $"SetState({state})";
        Type = type;
        State = state;
        Value = value;
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