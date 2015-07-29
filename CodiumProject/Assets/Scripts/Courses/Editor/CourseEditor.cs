//-----------------------------------------------------------------
// Used in conjunktion with Course.cs
// Allows for creating and editing course data assets.
//-----------------------------------------------------------------


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Course)), CanEditMultipleObjects]
public class CourseEditor : Editor {
	//Creating new courses
	[MenuItem ("Course Editor/Create Course")]
	static void CreateCourse () {
		string path = EditorUtility.SaveFilePanel ("Create Course",
									"Assets/", "Custom Course.asset", "asset");

		if (path == "") {
			return;
		}
  
		path = FileUtil.GetProjectRelativePath(path);

		Course course = CreateInstance<Course>();
		course.ResetID();
		AssetDatabase.CreateAsset (course, path);

		AssetDatabase.SaveAssets();
	}

	//Property declaration
	public SerializedProperty title;
	public SerializedProperty author;
	public SerializedProperty description;
	public SerializedProperty category;
	public SerializedProperty difficulty;
	public SerializedProperty courseViews;

	//Editing the properties
	void OnEnable () {
		title = serializedObject.FindProperty ("title");
		author = serializedObject.FindProperty ("author");
		description = serializedObject.FindProperty ("description");
		category = serializedObject.FindProperty ("category");
		difficulty = serializedObject.FindProperty ("difficulty");
		courseViews = serializedObject.FindProperty ("courseViews");
	}


