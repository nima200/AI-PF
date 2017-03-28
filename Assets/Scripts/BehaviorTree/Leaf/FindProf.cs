public class FindProf : LeafNode
{
    public bool FoundProfessorPlaque { get; set; }

    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().FindProfessor(this);
    }

    public override BehaviorResult Process()
    {
        Result = !FoundProfessorPlaque ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }

    public override void Reset()
    {
        base.Reset();
        FoundProfessorPlaque = false;
    }
}