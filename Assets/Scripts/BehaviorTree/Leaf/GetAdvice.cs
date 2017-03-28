public class GetAdvice : LeafNode
{

    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().Talk(this);
    }

    public override void Reset()
    {
        base.Reset();
        Agent.FinishedTalking = false;
    }

    public override BehaviorResult Process()
    {
        if (Agent.FinishedTalking)
        {
            Result = BehaviorResult.SUCCESS;
            return Result;
        }
        Result = BehaviorResult.RUNNING;
        return Result;
    }
}