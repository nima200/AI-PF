using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
    public Vector2 SpawnArea;
    public int NumberOfStudents;
    public Agent StudentPrefab;
    private void Awake()
    {
        for (int i = 0; i < NumberOfStudents; i++)
        {
            var student = Instantiate(StudentPrefab);
            student.transform.SetParent(GameObject.Find("Students").transform);
            student.transform.localPosition = new Vector3(Random.Range(-SpawnArea.x, SpawnArea.x), Random.Range(-SpawnArea.y, SpawnArea.y));
        }
    }
}
