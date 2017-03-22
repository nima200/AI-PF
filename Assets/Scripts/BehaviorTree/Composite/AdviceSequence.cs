using System;

public class AdviceSequence : SequenceNode
{
    public InteractionSequence InteractionSequence;
    public Idle Idle;

    public override void SetProf(string professorName)
    {
        InteractionSequence.SetProf(professorName);
    }

    public AdviceSequence(InteractionSequence interactionSequence, Idle idle)
    {
        InteractionSequence = interactionSequence;
        Idle = idle;
    }

    public override void Initialize()
    {
        Initialized = true;
        InteractionSequence.Initialize();
    }

    public override BehaviorResult Process()
    {
        if (InteractionSequence.Initialized)
        {
            switch (InteractionSequence.Process())
            {
                case BehaviorResult.FAIL:
                    Result = BehaviorResult.FAIL;
                    return Result;
                case BehaviorResult.SUCCESS:
                    if (Idle.Initialized)
                    {
                        switch (Idle.Process())
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
                    Idle.Initialize();
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
        InteractionSequence.Initialize();
        Result = BehaviorResult.RUNNING;
        return Result;
    }
}