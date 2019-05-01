
using FluidHTN;
using UnityEngine;

public class MobileSensor : MonoBehaviour, ISensor
{
    private static Collider[] _hits = new Collider[128];

    [SerializeField][Tooltip("How often should we update our knowledge about the whereabouts of mobiles.")]
    private float _tickRate = 1f;

    public float TickRate => _tickRate;
    public float NextTickTime { get; set; }

    public void Tick(AIContext context)
    {
        context.KnownFriends.Clear();
        context.KnownEnemies.Clear();

        var hitCount = Physics.OverlapSphereNonAlloc(context.Position, context.Senses.EyeSight, _hits);
        if (hitCount > 0)
        {
            for (var i = 0; i < hitCount; i++)
            {
                var mobile = _hits[i].GetComponent<Mobile>();
                if (mobile != null)
                {
                    if (mobile.Type == context.Mobile?.Type)
                    {
                        context.KnownFriends.Add(mobile);
                    }
                    else
                    {
                        context.KnownEnemies.Add(mobile);
                    }
                }
            }
        }

        context.SetState(AIWorldState.HasEnemyInSight, context.KnownEnemies.Count > 0, EffectType.Permanent);
    }

    public void DrawGizmos(AIContext context)
    {
        foreach (var enemy in context.KnownEnemies)
        {
            DrawnEnemyGizmo(context, enemy);
        }
    }

    private void DrawnEnemyGizmo(AIContext context, Mobile enemy)
    {
        if (enemy == context.CurrentEnemy)
        {
            Gizmos.color = new Color(1f, 0f, 1f, 0.85f);
        }
        else
        {
            Gizmos.color = new Color(1f, 0f, 1f, 0.15f);
        }
        Gizmos.DrawLine(context.Head.position, enemy.transform.position + Vector3.up * 2f);
        Gizmos.DrawSphere(enemy.transform.position + Vector3.up * 2f, 0.25f);
    }
}