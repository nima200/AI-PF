using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    [HideInInspector]
    public Vector3[] Path;
    public Transform Target;
    public Professor TargetProfessor;
    [HideInInspector]
    public Professor PreviousProfessor;
    public Vector3 CurrentWaypoint;
    [Range(1, 30)]
    public float Speed = 5;
    public int WaypointIndex;
    private Grid _grid;
    public List<Plaque> Plaques;
    public List<Professor> Memory = new List<Professor>();
    public List<Professor> Professors { get; set; }
    [HideInInspector]
    public bool ReachedTarget;
    public List<Agent> OtherStudents = new List<Agent>();

    private void Awake()
    {
        _grid = GameObject.Find("A*").GetComponent<Grid>();
        Professors = new List<Professor>(Plaques.Count);
        foreach (var p in Plaques)
        {
            Professors.Add(p.Professor);
        }
        TargetProfessor = Professors[Random.Range(0, Professors.Count)];
    }

    private void Start()
    {
        OtherStudents = FindObjectsOfType<Agent>().ToList();
        OtherStudents.Remove(this);
    }

	private void Update () {
	    if (Input.GetKeyDown(KeyCode.F1))
	    {
            ResetPath();
	        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
	    }
	    if (Input.GetKeyDown(KeyCode.F3))
	    {
	        StopPath();
	    }
	}

    private void ResetPath()
    {
        ReachedTarget = false;
        WaypointIndex = 0;
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
    /// Attempts to stop the path and recalculate another A* once the waypoint index has reached the 
    /// cap of the reservation window. In this way, we can limit our reservation window to whatever we want without
    /// worrying about the depth of the tree cause it will get replaced once the request to re-pathfind is sent.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPath()
    {
        if (Path.Length > 0)
        {
            CurrentWaypoint = Path[0];
            while (true)
            {
                if (Path.Length > _grid.TimeStepWindow && WaypointIndex == _grid.TimeStepWindow)
                {
                    _grid.ResetGridReservations(this);
                    ResetPath();
                    PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
                    yield break;
                }
                if (transform.position == CurrentWaypoint)
                {
                    WaypointIndex++;
                    // agent has reached target
                    if (WaypointIndex >= Path.Length)
                    {
                        ReachedTarget = true;
                        yield break;
                    }
                    CurrentWaypoint = Path[WaypointIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, CurrentWaypoint, Speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void RequestPath(Transform target)
    {
        Target = target;
        StopPath();
        ResetPath();
        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
    }

    public void GetRandomProf()
    {
        PreviousProfessor = TargetProfessor;
        do
        {
            int random = Random.Range(0, Professors.Count);
            TargetProfessor = Professors[random];
        } while (TargetProfessor == PreviousProfessor);
    }

    public void OnDrawGizmos()
    {
        if (Path == null) return;
        for (int i = WaypointIndex; i < Path.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(Path[i], Vector3.one * 2);
        }
    }
}
