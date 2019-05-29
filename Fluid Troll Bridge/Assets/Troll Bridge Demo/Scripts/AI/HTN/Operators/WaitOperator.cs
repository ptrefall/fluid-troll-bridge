
using FluidHTN;
using FluidHTN.Operators;

public class WaitOperator : IOperator
{
    public float WaitTime { get; set; }
    public WaitOperator(float waitTime)
    {
        WaitTime = waitTime;
    }

    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            if (c.GenericTimer <= 0f)
            {
                c.GenericTimer = c.Time + WaitTime;
                return TaskStatus.Continue;
            }

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
}