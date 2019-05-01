
using FluidHTN;
using UnityEngine;

public class MeleeRangeSensor : MonoBehaviour, ISensor
{
    [SerializeField]
    [Tooltip("How often should we update our knowledge about the whereabouts of mobiles.")]
    private float _tickRate = 1f;

    public float TickRate => _tickRate;
    public float NextTickTime { get; set; }

    public void Tick(AIContext context)
    {
        if (context.CurrentEnemy != null)
        {
            var distance = Vector3.SqrMagnitude(context.CurrentEnemy.transform.position - context.Position);
            context.SetState(AIWorldState.HasEnemyInMeleeRange, distance <= context.Senses.SqrMeleeRange, EffectType.Permanent);
        }
        else
        {
            context.SetState(AIWorldState.HasEnemyInMeleeRange, false, EffectType.Permanent);
        }
    }

    public void DrawGizmos(AIContext context)
    {
        if (context.HasState(AIWorldState.HasEnemyInMeleeRange))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(context.Head.position, 1f);
        }
    }
}