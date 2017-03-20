using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTests : MonoBehaviour
{
    public ActionManager ActionManagerPrefab;
    BehaviorResult myresult = BehaviorResult.FAIL;
    private BehaviorTree _myTree;
	private void Awake ()
	{
	    Instantiate(ActionManagerPrefab);
        LeafNode getAdviceNode = new GetAdvice();
        LeafNode testleaf2 = new TestLeaf2();
        CompositeNode sequenceNode = new SequenceNode();
        sequenceNode.Add(getAdviceNode);
        sequenceNode.Add(testleaf2);
        _myTree = new BehaviorTree(sequenceNode);
        _myTree.InitializeTree();
	}

	private void Update ()
	{
	    if (myresult != BehaviorResult.SUCCESS)
	    {
	        myresult =_myTree.ExecuteTree();
	    }
	}
}
