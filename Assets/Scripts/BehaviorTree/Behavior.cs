﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public ActionManager ActionManagerPrefab;
    private BehaviorResult _myresult = BehaviorResult.FAIL;
    private BehaviorTree _myTree;
    [HideInInspector]
    public List<Professor> Professors;
    [HideInInspector]
    public List<Plaque> Plaques;
    [HideInInspector]
    public Agent Agent;
	private void Start ()
	{
        Agent = GetComponent<Agent>();
        Plaques = Agent.Plaques;
        Professors = Agent.Professors;
	    

        Instantiate(ActionManagerPrefab);
        
        var paulKryFind = new FindProf();
        var paulKryPlaque = new ReadPlaque();
        var paulKryAdvice = new GetAdvice();
	    var paulKryInteraction = new InteractionSequence();
        var paulKryIdle = new Idle();
        paulKryInteraction.Add(paulKryFind);
        paulKryInteraction.Add(paulKryPlaque);
        paulKryInteraction.Add(paulKryAdvice);
        var paulKryAdviceSequence = new InteractionSequence();
        paulKryAdviceSequence.Add(paulKryInteraction);
        paulKryAdviceSequence.Add(paulKryIdle);

        var clarkFind = new FindProf();
        var clarkPlaque = new ReadPlaque();
        var clarkAdvice = new GetAdvice();
        var clarkInteraction = new InteractionSequence();
        var clarkIdle = new Idle();
        clarkInteraction.Add(clarkFind);
        clarkInteraction.Add(clarkPlaque);
        clarkInteraction.Add(clarkAdvice);
        var clarkAdviceSequence = new InteractionSequence();
        clarkAdviceSequence.Add(clarkInteraction);
        clarkAdviceSequence.Add(clarkIdle);

        var prakashFind = new FindProf();
        var prakashPlaque = new ReadPlaque();
        var prakashAdvice = new GetAdvice();
        var prakashInteraction = new InteractionSequence();
        var prakashIdle = new Idle();
        prakashInteraction.Add(prakashFind);
        prakashInteraction.Add(prakashPlaque);
        prakashInteraction.Add(prakashAdvice);
        var prakashAdviceSequence = new InteractionSequence();
        prakashAdviceSequence.Add(prakashInteraction);
        prakashAdviceSequence.Add(prakashIdle);

        var kimFind = new FindProf();
        var kimPlaque = new ReadPlaque();
	    var kimAdvice = new GetAdvice();
        var kimInteraction = new InteractionSequence();
        var kimIdle = new Idle();
        kimInteraction.Add(kimFind);
        kimInteraction.Add(kimPlaque);
        kimInteraction.Add(kimAdvice);
        var kimAdviceSequence = new InteractionSequence();
        kimAdviceSequence.Add(kimInteraction);
        kimAdviceSequence.Add(kimIdle);

        var futureFind = new FindProf();
        var futurePlaque = new ReadPlaque();
        var futureAdvice = new GetAdvice();
        var futureInteraction = new InteractionSequence();
        var futureIdle = new Idle();
        futureInteraction.Add(futureFind);
        futureInteraction.Add(futurePlaque);
        futureInteraction.Add(futureAdvice);
        var futureAdviceSequence = new InteractionSequence();
        futureAdviceSequence.Add(futureInteraction);
        futureAdviceSequence.Add(futureIdle);

        var drakeFind = new FindProf();
        var drakePlaque = new ReadPlaque();
        var drakeAdvice = new GetAdvice();
        var drakeInteraction = new InteractionSequence();
        var drakeIdle = new Idle();
        drakeInteraction.Add(drakeFind);
        drakeInteraction.Add(drakePlaque);
        drakeInteraction.Add(drakeAdvice);
        var drakeAdviceSequence = new InteractionSequence();
        drakeAdviceSequence.Add(drakeInteraction);
        drakeAdviceSequence.Add(drakeIdle);

        var randomprofs = new RandomProfSelector(Professors, Plaques, Agent);

        randomprofs.Add(paulKryAdviceSequence);
        randomprofs.Add(clarkAdviceSequence);
        randomprofs.Add(prakashAdviceSequence);
        randomprofs.Add(kimAdviceSequence);
        randomprofs.Add(futureAdviceSequence);
        randomprofs.Add(drakeAdviceSequence);

        _myTree = new BehaviorTree(randomprofs);
        _myTree.InitializeTree();
	}

	private void Update ()
	{
	    _myresult = _myTree.ExecuteTree();
	    switch (_myresult)
	    {
	        case BehaviorResult.FAIL:
	        case BehaviorResult.SUCCESS:
                _myTree.ResetTree();
                _myTree.InitializeTree();
	            break;
	        case BehaviorResult.RUNNING:
	            break;
	    }
	}
}


