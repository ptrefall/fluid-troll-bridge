
using FluidHTN;
using UnityEngine;

public abstract class AIDomainDefinition : ScriptableObject
{
    public abstract Domain<AIContext> Create();
}