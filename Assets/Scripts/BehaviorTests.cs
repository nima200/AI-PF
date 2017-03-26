using System.Collections.Generic;
using UnityEngine;

public class BehaviorTests : MonoBehaviour
{
    public ActionManager ActionManagerPrefab;
    private BehaviorResult _myresult = BehaviorResult.FAIL;
    private BehaviorTree _myTree;
	private void Awake ()
	{
	    Instantiate(ActionManagerPrefab);
        var profNameList = new List<string>()
        {
            "Paul Kry",
            "Clark",
            "Prakash"
        };
        
        var paulKryFind = new FindProf();
        var paulKryPlaque = new ReadPlaque();
        var paulKryAdvice = new GetAdvice();
	    var paulKryInteraction = new InteractionSequence();
        paulKryInteraction.Add(paulKryFind);
        paulKryInteraction.Add(paulKryPlaque);
        paulKryInteraction.Add(paulKryAdvice);
        var paulKryIdle = new Idle();
        var paulKryAdviceSequence = new InteractionSequence();
        paulKryAdviceSequence.Add(paulKryInteraction);
        paulKryAdviceSequence.Add(paulKryIdle);

        var clarkFind = new FindProf();
        var clarkPlaque = new ReadPlaque();
        var clarkAdvice = new GetAdvice();
        var clarkInteraction = new InteractionSequence();
        clarkInteraction.Add(clarkFind);
        clarkInteraction.Add(clarkPlaque);
        clarkInteraction.Add(clarkAdvice);
        var clarkIdle= new Idle();
        var clarkAdviceSequence = new InteractionSequence();
        clarkAdviceSequence.Add(clarkInteraction);
        clarkAdviceSequence.Add(clarkIdle);

        var prakashFind = new FindProf();
        var prakashPlaque = new ReadPlaque();
        var prakashAdvice = new GetAdvice();
        var prakashInteraction = new InteractionSequence();
        prakashInteraction.Add(prakashFind);
        prakashInteraction.Add(prakashPlaque);
        prakashInteraction.Add(prakashAdvice);
        var prakashIdle = new Idle();
        var prakashAdviceSequence = new InteractionSequence();
        prakashAdviceSequence.Add(prakashInteraction);
        prakashAdviceSequence.Add(prakashIdle);

        var randomprofs = new RandomProfSelector(profNameList);

        randomprofs.Add(paulKryAdviceSequence);
        randomprofs.Add(clarkAdviceSequence);
        randomprofs.Add(prakashAdviceSequence);

        _myTree = new BehaviorTree(randomprofs);
        _myTree.InitializeTree();
	}

	private void Update ()
	{
	    if (_myresult != BehaviorResult.SUCCESS)
	    {
	        _myresult = _myTree.ExecuteTree();
	    }
	    else
	    {
            _myresult = BehaviorResult.RUNNING;
	        _myTree.ResetTree();
            _myTree.InitializeTree();
	    }

	}
}


