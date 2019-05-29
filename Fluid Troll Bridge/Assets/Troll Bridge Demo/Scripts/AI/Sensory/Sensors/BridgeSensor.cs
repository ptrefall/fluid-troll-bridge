
using FluidHTN;
using UnityEngine;

public class BridgeSensor : MonoBehaviour, ISensor
{
    private static Collider[] _hits = new Collider[128];

    [SerializeField][Tooltip("How often should we update our knowledge about the whereabouts of bridges.")]
    private float _tickRate = 1f;

    public float TickRate => _tickRate;
    public float NextTickTime { get; set; }

    public void Tick(AIContext context)
    {
        context.KnownBridges.Clear();
        var hitCount = Physics.OverlapSphereNonAlloc(context.Position, context.Senses.EyeSight, _hits);
        if (hitCount > 0)
        {
            for (var i = 0; i < hitCount; i++)
            {
                var bridge = _hits[i].GetComponent<Bridge>();
                if (bridge != null)
                {
                    context.KnownBridges.Add(bridge);
                }
            }
        }

        context.SetState(AIWorldState.HasBridgesInSight, context.KnownBridges.Count > 0, EffectType.Permanent);
    }

    public void DrawGizmos(AIContext context)
    {
        foreach (var bridge in context.KnownBridges)
        {
            if (bridge == context.CurrentBridge)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.85f);
            }
            else
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.15f);
            }
            Gizmos.DrawLine(context.Head.position, bridge.transform.position + Vector3.up * 2f);
            Gizmos.DrawSphere(bridge.transform.position + Vector3.up * 2f, 0.25f);
        }
    }
}