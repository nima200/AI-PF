public class FindProf : LeafNode
{
    /// <summary>
    /// Instantiates a request to the action manager 
    /// to initialize a pathfinding request for the agent, to the path request manager.
    /// Directly returns without requesting a path if agent has memory of the professor.
    /// </summary>
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

    /// <summary>
    /// Ran on every tick of the leaf. Evaluates whether the agent has reached its target or not.
    /// </summary>
    /// <returns>SUCCESS if agent is at target location, RUNNING if not.</returns>
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