
using FluidHTN;
using UnityEngine;

public enum MobileType { Human, Troll }

public abstract class Mobile : MonoBehaviour
{
    [SerializeField] private float _attackDamage = 10f;

    public abstract MobileType Type { get; }
    public AIContext Context { get; private set; }

    public void Init(AIContext context)
    {
        Context = context;
    }

    public void TakeDamage(float damage)
    {
        Context.SetState(AIWorldState.HasReceivedDamage, true, EffectType.Permanent);
        // TODO: HP system?
    }

    // This should be called from an event inside each attack animation
    private void OnDealDamage()
    {
        if (Context.CurrentEnemy != null)
        {
            Context.CurrentEnemy.TakeDamage(_attackDamage);
        }
    }
}