using System;
using System.Collections.Generic;
using System.Linq;

public class Pair<TA, TB>
{
    public TA AValue { get; set; }
    public TB BValue { get; set; }

    public Pair(TA aValue, TB bValue)
    {
        AValue = aValue;
        BValue = bValue;
    }
}


public class Cell
{
    public int X;
    public int Y;
    public int TimeStepWindow { get; private set; }
    public Node GenericNode;
    private readonly List<Node> _timeStepNodes;
    public Agent[] ReservationTable { get; private set; } 

    public Cell(int x, int y, int timeStepWindow, Node genericNode)
    {
        X = x;
        Y = y;
        TimeStepWindow = timeStepWindow;
        _timeStepNodes =
            Enumerable.Repeat(new Node(genericNode.Walkable, genericNode.WorldPosition, genericNode.X, genericNode.Y),
                TimeStepWindow).ToList();
        GenericNode = genericNode;
        ReservationTable = new Agent[TimeStepWindow];
    }

    public bool Walkable(int timeStep)
    {
        return timeStep > _timeStepNodes.Count - 1 ? GenericNode.Walkable : _timeStepNodes[timeStep].Walkable;
    }

    public Node GetNode(int timeStep)
    {
        return timeStep > _timeStepNodes.Count - 1 ? GenericNode : _timeStepNodes[timeStep];
    }

    public void Reserve(Agent agent, int timeStep)
    {
        if (timeStep > ReservationTable.Length - 1) return;
        ReservationTable[timeStep] = agent;
    }

    public bool IsReserved(int timeStep)
    {
        return timeStep <= ReservationTable.Length - 1 && ReservationTable[timeStep] != null;
    }

    public void SetGCost(int gCost)
    {
        GenericNode.GCost = gCost;
        foreach (var node in _timeStepNodes)
        {
            node.GCost = gCost;
        }
    }

    public void SetHCost(int hCost)
    {
        GenericNode.HCost = hCost;
        foreach (var node in _timeStepNodes)
        {
            node.HCost = hCost;
        }
    }

    public void SetParent(Node parentNode)
    {
        GenericNode.Parent = parentNode;
        foreach (var node in _timeStepNodes)
        {
            node.Parent = parentNode;
        }
    }

    public void ResetReservationTable(Agent caller)
    {
        for (int i = 0; i < ReservationTable.Length; i++)
        {
            if (ReservationTable[i] == caller)
            {
                ReservationTable[i] = null;
            }
        }
    }
}