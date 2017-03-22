using System;
using System.Xml;

public class ProfAdviceSequence : SequenceNode
{
    public ReadPlaque ReadPlaqueNode;
    public GetAdvice GetAdviceNode;
    public string ProfessorName;

    public ProfAdviceSequence(ReadPlaque readPlaqueNode, GetAdvice getAdviceNode)
    {
        ReadPlaqueNode = readPlaqueNode;
        GetAdviceNode = getAdviceNode;
    }

    public override void Initialize()
    {
        ReadPlaqueNode.Professor = ProfessorName;
        GetAdviceNode.Professor = ProfessorName;
    }

    public override void SetProf(string professorName)
    {
        ProfessorName = professorName;
    }

    public override BehaviorResult Process()
    {
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
    }
}