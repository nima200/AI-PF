public class GetAdvice : LeafNode
{
    public bool FinishedTalking { get; set; }

    /// <summary>
    /// Requests the action manager to instantiate a talk between the student and the professor.
    /// </summary>
    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().Talk(this);
    }

    public override void Reset()
    {
        base.Reset();
        FinishedTalking = false;
    }

    /// <summary>
    /// Returns running if agent is still not done talking, and success if so.
    /// </summary>
    /// <returns></returns>
    public override BehaviorResult Process()
    {
        Result = !FinishedTalking ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }
}