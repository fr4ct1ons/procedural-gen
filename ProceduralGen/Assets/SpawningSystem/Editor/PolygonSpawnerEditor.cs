using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonSpawner))]
 public class PolygonSpawnerEditor : Editor
 {
     private PolygonSpawner polygonSpawner;
     private Transform handleTransform;
     private Quaternion handleRotation;
     private List<Vector3> transformHandles;
     private Vector3 bufferVector; 
     private void OnSceneGUI()
     {
         polygonSpawner = target as PolygonSpawner;
         if (polygonSpawner.points.Count != 0)
         {
             handleTransform = polygonSpawner.transform;
             handleRotation = Tools.pivotRotation == PivotRotation.Local
                 ? handleTransform.rotation
                 : Quaternion.identity;

             /*for (int i = 0; i < polygonSpawner.points.Count; i++)
             {
                 transformHandles[i] = handleTransform.TransformPoint(polygonSpawner.points[i]);
             }*/

             for (int i = 0; i < polygonSpawner.points.Count; i++)
             {
                 bufferVector = handleTransform.TransformPoint(polygonSpawner.points[i]);
                 bufferVector = Handles.DoPositionHandle(bufferVector, Quaternion.identity);

                 polygonSpawner.points[i] = handleTransform.InverseTransformPoint(bufferVector);
                 //polygonSpawner.points[i].Set(polygonSpawner.points[i].x, 0, polygonSpawner.points[i].z);
                 //polygonSpawner.points[i] = handleTransform.InverseTransformPoint(polygonSpawner.points[i]);
             }
             
             Handles.color = Color.yellow;
             for (int i = 0; i < polygonSpawner.points.Count; i++)
             {
                 Handles.DrawLine(handleTransform.TransformPoint(polygonSpawner.points[i]), 
                     handleTransform.TransformPoint(polygonSpawner.points[Mathf.FloorToInt(Mathf.Repeat(i + 1, polygonSpawner.points.Count))]));
             }
         }
     }

     public override void OnInspectorGUI()
     {
         base.OnInspectorGUI();
         if (GUILayout.Button("Add point"))
         {
             polygonSpawner.points.Add(new Vector3(polygonSpawner.points.Count, 0, polygonSpawner.points.Count));
         }

         if (GUILayout.Button("Spawn all"))
         {
             polygonSpawner.SpawnAll();
         }
     }
 }