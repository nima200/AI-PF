using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomSelector : CompositeNode
{
    private BehaviorNode _previousRandom;
    private BehaviorNode _currentRandom;
    private readonly CappedQueue<Professor> _memory = new CappedQueue<Professor>(4);
    public override BehaviorResult Process()
    {
        switch (_currentRandom.Process())
        {
            case BehaviorResult.FAIL:
                _memory.Enqueue(_currentRandom.Professor);
                Agent.Memory = _memory.Q.ToList();
                Result = BehaviorResult.FAIL;
                return Result;
            case BehaviorResult.SUCCESS:
                _memory.Enqueue(_currentRandom.Professor);
                Agent.Memory = _memory.Q.ToList();
                Result = BehaviorResult.SUCCESS;
                break;
            case BehaviorResult.RUNNING:
                Result = BehaviorResult.RUNNING;
                return Result;
        }
        Result = BehaviorResult.RUNNING;
        return Result;
    }

    public override void Initialize()
    {
        ChildrenNodes[Random.Range(0, ChildrenNodes.Count)].Initialize();
        _currentRandom = GetRandomChild();
    }

    public override void Reset()
    {
        base.Reset();
        _previousRandom = _currentRandom;
    }

    private BehaviorNode GetRandomChild()
    {
        while (true)
        {
            int randomChildIndex = Random.Range(0, ChildrenNodes.Count);
            if (Agent.Memory.Contains(Agent.TargetProfessor))
                return ChildrenNodes.First(node => node.Professor == Agent.TargetProfessor);
            if (ChildrenNodes[randomChildIndex] == _previousRandom ||
                _memory.Contains(ChildrenNodes[randomChildIndex].Professor)) continue;
            return ChildrenNodes[randomChildIndex];
        }
    }
}