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
        Initialized = true;
        ActionManager.GetInstance().FindProfessor(this);
    }

    public override BehaviorResult Process()
    {
        Result = !FoundProfessorPlaque ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }
}