using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public Transform[] Targets;
    public Transform Target;
    private float speed = 20;
    private Vector3[] _path;
    private int _targetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
    }

    /// <summary>
    /// Function that runs once the path is found to the target location.
    /// </summary>
    /// <param name="newPath">The path</param>
    /// <param name="pathSuccessful">The result of finding a path</param>
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (!pathSuccessful) return;
        _path = newPath;
        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
    }

    /// <summary>
    /// The movement to the target location, once a path has been found.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPath()
    {
        var currentWayPoint = _path[0];
        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                _targetIndex++;
                // If target has been reached, break the coroutine.
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }
                currentWayPoint = _path[_targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (_path == null) return;
        for (int i = _targetIndex; i < _path.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(_path[i], Vector3.one);
            Gizmos.DrawLine(i == _targetIndex ? transform.position : _path[i - 1], _path[i]);
        }
    }
}
