using UnityEditor;


[CustomEditor(typeof(NoiseMesh))]
public class NoiseMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        NoiseMesh gen = target as NoiseMesh;
        EditorGUILayout.LabelField("Mesh will be " + gen.GetXLength() + "x" + gen.GetZLength());

        if (gen.AutoUpdateMesh)
        {
            gen.GenerateMesh();
        }
    }
    
}