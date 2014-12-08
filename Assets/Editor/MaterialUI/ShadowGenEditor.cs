using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ShadowGen))]

class ShadowSnapEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		ShadowGen myTarget = (ShadowGen)target;

		if (GUILayout.Button("Generate Shadow"))
		{
			myTarget.GetComponent<ShadowGen>().GenerateShadowFromImage();
		}
	}
}