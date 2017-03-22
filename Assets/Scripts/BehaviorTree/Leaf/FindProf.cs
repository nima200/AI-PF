using System.Linq.Expressions;

public class FindProf : LeafNode
{
    public string Professor;
    public bool FoundProfessorPlaque { get; set; }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }

    public override void Initialize()
    {
        Print("Finding professor: " + Professor);
        Initialized = true;
        ActionManager.GetInstance().FindProfessor(this);
    }

    public override BehaviorResult Process()
    {
        Print("Find professor for: " + Professor + " in progress.");
        return !FoundProfessorPlaque ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
    }
}