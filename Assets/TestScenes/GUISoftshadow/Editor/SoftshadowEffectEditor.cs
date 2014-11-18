using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SoftshadowEffect))]
public class SoftshadowEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SoftshadowEffect effect = (SoftshadowEffect)target;
        if(GUILayout.Button("Generate Softshadow"))
        {
            effect.GenerateBoxShadow();
        }
    }
}