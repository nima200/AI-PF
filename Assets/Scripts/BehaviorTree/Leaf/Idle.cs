using UnityEngine;

public class Idle : LeafNode
{
    public string Professor;
    public bool FinishedIdle { get; set; }

    public override void Initialize()
    {
        int random = Random.Range(0, 10);
        if (random < 5)
        {
            Print("Going into idle");
            ActionManager.GetInstance().GoIdle(this);
        }
        else
        {
            FinishedIdle = true;
        }
    }

    public override BehaviorResult Process()
    {
        return !FinishedIdle ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
    }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }
}