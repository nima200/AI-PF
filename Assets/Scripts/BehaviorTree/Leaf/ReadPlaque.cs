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
        Result = !DoneReading
            ? BehaviorResult.RUNNING
            : (Professor == Agent.TargetProfessor ? BehaviorResult.SUCCESS : BehaviorResult.FAIL);
        return Result;
    }
}