	public override void OnInspectorGUI () {
		Course course = (Course)target;

		GUI.color = Color.red;

		if (GUILayout.Button ("Reset to Default")) {
			course.Reset();
		}

		GUI.color = Color.white;
		if (GUILayout.Button("Generate ID"))
		{
			course.ResetID();
		}
		course.ID = EditorGUILayout.TextField("ID", course.ID);

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		course.title = EditorGUILayout.TextField("Title:  " + course.title.Length + "/25 chars", course.title);
		course.author = EditorGUILayout.TextField("Author:  " + course.author.Length + "/25 chars", course.author);
		EditorGUILayout.LabelField ("Description:", course.description.Length + "/130 chars");
		course.description = EditorGUILayout.TextArea(course.description, GUILayout.Height(50));
		course.category = (Course.Category)	EditorGUILayout.EnumPopup("Category:", course.category);
		course.difficulty = (Course.Difficulty)	EditorGUILayout.EnumPopup("Difficulty:", course.difficulty);
		//categoryIndex = EditorGUILayout.Popup(categoryIndex, );

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		if (course.courseViews == null) {
			EditorGUILayout.LabelField ("Refreshing...");
			return;
		}

		if (GUILayout.Button ("Add View Below")) {

			if (course.courseViews.Count < 20) {
					course.courseViews.Add (new CourseView ());
			}
			else {
					EditorUtility.DisplayDialog("Too many views in Course. " + course.courseViews.Count + "/20",
				"Please keep the information concise or split it up into multiple courses.", "Ok");
			}
		}

		EditorGUILayout.Space();

		GUILayout.Label ("A view represents a single 'slide' in the course.");
		GUILayout.Label ("In each view the user should learn something new & be able to practice it using code.");

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		if (GUI.changed)
			EditorUtility.SetDirty (target);

		// COURSE VIEWS
		for (int i = 0; i < course.courseViews.Count; i++) {
			CourseView cw = course.courseViews[i];
			if (cw == null) {
				EditorGUILayout.LabelField ("Refreshing...");
				return;
			}

			if (cw.subject == null) {
				EditorGUILayout.LabelField ("Refreshing...");
				return;
			}

			EditorGUILayout.LabelField ("View " + (i+1) + ": " + cw.subject, EditorStyles.whiteLargeLabel);
			cw.subject = EditorGUILayout.TextField("Subject:  " + cw.subject.Length + "/21 chars", cw.subject);
			EditorGUILayout.LabelField ("Explaination:", cw.explaination.Length + "/150 chars");
			cw.explaination = EditorGUILayout.TextArea(cw.explaination, GUILayout.Height(50));

			EditorGUILayout.Space();

			EditorGUILayout.LabelField ("Code Breakdown:", "(Leave empty to exclude)");
			for (int j = 0; j < cw.codeBulletPoints.Length; j++) {
				cw.codeBulletPoints[j] = EditorGUILayout.TextField((j+1) + ".     " + cw.codeBulletPoints[j].Length + "/100 chars", cw.codeBulletPoints[j]);
			}

			EditorGUILayout.Space();

			EditorGUILayout.LabelField ("Examples:", "(Leave empty to exclude)");
			for (int j = 0; j < cw.exampleBulletPoints.Length; j++) {
				cw.exampleBulletPoints[j] = EditorGUILayout.TextField((j+1) + ".     " + cw.exampleBulletPoints[j].Length + "/100 chars", cw.exampleBulletPoints[j]);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Goal:", cw.goal.Length + "/100 chars");
			cw.goal= EditorGUILayout.TextArea(cw.goal, GUILayout.Height(50));

			EditorGUILayout.Space();

			EditorGUILayout.LabelField ("Instructions:", "(Leave empty to exclude)");
			for (int j = 0; j < cw.instructionBulletPoints.Length; j++) {
				cw.instructionBulletPoints[j] = EditorGUILayout.TextField((j+1) + ".     " + cw.instructionBulletPoints[j].Length + "/100 chars", cw.instructionBulletPoints[j]);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField ("Starting Code:", cw.defaultCode.Length + "/500 chars");
			cw.defaultCode = EditorGUILayout.TextArea(cw.defaultCode, GUILayout.Height(100));

			EditorGUILayout.Space();

			EditorGUILayout.LabelField ("Solution Code:", cw.solutionCode.Length + "/800 chars");
			cw.solutionCode = EditorGUILayout.TextArea(cw.solutionCode, GUILayout.Height(100));

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Code Environment Settings:");
			CodeEnvironment.CESettings _ceSettings = cw.ceSettings;

			_ceSettings.expressionMode = EditorGUILayout.Toggle("Expression mode (simple evaluation):", _ceSettings.expressionMode);

			cw.ceSettings = _ceSettings;

			EditorGUILayout.Space();

			cw.validator = EditorGUILayout.ObjectField("Validator Script:", cw.validator, typeof(MonoScript), false) as MonoScript;
			if (cw.validator == null)
			{
				GUI.color = Color.green;
				if (GUILayout.Button("Generate Validator Script Template"))
				{
					cw.validator = CourseUtil.GenValidatorTemplate(AssetDatabase.GetAssetPath(course), i);
				}
				GUI.color = Color.white;
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			cw.gameScene = EditorGUILayout.ObjectField("Game Scene:", cw.gameScene, typeof(UnityEngine.Object), false);
			if (cw.gameScene != null)
			{
				if (!CourseUtil.IsInBuildSettings(cw.gameScene))
				{
					GUI.color = Color.green;
					if (GUILayout.Button("Add to Build Settings"))
					{
						CourseUtil.AddToBuildSettings(cw.gameScene);
					}
					GUI.color = Color.white;
				}
				else
				{
					GUI.color = Color.red;
					if (GUILayout.Button("Remove from Build Settings"))
					{
						CourseUtil.RemoveFromBuildSettings(cw.gameScene);
					}
					GUI.color = Color.white;
				}
			}

			EditorGUILayout.Space();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button ("Remove View")) {
				course.courseViews.RemoveAt (i);
			}

			if (GUILayout.Button ("Add View Below")) {
				if (course.courseViews.Count < 20) {
					course.courseViews.Insert (i+1, new CourseView ());
				}
				else {
						EditorUtility.DisplayDialog("Too many views in Course. " + course.courseViews.Count + "/20",
					"Please keep the information concise or split it up into multiple courses.", "Ok");
				}
			}

			GUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			if (GUI.changed)
				EditorUtility.SetDirty (target);
		}
	}
}
