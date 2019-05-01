
using FluidHTN;
using UnityEngine;

[CreateAssetMenu(fileName = "TrollDomain", menuName = "AI/Domains/Troll")]
public class TrollDomainDefinition : AIDomainDefinition
{
    public override Domain<AIContext> Create()
    {
        return new AIDomainBuilder("Troll")
            .Select("Enemy engagement")
                .HasState(AIWorldState.HasEnemyInSight)
                .ReceivedDamage()
            .End()
            .Sequence("Bridge patrol")
                .HasState(AIWorldState.HasBridgesInSight)
                .FindBridge()
                .MoveToBridge()
                .Wait(2f)
            .End()
            .Build();
    }
}