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
            "Clark Verbrugge",
            "Prakash Panengaden"
        };
        
        var paulKryFind = new FindProf();
        var paulKryPlaque = new ReadPlaque();
        var paulKryAdvice = new GetAdvice();
	    var paulKryAdviceSequence = new ProfAdviceSequence(paulKryFind, paulKryPlaque, paulKryAdvice);
        /*var paulKry = new SequenceNode();
        paulKry.Add(paulKryAdviceSequence);
        paulKry.Add(new Idle());*/

        var clarkFind = new FindProf();
        var clarkPlaque = new ReadPlaque();
        var clarkAdvice = new GetAdvice();
        var clarkAdviceSequence = new ProfAdviceSequence(clarkFind, clarkPlaque, clarkAdvice);
        /*var clark = new SequenceNode();
        clark.Add(clarkAdviceSequence);
        clark.Add(new Idle());*/

        var prakashFind = new FindProf();
        var prakashPlaque = new ReadPlaque();
        var prakashAdvice = new GetAdvice();
        var prakashAdviceSequence = new ProfAdviceSequence(prakashFind, prakashPlaque, prakashAdvice);
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


