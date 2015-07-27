//-----------------------------------------------------------------
// Hosts utility methods for handling / loading course assets at runtime.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class CourseUtil
{


	#region COURSE LOADING

	public static Course LoadCourse (string name) {
		Course course = Resources.Load<Course>("Courses" + name);

		if (course == null) {
			Debug.LogError ("Course with name of "+ name +
			                " doesn't exist. Returning null.");
		}

		return course;
	}

	public static Course[] LoadAllCourses () {
		Course[] courses = Resources.LoadAll<Course>("Courses");
		if (courses.Length == 0) {
			Debug.LogWarning ("No courses found. Returning empty Array.");
		}

		return courses;
	}
	#endregion

	#region COURSE SORTING

	// Sort course list by course title
	public static List<Course> SortCoursesByTitle (List<Course> courses) {
		courses.Sort ( (x, y) => string.Compare (x.title, y.title) );
		return courses;
	}
	
	// Sort course list by course category
	public static List<Course> SortCoursesByCategory (List<Course> courses) {
		courses.Sort ( (x, y) => string.Compare (x.category.ToString(), y.category.ToString()) );
		return courses;
	}
	
	// Sort course list by category first, then title
	public static List<Course> SortCourses (List<Course> courses) {

		courses = SortCoursesByCategory (courses);	// Sort by category

		// Final list to return
		List <Course> sortedList = new List<Course>();

		// Temp list for storing courses in same category
		List <Course> coursesInCategory = new List<Course>();
		Course.Category lastCourse = new Course.Category ();

		for (int i = 0; i < courses.Count; i++) {

			if (i == 0) {	// On first iteration
				coursesInCategory.Add ( courses[i]);	// Add course to temp list
				lastCourse = courses[i].category;		// Store this iteration's category
				continue;
			}

			if (lastCourse != courses[i].category) {	// The course category hasn't changed
				// Sort category and add to final list, then clear the temp list
				sortedList.AddRange ( SortCoursesByTitle (coursesInCategory) );
				coursesInCategory.Clear();
			}

			coursesInCategory.Add ( courses[i]);	// Add course to temp list
			lastCourse = courses[i].category;		// Store this iteration's category
		}

		// Add the remainder of our list
		sortedList.AddRange ( SortCoursesByTitle (coursesInCategory) );
		
		return sortedList;
	}
	#endregion

	#region GAME SCENE HANDLING

	//Check if scene is added to Build Settings
	public static bool IsInBuildSettings(Object _sceneObj)
	{

		string _pathToScene = AssetDatabase.GetAssetPath(_sceneObj);
		EditorBuildSettingsScene _scene = new EditorBuildSettingsScene(_pathToScene, true);

		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			if (EditorBuildSettings.scenes[i].path == _scene.path)
			{
				return true;
			}

			//Debug.Log("Scene: " + i + " - " + EditorBuildSettings.scenes[i].path);
		}

		return false;

	}

	//Ads the scene to Build Settings
	public static void AddToBuildSettings(Object _sceneObj)
	{
		string _pathToScene = AssetDatabase.GetAssetPath(_sceneObj);
		EditorBuildSettingsScene _scene = new EditorBuildSettingsScene(_pathToScene, true);

		EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
		EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);

		newSettings[original.Length] = _scene;

		EditorBuildSettings.scenes = newSettings;

		Debug.Log("Scene with path: " + _scene.path + " has been added to Build Settings");
	}

	//Removes the scene from Build Settings
	public static void RemoveFromBuildSettings(Object _sceneObj)
	{

		string _pathToScene = AssetDatabase.GetAssetPath(_sceneObj);
		EditorBuildSettingsScene _scene = new EditorBuildSettingsScene(_pathToScene, true);

		List<EditorBuildSettingsScene> _scenes = new List<EditorBuildSettingsScene>();

		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			if (EditorBuildSettings.scenes[i].path != _scene.path)
			{
				_scenes.Add(EditorBuildSettings.scenes[i]);
			}
		}

		EditorBuildSettings.scenes = _scenes.ToArray();

		Debug.Log("Scene with path: " + _scene.path + " has been removed from Build Settings");
	}

	#endregion

	#region VALIDATOR SCRIPT HANDLING

	// Generates a C# script with the template defined by ValidatorTemplate.txt
	public static MonoScript GenValidatorTemplate(string _assetPath, int _viewIndex)
	{
		string name = Path.GetFileNameWithoutExtension(_assetPath) + "_View" + (_viewIndex + 1).ToString("D2"); ;
		string fullPath = Path.GetDirectoryName(_assetPath) + "/" + name + ".cs";
		string resourcesPath = "Assets/Resources/";

		string validatorTemplateName = "ValidatorTemplate.txt";
		string validatorTemplateTxt = File.ReadAllText(Application.dataPath + "/Resources/Courses/" + validatorTemplateName);
		validatorTemplateTxt = validatorTemplateTxt.Replace("**CLASSNAME**", name);
		//Debug.Log(validatorTemplateTxt);

		if (File.Exists(fullPath) == false)
		{
			Debug.Log("Creating Validator Template: " + fullPath);

			using (StreamWriter outfile =
				new StreamWriter(fullPath))
			{
				outfile.Write(validatorTemplateTxt);
			}//File written
		}
		else
		{
			Debug.Log("File already exists.");
		}
		AssetDatabase.Refresh();
		string relativePath = fullPath.Remove(0, resourcesPath.Length);
		relativePath = relativePath.Remove(relativePath.Length - 3, 3);
		//Debug.Log(relativePath);
		MonoScript _validatorTemp = Resources.Load(relativePath) as MonoScript;
		if (_validatorTemp == null)
		{
			Debug.LogError("Something went wrong. Validator template is null");
			return null;
		}
		else
		{
			return _validatorTemp;
		}
	}

	#endregion

}
