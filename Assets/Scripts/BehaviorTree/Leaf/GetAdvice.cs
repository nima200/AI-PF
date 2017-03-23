public class GetAdvice : LeafNode
{
    public string Professor;
    public bool FinishedTalking { get; set; }

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

    public override BehaviorResult Process()
    {
        Result = !FinishedTalking ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }
}