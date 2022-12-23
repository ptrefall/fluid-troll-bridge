
using FluidHTN;
using FluidHTN.Operators;

public class FindBridgeOperator : IOperator
{
    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c)
        {
            if (c.CurrentBridge == null)
            {
                var bestTime = c.Time;
                foreach (var bridge in c.KnownBridges)
                {
                    if (bridge.LastTimeVisited < bestTime)
                    {
                        bestTime = bridge.LastTimeVisited;
                        c.CurrentBridge = bridge;
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