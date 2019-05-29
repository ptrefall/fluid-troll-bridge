
public interface ISensor
{
    float TickRate { get; }
    float NextTickTime { get; set; }
    void Tick(AIContext context);
    void DrawGizmos(AIContext context);
}