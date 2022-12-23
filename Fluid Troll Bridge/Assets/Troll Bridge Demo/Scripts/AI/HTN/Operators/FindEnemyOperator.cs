
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

public class FindEnemyOperator : IOperator
{
    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            if (c.CurrentEnemy == null)
            {
                var bestDistance = c.Senses.SqrEyeSight;
                foreach (var enemy in c.KnownEnemies)
                {
                    var distance = Vector3.SqrMagnitude(enemy.transform.position - c.Position);
                    if (bestDistance > distance)
                    {
                        bestDistance = distance;
                        c.CurrentEnemy = enemy;
                    }
                }
            }

            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public void Stop(IContext ctx)
    {

    }

    public void Aborted(IContext ctx)
    {

    }
}