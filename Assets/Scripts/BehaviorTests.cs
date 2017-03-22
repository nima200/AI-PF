using System.Collections;
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
            "Clark Verbrugge"
        };
        

        var paulKryPlaque = new ReadPlaque();
        var paulKryAdvice = new GetAdvice(); 
        var paulKryAdviceSequence = new ProfAdviceSequence(paulKryPlaque, paulKryAdvice);

        var clarkPlaque = new ReadPlaque();
        var clarkAdvice = new GetAdvice();
        var clarkAdviceSequence = new ProfAdviceSequence(clarkPlaque, clarkAdvice);
        var randomprofs = new RandomProfSelector(profNameList);
        randomprofs.Add(paulKryAdviceSequence);
        randomprofs.Add(clarkAdviceSequence);

//        LeafNode getAdviceNode = new GetAdvice();
//        LeafNode testleaf2 = new TestLeaf2();
//        CompositeNode sequenceNode = new SequenceNode();
//        sequenceNode.Add(getAdviceNode);
//        sequenceNode.Add(testleaf2);
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
