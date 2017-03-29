public class ReadPlaque : LeafNode
{
    public bool DoneReading { get; set; }
    public bool AdjacentToPlaque { get; set; }

    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().Read(this);
    }

    public override void Reset()
    {
        base.Reset();
        DoneReading = false;
        AdjacentToPlaque = false;
    }

    public override BehaviorResult Process()
    {
        switch (DoneReading)
        {
            case true:
                switch (Agent.TargetProfessor == Professor)
                {
                    case true:
                        Result = BehaviorResult.SUCCESS;
                        return Result;
                    default:
                        Result = BehaviorResult.FAIL;
                        return Result;
                }
            default:
                Result = BehaviorResult.RUNNING;
                return Result;
        }
    }
}