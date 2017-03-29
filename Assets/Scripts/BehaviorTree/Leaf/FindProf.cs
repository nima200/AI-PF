public class FindProf : LeafNode
{

    public override void Initialize()
    {
        Initialized = true;
        if (Agent.Memory.Contains(Professor))
        {
            Agent.ReachedTarget = true;
            return;
        }
        ActionManager.GetInstance().FindProfessor(this);
    }

    public override BehaviorResult Process()
    {
        Result = !Agent.ReachedTarget ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }

    public override void Reset()
    {
        base.Reset();
        Agent.ReachedTarget = false;
    }
}