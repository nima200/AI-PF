public class ReadPlaque : LeafNode
{
    public bool DoneReading { get; set; }
    public bool AdjacentToPlaque { get; set; }

    /// <summary>
    /// Attempts to initialize the node. The effect is that if the agent already
    /// knows about the professor, in its memory, then it does not search for it again,
    /// while if it doesn't know about it, it will send a request to the action manager
    /// to instantiate a path finding request for the agent from its current location, to
    /// the location of the node's prof's transform.
    /// </summary>
    public override void Initialize()
    {
        Initialized = true;
        if (Agent.Memory.Contains(Professor))
        {
            DoneReading = true;
            return;
        }
        ActionManager.GetInstance().Read(this);
    }

    public override void Reset()
    {
        base.Reset();
        DoneReading = false;
        AdjacentToPlaque = false;
    }

    /// <summary>
    /// Given the agent has completed reading the plaque, it evaluates whether the plaque's
    /// mapped professor is the same as teh agent's target professor. If so, it returns SUCCESS and if
    /// not, it returns FAIL.
    /// While reading, it returns RUNNING.
    /// </summary>
    /// <returns></returns>
    public override BehaviorResult Process()
    {
        if (Result != BehaviorResult.SUCCESS)
        {
            switch (DoneReading)
            {
                case true:
                    switch (Agent.TargetProfessor == Professor)
                    {
                        case true:
                            Result = BehaviorResult.SUCCESS;
                            return Result;
                        default:
                            Result = BehaviorResult.FAIL;
                            return Result;
                    }
                default:
                    Result = BehaviorResult.RUNNING;
                    return Result;
            }
        }
        Result = BehaviorResult.SUCCESS;
        return Result;
    }
}