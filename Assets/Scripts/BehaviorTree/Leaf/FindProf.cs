public class FindProf : LeafNode
{
    public bool FoundProfessorPlaque { get; set; }

    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().FindProfessorPlaque(this);
    }

    public override BehaviorResult Process()
    {
        Result = !Agent.ReachedTarget ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }

    public override void Reset()
    {
        base.Reset();
        FoundProfessorPlaque = false;
        Agent.ReachedTarget = false;
    }
}