using System;
using System.Collections.Generic;
using FluidHTN;
using FluidHTN.Contexts;
using FluidHTN.Factory;

public partial class AIContext : BaseContext
{
    public override IFactory Factory { get; set; } = new DefaultFactory();
    public override List<string> MTRDebug { get; set; }
    public override List<string> LastMTRDebug { get; set; }
    public override bool DebugMTR { get; } = false;
    public override Stack<string> DecompositionLog { get; set; }
    public override bool LogDecomposition { get; } = false;

    public override byte[] WorldState { get; } = new byte[Enum.GetValues(typeof(AIWorldState)).Length];

    public bool HasState(AIWorldState state, bool value)
    {
        return HasState((int) state, (byte) (value ? 1 : 0));
    }

    public bool HasState(AIWorldState state, byte value)
    {
        return HasState((int)state, value);
    }

    public bool HasState(AIWorldState state)
    {
        return HasState((int) state, 1);
    }

    public void SetState(AIWorldState state, bool value, EffectType type)
    {
        SetState((int) state, (byte) (value ? 1 : 0), true, type);
    }

    public void SetState(AIWorldState state, byte value, EffectType type)
    {
        SetState((int)state, value, true, type);
    }

    public byte GetState(AIWorldState state)
    {
        return GetState((int) state);
    }
}