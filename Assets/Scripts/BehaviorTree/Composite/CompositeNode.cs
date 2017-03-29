using System.Collections.Generic;

public abstract class CompositeNode : BehaviorNode
{
    protected List<BehaviorNode> ChildrenNodes;
    protected CompositeNode()
    {
        ChildrenNodes = new List<BehaviorNode>();
    }

    /// <summary>
    /// Method that adds a child to the list of children that the composite node has.
    /// </summary>
    /// <param name="child">The child behavior node.</param>
    public void Add(BehaviorNode child)
    {
        ChildrenNodes.Add(child);
    }

    /// <summary>
    /// Attempts to initialize all children of the composite node, all at once.
    /// </summary>
    public override void Initialize()
    {
        foreach (var node in ChildrenNodes)
        {
            node.Initialize();
        }
    }

    /// <summary>
    /// Attempts to reset all children of the composite node, all at once.
    /// </summary>
    public override void Reset()
    {
        Initialized = false;
        foreach (var node in ChildrenNodes)
        {
            node.Reset();
        }
    }

    /// <summary>
    /// Sets the professor attribute of this node as well as all its children.
    /// </summary>
    /// <param name="professor">The professor to set</param>
    public override void SetProf(Professor professor)
    {
        Professor = professor;
        foreach (var node in ChildrenNodes)
        {
            node.SetProf(professor);
        }
    }
    /// <summary>
    /// Sets the plaque attribute of this node as well as all its children.
    /// </summary>
    /// <param name="plaque">The plaque to set</param>
    public override void SetPlaque(Plaque plaque)
    {
        Plaque = plaque;
        foreach (var node in ChildrenNodes)
        {
            node.SetPlaque(plaque);
        }
    }

    /// <summary>
    /// Sets the agent attribute of this node as well as all its children.
    /// </summary>
    /// <param name="agent"></param>
    public override void SetAgent(Agent agent)
    {
        base.SetAgent(agent);
        foreach (var node in ChildrenNodes)
        {
            node.SetAgent(agent);
        }
    }
}