
using FluidHTN;
using FluidHTN.Operators;

public class TakeDamageOperator : IOperator
{
    public TaskStatus Start(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            c.Animator.SetTrigger("takeDamage");
            var clipInfo = c.Animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                var clip = clipInfo[0].clip;
                c.GenericTimer = c.Time + clip.length;
            }

            return TaskStatus.Continue;
        }

        return TaskStatus.Failure;
    }

    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            if (c.Time < c.GenericTimer)
            {
                return TaskStatus.Continue;
            }

            c.GenericTimer = -1f;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public void Stop(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            c.GenericTimer = -1f;
        }
    }

    public void Abort(IContext ctx)
    {
        Stop(ctx);
    }
}