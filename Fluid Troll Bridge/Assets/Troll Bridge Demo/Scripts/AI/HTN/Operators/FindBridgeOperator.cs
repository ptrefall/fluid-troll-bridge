using FluidHTN;
using FluidHTN.Operators;

public class FindBridgeOperator : IOperator
{
    public TaskStatus Start(IContext ctx)
    {
        if (ctx is AIContext c)
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

            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public TaskStatus Update(IContext ctx)
    {
        return TaskStatus.Failure;
    }

    public void Stop(IContext ctx)
    {
        
    }

    public void Abort(IContext ctx)
    {

    }
}