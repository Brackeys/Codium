using UnityEngine;
using UnityEditor;
using LitJson;
using Codium.GSFU;

[CustomEditor(typeof(LessonData))]
public class LessonDataEditor : Editor {

	public override void OnInspectorGUI () {
		
		LessonData lesson = target as LessonData;
		
		if (GUILayout.Button("Update from Spreadsheet")) {
			//UpdateFromSpreadsheet(lesson);
		}
		
		base.OnInspectorGUI();
	}
	
}
