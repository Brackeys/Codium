using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ShadowSnap))]

class ShadowGenEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		ShadowSnap myTarget = (ShadowSnap)target;

		if (GUILayout.Button("Snap Shadow"))
		{
			myTarget.GetComponent<ShadowSnap>().Snap();
		}
	}
}