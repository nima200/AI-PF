public class ReadPlaque : LeafNode
{
    public string Professor;
    public bool DoneReading { get; set; }
    public bool AdjacentToPlaque { get; set; }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }

    public override void Initialize()
    {
        Initialized = true;
        ActionManager.GetInstance().Read(this);
    }

    public override void Reset()
    {
        Print("Look plaque for: " + Professor + " Reseted");
    }

    public override BehaviorResult Process()
    {
        Result = !DoneReading ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }
}