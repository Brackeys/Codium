using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SnapButtonToText))]

class SnapButtonToTextEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		SnapButtonToText myTarget = (SnapButtonToText)target;

		if (GUILayout.Button("Snap"))
		{
			myTarget.GetComponent<SnapButtonToText>().Snap();
		}
	}
}