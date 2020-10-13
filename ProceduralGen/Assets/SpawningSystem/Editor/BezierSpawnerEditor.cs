using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpawner))]
public class BezierSpawnerEditor : Editor
{

    private int lineSteps = 10;
    private float directionScale = 0.5f;
    private BezierSpawner spawner;
    private Transform handleTransform;
    private Quaternion handleRotation;
    

    private void OnSceneGUI () {
        spawner = target as BezierSpawner;
        handleTransform = spawner.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.blue;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        //ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.yellow, null, 2f);
        
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn all"))
        {
            spawner.SpawnAll();
        }
    }

    private Vector3 ShowPoint (int index) {
        Vector3 point = handleTransform.TransformPoint(spawner.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spawner, "Move Point");
            spawner.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
    
    private void ShowDirections () {
        Handles.color = Color.green;
        Vector3 point = spawner.GetPoint(0f);
        Handles.DrawLine(point, point + spawner.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++) {
            point = spawner.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + spawner.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }
}