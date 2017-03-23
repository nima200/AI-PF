using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    private float speed = 20;
    private Vector3[] path;
    private int targetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }


    private IEnumerator FollowPath()
    {
        var currentWayPoint = path[0];
        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path == null) return;
        for (int i = targetIndex; i < path.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(path[i], Vector3.one);
            Gizmos.DrawLine(i == targetIndex ? transform.position : path[i - 1], path[i]);
        }
    }
}
