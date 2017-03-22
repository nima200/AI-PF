public class ReadPlaque : LeafNode
{
    public string Professor;
    public bool DoneReading { get; set; }
    public bool AdjacentToPlaque { get; set; }
    public bool Initialized { get; private set; }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }

    public override void Initialize()
    {
        Print("Look plaque for professor: " + Professor + " initialized.");
        Initialized = true;
        ActionManager.GetInstance().Read(this);
    }

    public override void Reset()
    {
        Print("Look plaque for: " + Professor + " Reseted");
    }

    public override BehaviorResult Process()
    {
        Print("ReadPlaque for: " + Professor + " processing.");
        return !DoneReading ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
    }
}