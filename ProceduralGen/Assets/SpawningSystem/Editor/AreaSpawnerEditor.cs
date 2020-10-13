using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AreaSpawner))]
public class AreaSpawnerEditor : Editor
{
    private AreaSpawner system;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private void OnSceneGUI()
    {
        system = target as AreaSpawner;
        handleTransform = system.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local?
            handleTransform.rotation : Quaternion.identity;
        Vector3 topR, bottomL;
        Vector3[] p = new Vector3[4];
        topR = handleTransform.TransformPoint(system.bottomL);
        bottomL = handleTransform.TransformPoint(system.topR);

        p[0].Set(bottomL.x, handleTransform.position.y, bottomL.z);
        p[1].Set(bottomL.x, handleTransform.position.y, topR.z);
        p[2].Set(topR.x, handleTransform.position.y, bottomL.z);
        p[3].Set(topR.x, handleTransform.position.y, topR.z);
        
        Handles.color = Color.yellow;
        Handles.DrawLine(p[0], p[1]);
        Handles.DrawLine(p[1], p[3]);
        Handles.DrawLine(p[3], p[2]);
        Handles.DrawLine(p[2], p[0]);
        
        Handles.DoPositionHandle(topR, handleRotation);
        Handles.DoPositionHandle(bottomL, handleRotation);
        
        EditorGUI.BeginChangeCheck();
        topR = Handles.DoPositionHandle(topR, handleRotation);
        if (EditorGUI.EndChangeCheck()) 
        {
            Undo.RecordObject(system, "Move p0");
            system.bottomL = handleTransform.InverseTransformPoint(topR);
        }
        
        EditorGUI.BeginChangeCheck();
        bottomL = Handles.DoPositionHandle(bottomL, handleRotation);
        if (EditorGUI.EndChangeCheck()) 
        {
            Undo.RecordObject(system, "Move p1");
            system.topR = handleTransform.InverseTransformPoint(bottomL);
        }
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn all"))
        {
            system.SpawnAll();
        }
    }
}