using System;
using System.Collections.Generic;
using System.Linq;


public class Cell
{
    public int X;
    public int Y;
    public int TimeStepWindow { get; private set; }
    public Node GenericNode;
    private readonly List<Node> _timeStepNodes;
    public Agent[] ReservationTable { get; private set; }

    /// <summary>
    /// Constructor creates a cell and assigns its generic node.
    /// It also creates a reservation table of size given by the TimeStepWindow parameter.
    /// </summary>
    /// <param name="x">The X location of the cell, in the grid.</param>
    /// <param name="y">The Y location of the cell, in the grid.</param>
    /// <param name="timeStepWindow">The size of the reservation window</param>
    /// <param name="genericNode">The generic node of this cell that essentially represents the cell once the cell is accessed at a time outside of its window of reservation.</param>
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

    /// <summary>
    /// Checks whether the cell is walkable or not at a given time step. If the time step requested is 
    /// outside of the boundaries of the reservation table's window, the generic node is checked.
    /// </summary>
    /// <param name="timeStep">The time step to check whether a cell is walkable at that time or not.</param>
    /// <returns>Result of whether the cell is walkable at the given time or not.</returns>
    public bool Walkable(int timeStep)
    {
        return timeStep > _timeStepNodes.Count - 1 ? GenericNode.Walkable : _timeStepNodes[timeStep].Walkable;
    }

    /// <summary>
    /// Returns the node at a given time step. If the time step request is outside of the boundaries of the
    /// reservation table's window, the generic node is checked.
    /// </summary>
    /// <param name="timeStep">The time step to check whether a cell is walkable at that time or not.</param>
    /// <returns>The node at the requested time step</returns>
    public Node GetNode(int timeStep)
    {
        return timeStep > _timeStepNodes.Count - 1 ? GenericNode : _timeStepNodes[timeStep];
    }

    /// <summary>
    /// Attempts to reserve a cell at a given time step. If the time step requested is outside of the boundaries 
    /// of the reservation table's window, void is returned right away.
    /// </summary>
    /// <param name="agent">The agent to reserve the cell for</param>
    /// <param name="timeStep">The time step to reserve at.</param>
    public void Reserve(Agent agent, int timeStep)
    {
        if (timeStep > ReservationTable.Length - 1) return;
        ReservationTable[timeStep] = agent;
    }
    /// <summary>
    /// Checks whether the cell at the given time step is reserved or not.
    /// If the time step requested is outside of the boundaries of the reservation window, then
    /// false is returned assuming the cell is not reserved.
    /// </summary>
    /// <param name="timeStep"></param>
    /// <returns></returns>
    public bool IsReserved(int timeStep)
    {
        return timeStep <= ReservationTable.Length - 1 && ReservationTable[timeStep] != null;
    }
    /// <summary>
    /// Basic setter for the GCost.
    /// </summary>
    /// <param name="gCost">The new GCost of the cell.</param>
    public void SetGCost(int gCost)
    {
        GenericNode.GCost = gCost;
        foreach (var node in _timeStepNodes)
        {
            node.GCost = gCost;
        }
    }
    /// <summary>
    /// Basic getter for the GCost.
    /// </summary>
    /// <returns>The GCost of the generic node of the cell.</returns>
    public int GetGCost()
    {
        return GenericNode.GCost;
    }

    /// <summary>
    /// BAsic setter for HCost.
    /// </summary>
    /// <param name="hCost">The new HCost of the cell.</param>
    public void SetHCost(int hCost)
    {
        GenericNode.HCost = hCost;
        foreach (var node in _timeStepNodes)
        {
            node.HCost = hCost;
        }
    }

    /// <summary>
    /// Attempts to set the parent of all reservation window nodes, as well as the generic node.
    /// </summary>
    /// <param name="parentNode">The parent to set for the cell's node(s)</param>
    public void SetParent(Node parentNode)
    {
        GenericNode.Parent = parentNode;
        foreach (var node in _timeStepNodes)
        {
            node.Parent = parentNode;
        }
    }

    /// <summary>
    /// Method that resets an agent's reservations in the reservation table, removing them essentially.
    /// </summary>
    /// <param name="caller">The agent to reset reservations for.</param>
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