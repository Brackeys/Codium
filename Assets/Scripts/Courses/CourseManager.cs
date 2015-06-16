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

	public bool loadCourseOnStart = true;

	// GUI REFERENCES

	public Text title;
	public Text subject;
	public Text explaination;
	public Transform codeDesc;
	public RectTransform descBulletPoint;
	public Transform examples;
	public InputField codeField;
	public RectTransform stepBulletPoint;
	public Transform instructions;
	public Text goal;

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

		if (loadCourseOnStart)
		{
			if (title == null)
			{
				Debug.LogError("No title text object referenced");
			}
			if (subject == null)
			{
				Debug.LogError("No subject text object referenced");
			}
			if (explaination == null)
			{
				Debug.LogError("No explaination text object referenced");
			}
			if (codeDesc == null)
			{
				Debug.LogError("No codeDesc object referenced");
			}
			if (descBulletPoint == null)
			{
				Debug.LogError("No descBulletPoint prefab referenced");
			}
			if (examples == null)
			{
				Debug.LogError("No examples object referenced");
			}
			if (codeField == null)
			{
				Debug.LogError("No codeField object referenced");
			}
			if (stepBulletPoint == null)
			{
				Debug.LogError("No stepBulletPoint prefab referenced");
			}
			if (instructions == null)
			{
				Debug.LogError("No instructions object referenced");
			}
			if (goal == null)
			{
				Debug.LogError("No goal object referenced");
			}
		}

		UpdateCourseList ();
		courseList = CourseUtil.SortCourses (courseList);
	}

	void Start()
	{
		if (loadCourseOnStart)
		{
			curCourse = courseList[0];
			curCourseView = GetViewByIndex(0);

			LoadCurrentCourseView();
		}
	}

	// Populate the courseList variable with Course assets in the /Courses folder
	public void UpdateCourseList () {
		courseList = CourseUtil.LoadAllCourses ().ToList<Course>();
	}

	#region Methods for interfacing with course data

	public void LoadCurrentCourseViewDefaultCode()
	{
		if (curCourseView == null)
		{
			Debug.LogError("No current course view.");
			return;
		}

		LoadCourseViewDefaultCode(curCourse, GetCourseViewIndex(curCourseView));
	}

	private void LoadCourseViewDefaultCode(Course _course, int _index)
	{
		CourseView _cv = _course.courseViews[_index];
		codeField.text = _cv.defaultCode;
	}

	public void LoadCurrentCourseViewSolutionCode()
	{
		if (curCourseView == null)
		{
			Debug.LogError("No current course view.");
			return;
		}

		LoadCourseViewSolutionCode(curCourse, GetCourseViewIndex(curCourseView));
	}

	private void LoadCourseViewSolutionCode(Course _course, int _index)
	{
		CourseView _cv = _course.courseViews[_index];
		codeField.text = _cv.solutionCode;
	}

	#endregion

	#region Course Loading
	public void LoadCurrentCourseView()
	{
		if (curCourseView == null)
		{
			Debug.LogError("No current course view.");
			return;
		}

		LoadCourseView (curCourse, GetCourseViewIndex(curCourseView));
	}

	public void LoadCourseView(Course course, int index)
	{
		// Load title
		title.text = course.title;

		// LOAD COURSE VIEW:

		if (course.courseViews.Count <= index) {
			Debug.LogWarning ("CourseView couldn't be loaded because the index isn't within range.");
			return;
		}

		CourseView cv = course.courseViews[index];

		subject.text = cv.subject;	// Load subject
		explaination.text = cv.explaination;	// Load explaination

		// Load codeBulletPoints
		for (int i = 0; i < cv.codeBulletPoints.Length; i++)
		{
			if (cv.codeBulletPoints[i].Length == 0)
			{
				continue;
			}

			RectTransform bp = Instantiate(descBulletPoint) as RectTransform;
			bp.name = descBulletPoint.name;
			Text bpText = bp.GetComponent<Text>();
			if (bpText == null)
			{
				Debug.LogError("No Text component on the descBulletPoint prefab.");
				break;
			}
			bpText.text = "    " + cv.codeBulletPoints[i];
			bp.transform.SetParent (codeDesc);
		}

		// Load example bullet points
		for (int i = 0; i < cv.exampleBulletPoints.Length; i++)
		{
			if (cv.exampleBulletPoints[i].Length == 0)
			{
				continue;
			}

			RectTransform bp = Instantiate(descBulletPoint) as RectTransform;
			bp.name = descBulletPoint.name;
			Text bpText = bp.GetComponent<Text>();
			if (bpText == null)
			{
				Debug.LogError("No Text component on the descBulletPoint prefab.");
				break;
			}
			bpText.text = "    " + cv.exampleBulletPoints[i];
			bp.transform.SetParent(examples);
		}

		// load default code
		codeField.text = cv.defaultCode;

		// load goal
		goal.text = cv.goal;

		// load instructions, step by step
		for (int i = 0; i < cv.instructionBulletPoints.Length; i++)
		{
			if (cv.instructionBulletPoints[i].Length == 0)
			{
				continue;
			}

			RectTransform bp = Instantiate(stepBulletPoint) as RectTransform;
			bp.name = stepBulletPoint.name;
			Text bpText = bp.GetComponent<Text>();
			if (bpText == null)
			{
				Debug.LogError("No Text component on the descBulletPoint prefab.");
				break;
			}
			bpText.text = cv.instructionBulletPoints[i];
			Transform bpCount = bp.FindChild("StepCount");
			if (bpCount == null)
			{
				Debug.LogError("No child called StepCount found under " + bp.name);
				break;
			}
			Text bpCountText = bpCount.GetComponent<Text>();
			if (bpCountText == null)
			{
				Debug.LogError("No Text component on the StepCount object: " + bpCount.name);
				break;
			}
			bpCountText.text = (i + 1).ToString();

			bp.transform.SetParent(instructions);
		}
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
