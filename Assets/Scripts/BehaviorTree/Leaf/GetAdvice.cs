public class GetAdvice : LeafNode
{
    public bool FinishedTalking { get; set; }

    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().Talk(this);
    }

    public override void Reset()
    {
        Print("Get advice Reseted");
    }

    public override BehaviorResult Process()
    {
        Result = !FinishedTalking ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }
}