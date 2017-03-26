public class InteractionSequence : SequenceNode
{
    public override void Initialize()
    {
        Initialized = true;
        foreach (var childrenNode in ChildrenNodes)
        {
            childrenNode.SetProf(Professor);
        }
    }

    /// <summary>
    /// Initializes the first child and attempts to process it. Once that is done, moves on to initialization
    /// and processing of the next child. This happens until the last child is done. Fails if any child fails.
    /// </summary>
    /// <returns>Returns SUCCESS only if all children succeed.</returns>
    public override BehaviorResult Process()
    {
        for (int i = 0; i < ChildrenNodes.Count - 1; i++)
        {
            var child = ChildrenNodes[i];
            var nextChild = ChildrenNodes[i + 1];
            if (!child.Initialized)
            {
                child.Initialize();
                switch (child.Process())
                {
                    case BehaviorResult.FAIL:
                        Result = BehaviorResult.FAIL;
                        return Result;
                    case BehaviorResult.SUCCESS:
                        if (!nextChild.Initialized)
                        {
                            nextChild.Initialize();
                        }
                        continue;
                    case BehaviorResult.RUNNING:
                        Result = BehaviorResult.RUNNING;
                        return Result;
                }
            }
            else
            {
                switch (child.Process())
                {
                    case BehaviorResult.FAIL:
                        Result = BehaviorResult.FAIL;
                        return Result;
                    case BehaviorResult.SUCCESS:
                        if (!nextChild.Initialized)
                        {
                            nextChild.Initialize();
                        }
                        continue;
                    case BehaviorResult.RUNNING:
                        Result = BehaviorResult.RUNNING;
                        return Result;
                }
            }
        }
        if (ChildrenNodes[ChildrenNodes.Count - 1].Initialized)
        {
            switch (ChildrenNodes[ChildrenNodes.Count - 1].Process())
            {
                case BehaviorResult.FAIL:
                    Result = BehaviorResult.FAIL;
                    return Result;
                case BehaviorResult.SUCCESS:
                    Result = BehaviorResult.SUCCESS;
                    return Result;
                case BehaviorResult.RUNNING:
                    Result = BehaviorResult.RUNNING;
                    return Result;
            }
        }
        Result = BehaviorResult.RUNNING;
        return Result;
    }
}