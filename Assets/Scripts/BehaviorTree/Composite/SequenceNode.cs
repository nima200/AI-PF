using System;

public class SequenceNode : CompositeNode
{
    public override void Initialize()
    {
        for (int i = 0; i < ChildrenNodes.Count - 1; i++)
        {
            var child = ChildrenNodes[i];
            var nextChild = ChildrenNodes[i + 1];
            if (child.Initialized && child.Result == BehaviorResult.SUCCESS)
            {
                nextChild.Initialize();
                break;
            }
            child.Initialize();
        }
    }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
        foreach (var node in ChildrenNodes)
        {
            node.SetProf(professorName);
        }
    }

    /// <summary>
    /// Attempts to proccess every child. If any fails, this fails. If all succeed it succeeds.
    /// If end is reached but a child was running in the check, then it returns running.
    /// </summary>
    /// <returns>The behavior result</returns>
    public override BehaviorResult Process()
    {
                bool childRunning = false;
                foreach (var node in ChildrenNodes)
                {
                    switch (node.Process())
                    {
                        case BehaviorResult.FAIL:
                            Print("A child failed. FAIL.");
                            Result = BehaviorResult.FAIL;
                            return Result;
                        case BehaviorResult.SUCCESS:
                            continue;
                        case BehaviorResult.RUNNING:
                            childRunning = true;
                            continue;
                        default:
                            Result = BehaviorResult.SUCCESS;
                            return Result;
                    }
                }
                Result = childRunning ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
                return Result;
    }
}