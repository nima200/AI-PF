using System;

public class InteractionSequence : SequenceNode
{
//    public FindProf FindPlaqueNode;
//    public ReadPlaque ReadPlaqueNode;
//    public GetAdvice GetAdviceNode;
    public string Professor;

//    public InteractionSequence(FindProf findPlaqueNode, ReadPlaque readPlaqueNode, GetAdvice getAdviceNode)
//    {
//        FindPlaqueNode = findPlaqueNode;
//        ReadPlaqueNode = readPlaqueNode;
//        GetAdviceNode = getAdviceNode;
//    }

    public override void Initialize()
    {
        Initialized = true;
        foreach (var childrenNode in ChildrenNodes)
        {
            childrenNode.SetProf(Professor);
        }
//        FindPlaqueNode.Professor = Professor;
//        ReadPlaqueNode.Professor = Professor;
//        GetAdviceNode.Professor = Professor;
    }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }

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


/*        if (FindPlaqueNode.Initialized)
        {
            switch (FindPlaqueNode.Process())
            {
                case BehaviorResult.FAIL:
                    Result = BehaviorResult.FAIL;
                    return Result;
                case BehaviorResult.SUCCESS:
                    if (ReadPlaqueNode.Initialized)
                    {
                        switch (ReadPlaqueNode.Process())
                        {
                            case BehaviorResult.FAIL:
                                Result = BehaviorResult.FAIL;
                                return Result;
                            case BehaviorResult.SUCCESS:
                                if (GetAdviceNode.Initialized)
                                {
                                    switch (GetAdviceNode.Process())
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
                                GetAdviceNode.Initialize();
                                Result = BehaviorResult.RUNNING;
                                return Result;
                            case BehaviorResult.RUNNING:
                                Result = BehaviorResult.RUNNING;
                                return Result;
                            default:
                                Result = BehaviorResult.FAIL;
                                return Result;
                        }
                    }
                    ReadPlaqueNode.Initialize();
                    Result = BehaviorResult.RUNNING;
                    return Result;
                case BehaviorResult.RUNNING:
                    Result = BehaviorResult.RUNNING;
                    return Result;
                default:
                    Result = BehaviorResult.FAIL;
                    return Result;
            }
        }
        FindPlaqueNode.Initialize();
        Result = BehaviorResult.RUNNING;
        return Result;
    }*/
    }
}