public class ReadPlaque : LeafNode
{
    public string Professor;
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
        Result = !DoneReading ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }
}