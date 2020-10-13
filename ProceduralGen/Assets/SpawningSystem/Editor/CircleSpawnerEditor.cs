using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircleSpawner))]
public class CircleSpawnerEditor : Editor
{
    private CircleSpawner system;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private void OnSceneGUI()
    {
        system = target as CircleSpawner;
        handleTransform = system.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local?
            handleTransform.rotation : Quaternion.identity;

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(system.transform.position, system.transform.up, system.radius);
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