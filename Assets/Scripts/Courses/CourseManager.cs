//-----------------------------------------------------------------
// CourseManager persists between scenes and keeps track of courses
//-----------------------------------------------------------------

using UnityEngine;
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
		SortCourseListIntelligently ();

		curCourse = courseList [0];
		Debug.Log (curCourse.GetCompletionPercent());
		Debug.Log (curCourse.IsCompleted());
		SelectCourseViewByIndex (0);
	}

	// Populate the courseList variable with Course assets in the /Courses folder
	// and remove completed courses from the list
	public void UpdateCourseList () {
		courseList = CourseUtil.LoadAllCourses ().ToList<Course>();
	}

	// Loops through the courseList and returns the course that matches
	public int FindCourseIndex ( Course course) {
		for (int i = 0; i < courseList.Count; i++) {
			if (courseList[i] == course) {
				return i;
			}
		}

		return -1;	// Return error
	}

	// Select a current course by index
	public void SelectCourseByIndex (int index) {
		if (courseList.Count <= index) {
			Debug.LogWarning ("Course couldn't be selected because the index isn't within range.");
			return;
		}

		curCourse = courseList [index];
	}

	// Select a current course by title
	public void SelectCourseByTitle (string title) {
		for (int i = 0; i < courseList.Count; i++) {
			if (courseList[i].title == title) {
				curCourse = courseList [i];
				return;
			}
		}

		Debug.LogWarning ("Course with title " + title + " couldn't be found.");
		return;
	}

	// Select a current course view by index
	public void SelectCourseViewByIndex (int index) {
		if (curCourse.courseViews.Count <= index) {
			Debug.LogWarning ("CourseView couldn't be selected because the index isn't within range.");
			return;
		}

		curCourseView = curCourse.courseViews [index];
	}

	// Select current course view by view subject
	public void SelectCourseViewBySubject (string subject) {
		for (int i = 0; i < curCourse.courseViews.Count; i++) {
			if (curCourse.courseViews [i].subject == subject) {
				curCourseView = curCourse.courseViews [i];
				return;
			}
		}

		Debug.LogWarning ("No courseview with subject " + subject + " could be found.");
		return;
	}

	// Sort the course list by course title
	public void SortCourseListByTitle () {
		courseList.Sort ( (x, y) => string.Compare (x.title, y.title) );
	}

	// Sort the course list by course category
	public void SortCourseListByCategory () {
		courseList.Sort ( (x, y) => string.Compare (x.category.ToString(), y.category.ToString()) );
	}

	// Sort the course list intelligently
	// This method can be adjusted to suit the user
	public void SortCourseListIntelligently () {
		SortCourseListByCategory ();

		for (int i = 0; i < courseList.Count; i++) {
			Debug.Log (i);
		}

		Debug.Log ("TODO: Intelligent course list sort");
	}	
}