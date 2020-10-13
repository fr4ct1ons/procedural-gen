using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineSpawner))]
public class LineSpawnerEditor : Editor
{
    private LineSpawner lineSpawner;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private void OnSceneGUI()
    {
        lineSpawner = target as LineSpawner;
        handleTransform = lineSpawner.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local?
            handleTransform.rotation : Quaternion.identity;
        Vector3 p0, p1;
        p0 = handleTransform.TransformPoint(lineSpawner.p0);
        p1 = handleTransform.TransformPoint(lineSpawner.p1);

        Handles.color = Color.yellow;
        Handles.DrawLine(p0, p1);
        Handles.DoPositionHandle(p0, handleRotation);
        Handles.DoPositionHandle(p1, handleRotation);
        
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck()) 
        {
            Undo.RecordObject(lineSpawner, "Move p0");
            //EditorUtility.SetDirty(line);
            lineSpawner.p0 = handleTransform.InverseTransformPoint(p0);
        }
        
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck()) 
        {
            Undo.RecordObject(lineSpawner, "Move p1");
            //EditorUtility.SetDirty(line);
            lineSpawner.p1 = handleTransform.InverseTransformPoint(p1);
        }
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn all"))
        {
            lineSpawner.SpawnAll();
        }
    }
}