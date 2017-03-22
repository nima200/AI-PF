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
//	    var paulKryInteraction = new InteractionSequence(paulKryFind, paulKryPlaque, paulKryAdvice);
	    var paulKryInteraction = new InteractionSequence();
        paulKryInteraction.Add(paulKryFind);
        paulKryInteraction.Add(paulKryPlaque);
        paulKryInteraction.Add(paulKryAdvice);
        var paulKryIdle = new Idle();
        var paulKryAdviceSequence = new AdviceSequence(paulKryInteraction, paulKryIdle);
        /*var paulKry = new SequenceNode();
        paulKry.Add(paulKryAdviceSequence);
        paulKry.Add(new Idle());*/

        var clarkFind = new FindProf();
        var clarkPlaque = new ReadPlaque();
        var clarkAdvice = new GetAdvice();
//        var clarkInteraction = new InteractionSequence(clarkFind, clarkPlaque, clarkAdvice);
        var clarkInteraction = new InteractionSequence();
        clarkInteraction.Add(clarkFind);
        clarkInteraction.Add(clarkPlaque);
        clarkInteraction.Add(clarkAdvice);
        var clarkIdle= new Idle();
	    var clarkAdviceSequence = new AdviceSequence(clarkInteraction, clarkIdle);

        /*var clark = new SequenceNode();
        clark.Add(clarkAdviceSequence);
        clark.Add(new Idle());*/

        var prakashFind = new FindProf();
        var prakashPlaque = new ReadPlaque();
        var prakashAdvice = new GetAdvice();
//        var prakashInteraction = new InteractionSequence(prakashFind, prakashPlaque, prakashAdvice);
        var prakashInteraction = new InteractionSequence();
        prakashInteraction.Add(prakashFind);
        prakashInteraction.Add(prakashPlaque);
        prakashInteraction.Add(prakashAdvice);
        var prakashIdle = new Idle();
        var prakashAdviceSequence = new AdviceSequence(prakashInteraction, prakashIdle);

        /*var prakash = new SequenceNode();
        prakash.Add(prakashAdviceSequence);
        prakash.Add(new Idle());*/

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
	        _myresult =_myTree.ExecuteTree();
	    }
	}
}


