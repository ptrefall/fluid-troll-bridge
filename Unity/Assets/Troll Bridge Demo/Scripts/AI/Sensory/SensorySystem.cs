
using System.Collections.Generic;
using UnityEngine;

public class SensorySystem
{
    private ISensor[] _sensors;

    public SensorySystem(AIAgent agent)
    {
        _sensors = agent.transform.GetComponents<ISensor>();
    }

    public void Tick(AIContext context)
    {
        foreach (var sensor in _sensors)
        {
            if (context.Time >= sensor.NextTickTime)
            {
                sensor.NextTickTime = context.Time + sensor.TickRate;
                sensor.Tick(context);
            }
        }
    }

    public void DrawGizmos(AIContext context)
    {
        foreach (var sensor in _sensors)
        {
            sensor.DrawGizmos(context);
        }
    }
}