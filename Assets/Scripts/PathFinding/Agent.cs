using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public AStar AStar;
    public Grid Grid;
    public List<Node> Path;

	private void Start () {
		
	}

	private void Update () {
	    if (AStar.FoundPath)
	    {
	        Path = Grid.Path;
            var currentPosition = Grid.NodeFromWorldPoint(transform.position);
	        currentPosition.Walkable = false;
            
	    }
	}
}
