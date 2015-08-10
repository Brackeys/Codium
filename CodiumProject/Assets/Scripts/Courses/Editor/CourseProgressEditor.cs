//-----------------------------------------------------------------
// This editor window makes it easy to see the current progress of
// a course at a glance. It also has buttons for completing and
// uncompleting course views.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEditor;
public class CourseProgressEditor : EditorWindow
{

	CourseManager _courseManager;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Course Progress Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		CourseProgressEditor window = (CourseProgressEditor)EditorWindow.GetWindow(typeof(CourseProgressEditor));
		window.Show();
	}

	void OnGUI()
	{
		_courseManager = CourseManager.ins;

		if (_courseManager == null)
		{
			GUILayout.Label ("MUST BE IN PLAYMODE");
		}
		else
		{
			if (_courseManager.curCourse != null)
			{
				GUILayout.Label("Current Course: " + _courseManager.curCourse.title, EditorStyles.boldLabel);
				GUILayout.Label("Progress: " + _courseManager.GetCourseProgressPercent(_courseManager.curCourse) + "%");

				if (_courseManager.curCourseView != null)
				{
					//GUILayout.Label("Current View: " + _courseManager.curCourseView.subject);

					EditorGUILayout.Space();
					CourseProgressData _pData = _courseManager.GetCourseProgressData(_courseManager.curCourse.ID);

					foreach (CourseView _view in _courseManager.curCourse.courseViews)
					{

						GUILayout.BeginHorizontal();
						CourseViewStateData _sData = _pData.GetStateDataByID(_view.ID);
						if (_sData == null)
							return;

						if (_sData.isCompleted)
							GUI.color = Color.green;
						else if (_sData.ID == _courseManager.curCourseView.ID)
							GUI.color = Color.blue;
						else
							GUI.color = Color.white;
						_sData.isCompleted = GUILayout.Toggle(_sData.isCompleted, _view.subject);

						if (_sData.isCompleted)
						{
							GUI.color = Color.red;
							if (GUILayout.Button("UnComplete", GUILayout.Width(100)))
							{
								_sData.isCompleted = false;
								_courseManager.SetCourseProgressData(_pData);
							}
						}
						else
						{
							GUI.color = Color.green;
							if (GUILayout.Button("Complete", GUILayout.Width(100)))
							{
								_sData.isCompleted = true;
								_courseManager.SetCourseProgressData(_pData);
							}
						}
						GUILayout.EndHorizontal();
					}
				}
			}

			//GUILayout.Label("All Course View States:", EditorStyles.boldLabel);
		}
	}
}
