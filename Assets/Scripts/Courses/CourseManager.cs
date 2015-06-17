//-----------------------------------------------------------------
// CourseManager persists between scenes and keeps track of courses & course views.
// It has methods for updating and retrieving courses & course views.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CourseManager : MonoBehaviour {

	private static CourseManager _ins;
	public static CourseManager ins {
		get {
			if (_ins == null) {
				_ins = GameObject.FindObjectOfType <CourseManager>();

				DontDestroyOnLoad (_ins.gameObject);
			}
		
			return _ins;
		}
		set {
			_ins = value;
		}
	}

	public Course curCourse;
	public List<Course> courseList;

	public CourseView curCourseView;

	void Awake () {
		if (_ins == null) {
			// Populate with first instance
			_ins = this;
			DontDestroyOnLoad (this);
		} else {
			// Another instance exists, destroy
			if (this != _ins)
				Destroy (this.gameObject);
		}

		UpdateCourseList ();
		courseList = CourseUtil.SortCourses (courseList);

		SaveCurCourseDataIfEmpty();
		LoadCurCourseData();
	}

	// Populate the courseList variable with Course assets in the /Courses folder
	public void UpdateCourseList () {
		courseList = CourseUtil.LoadAllCourses ().ToList<Course>();
	}

	#region Save and load current course data

	public void SaveCurCourseDataIfEmpty()
	{
		CurCourseData _cData = new CurCourseData ();
		if (!Serializer.PathExists(_cData.fileName))
		{
			Debug.Log ("No Course Data found. Saving first Course and CourseView found.");
			SetCurCourseByIndex(0);
			SetCurCourseView(0);
			_cData.ID = curCourse.ID;
			_cData.curCVIndex = 0;
			Serializer.Save<CurCourseData>(_cData, _cData.fileName);
		}
	}

	public void SaveCurCourseData ()
	{
		int _cvIndex = GetCourseViewIndex(curCourseView);
		if (_cvIndex == -1) {
			Debug.LogError ("CourseView couldn't be found in courseList.");
			return;
		}
		CurCourseData _cData = new CurCourseData(curCourse.ID, _cvIndex);
		Serializer.Save<CurCourseData>(_cData, _cData.fileName);

		Debug.Log("CurCourseData saved.");
	}

	public void LoadCurCourseData()
	{
		CurCourseData _cData = new CurCourseData();
		_cData = Serializer.Load<CurCourseData>(_cData.fileName);

		for (int i = 0; i < courseList.Count; i++)
		{
			if (courseList[i].ID == _cData.ID)
			{
				SetCurCourseByIndex(i);
			}
		}

		SetCurCourseView(_cData.curCVIndex);

		Debug.Log("CurCourseData loaded.");
	}

	#endregion

	#region Helpful setter methods

	public void SetCurCourse(Course _course)
	{
		int _index = GetCourseIndex(_course);
		if (_index == -1) {
			Debug.LogError ("Course: " + _course.title + " couldn't be found in courseList");
			return;
		}
		curCourse = courseList[_index];
	}

	public void SetCurCourseByIndex(int _index)
	{
		if (_index > courseList.Count - 1)
		{
			Debug.LogError("Index out of bounds. Could not set cur course by index.");
			return;
		}
		curCourse = courseList[_index];
	}

	public void SetCurCourseView(int _cvIndex)
	{

		if (_cvIndex > curCourse.courseViews.Count - 1)
		{
			Debug.LogError("Index out of bounds: couldn't set current course view");
			return;
		}

		curCourseView = curCourse.courseViews[_cvIndex];
	}

	#endregion

	#region Helpful getter methods

	// Loops through the courseList and returns the course that matches, otherwise return -1
	public int GetCourseIndex ( Course course) {
		return courseList.IndexOf (course);
	}

	// Loop through the courseView List and return the course that matches, otherwise return -1
	public int GetCourseViewIndex ( CourseView view) {
		return curCourse.courseViews.IndexOf (view);
	}

	// Return course by index
	public Course GetCourseByIndex (int index) {
		if (courseList.Count <= index) {
			Debug.LogWarning ("Course couldn't be selected because the index isn't within range. Returning null");
			return null;
		}

		return courseList [index];
	}

	// Return course by title
	public Course GetCourseByTitle (string title) {
		for (int i = 0; i < courseList.Count; i++) {
			if (courseList[i].title == title) {
				return courseList [i];
			}
		}

		Debug.LogWarning ("Course with title " + title + " couldn't be found. Returning null");
		return null;
	}

	// Return course view by index, else return null
	public CourseView GetViewByIndex (int index) {
		if (curCourse.courseViews.Count <= index) {
			Debug.LogWarning ("CourseView couldn't be selected because the index isn't within range.");
			return null;
		}

		return curCourse.courseViews [index];
	}

	// Return course view by view subject, else return null
	public CourseView GetViewBySubject (string subject) {
		for (int i = 0; i < curCourse.courseViews.Count; i++) {
			if (curCourse.courseViews [i].subject == subject) {
				return curCourse.courseViews [i];
			}
		}

		Debug.LogWarning ("No courseview with subject " + subject + " could be found.");
		return null;
	}
	#endregion

}
