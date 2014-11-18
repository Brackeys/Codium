using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NebulaGenerator))]
public class NebulaGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        NebulaGenerator gen = (NebulaGenerator)target;
        if(GUILayout.Button("Generate Nebula"))
        {
            gen.GenerateNebula();
        }
    }
}