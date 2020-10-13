using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TerrainGenerator gen = target as TerrainGenerator;

        if (gen.AutoUpdateMesh)
        {
            //gen.GenerateNewTerrain();
        }
    }
    
}