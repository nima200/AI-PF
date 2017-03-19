using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

    private void OnSceneGUI()
    {
        var fow = (FieldOfView) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
        var viewAngleA = fow.DirFromAngle(-fow.ViewAngle / 2, false);
        var viewAngleB = fow.DirFromAngle(fow.ViewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);

        Handles.color = Color.red;
        foreach (var visibleTarget in fow.VisibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
