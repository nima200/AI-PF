using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomSelector : CompositeNode
{
    private BehaviorNode _previousRandom;
    private BehaviorNode _currentRandom;
    private readonly CappedQueue<Professor> _memory = new CappedQueue<Professor>(4);
    /// <summary>
    /// Attempts to retrieve a random child among all children nodes and processes it.
    /// This class has been somewhat tweaked for the context of this assignment. 
    /// It has a capped queue data structure in it which is defined as memory. Once a child node 
    /// is visited, the memory enqueues the child node's professor and if the memory is ever at its capacity
    /// then the first thing inserted into it is popped of the queue, so that it can never hold memory 
    /// beyond its capacity.
    /// </summary>
    /// <returns></returns>
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
                return Result;
            case BehaviorResult.RUNNING:
                Result = BehaviorResult.RUNNING;
                return Result;
        }
        Result = BehaviorResult.RUNNING;
        return Result;
    }

    /// <summary>
    /// Attempts to initialize a random child.
    /// </summary>
    public override void Initialize()
    {
        _currentRandom = GetRandomChild();
        _currentRandom.Initialize();
    }

    public override void Reset()
    {
        base.Reset();
        _previousRandom = _currentRandom;
    }
    /// <summary>
    /// Gets a random child assuring the new random is not the previous random picked.
    /// </summary>
    /// <returns></returns>
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