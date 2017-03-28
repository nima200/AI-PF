using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector3[] Path;
    public Transform IdleWaypointsParent;
    private List<IdleWaypoint> _idleWaypoints;
    public Transform Target;
    public Professor TargetProfessor;
    public bool ReachedTarget;
    public bool FinishedTalking;
    [Range(1, 50)]
    public float Speed = 5;
    private int _waypointIndex;
    private Grid _grid;

    private void Awake()
    {
        _grid = GameObject.Find("A*").GetComponent<Grid>();
        _idleWaypoints = IdleWaypointsParent.GetComponentsInChildren<IdleWaypoint>().ToList();
        TargetProfessor =
            GetComponent<Behavior>().Professors[Random.Range(0, GetComponent<Behavior>().Professors.Count)];
    }

	private void Update () {
	    if (Input.GetKeyDown(KeyCode.F1))
	    {
            ResetPath();
	        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
	    }
	    if (Input.GetKeyDown(KeyCode.F2))
	    {
            ResetPath();
	        RequestIdlePath();
	    }
	}

    private void ResetPath()
    {
        ReachedTarget = false;
        FinishedTalking = false;
        _waypointIndex = 0;
    }

    /// <summary>
    /// Method that moves the target towards its target. It is called automatically 
    /// by the path request manager once the path has been found for the request.
    /// </summary>
    /// <param name="newPath">The path</param>
    /// <param name="pathFound">The result of calculating a path</param>
    public void OnPathFound(Vector3[] newPath, bool pathFound)
    {
        if (!pathFound) return;
        Path = newPath;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    public void StopPath()
    {
        StopCoroutine("FollowPath");
    }
    /// <summary>
    /// Moves the agent towards the waypoint it's supposed to be processing in the path.
    /// The waypoint starts from 0, and goes on proceeding through the path until the waypointindex
    /// has reached the length of the path, meaning the agent has reached the last waypoint in the path.
    /// Once the agent reaches the window of reservation # of waypoints in its path, it attempts to stop 
    /// the movement and request a new path from A* path finder.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPath()
    {
        if (Path.Length > 0)
        {
            var currentWaypoint = Path[0];
            while (true)
            {
                // Agent has reached its window of reservation and now requests a new path.
                if (Path.Length > _grid.TimeStepWindow && _waypointIndex == _grid.TimeStepWindow)
                {
                    _grid.ResetGridReservations(this);
                    ResetPath();
                    PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
                    yield break;
                }
                if (transform.position == currentWaypoint)
                {
                    _waypointIndex++;
                    // Agent has reached target
                    if (_waypointIndex >= Path.Length)
                    {
                        ReachedTarget = true;
                        yield break;
                    }
                    currentWaypoint = Path[_waypointIndex];
                }
//                transform.position = currentWaypoint;
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (Path == null) return;
        for (int i = _waypointIndex; i < Path.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(Path[i], Vector3.one * 2);
        }
    }

    public void RequestIdlePath()
    {
        // Find a random waypoint to move to for idle behavior
        var randomIdleWaypointIndex = Random.Range(0, _idleWaypoints.Count);
        var randomIdleWaypoint = _idleWaypoints[randomIdleWaypointIndex].transform;
        Target = randomIdleWaypoint;
        // Stop the coroutine and reset the grid reservations if any
        _grid.ResetGridReservations(this);
        ResetPath();
        StopPath();
        // Request a new path from the request manager with the random idle waypoint as a target.
        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
    }

    public void RequestPath(Transform target)
    {
        _grid.ResetGridReservations(this);
        ResetPath();
        StopPath();
        SetTarget(target);
        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
    }

    private void SetTarget(Transform target)
    {
        Target = target;
    }

    public void ChangeTargetProfessor(Professor professor)
    {
        TargetProfessor = professor;
    }
}
