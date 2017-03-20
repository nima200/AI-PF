using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    private readonly BehaviorNode _root;

    public BehaviorTree(BehaviorNode root)
    {
        _root = root;
    }

    public void ResetTree()
    {
        _root.Reset();
    }

    public void InitializeTree()
    {
        _root.Initialize();
    }

    public BehaviorResult ExecuteTree()
    {
        return _root.Process();
    }
}