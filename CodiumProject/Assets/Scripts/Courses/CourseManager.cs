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

				if (_ins != null)
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

	// References
	private AchievementManager achievementManager;
	private ApplicationManager applicationManager;
	private CourseViewLoader courseViewLoader;

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

		// Make sure all courses have CourseProgressData associated with them
		for (int i = 0; i < courseList.Count; i++)
		{
			if (!CourseProgressDataExists(courseList[i].ID))
			{
				SetDefaultCourseProgressData (courseList[i]);
			}
		}
	}

	void Start()
	{
		achievementManager = AchievementManager.ins;
		if (achievementManager == null)
		{
			Debug.LogError("No AchievementManager found!");
		}
		applicationManager = ApplicationManager.ins;
		if (applicationManager == null)
		{
			Debug.LogError("No ApplicationManager found!");
		}

		if (Application.loadedLevelName == "CourseView")
		{
			SetupCourseView();
		}
		
	}

	private void SetupCourseView()
	{
		courseViewLoader = CourseViewLoader.ins;
		if (courseViewLoader == null)
		{
			Debug.LogError("No CourseViewLoader found!");
		}

		SaveCurCourseDataIfEmpty();
		LoadCurCourseData();

		//If no current course, go to main menu
		if (curCourse == null)
		{
			applicationManager.TransitionToMainMenuScene();
			return;
		}

		LoadCourseProgressData(curCourse);

		//When all course data is set up, load the current one
		courseViewLoader.LoadCurrentCourseView();
	}

	// Populate the courseList variable with Course assets in the /Courses folder
	public void UpdateCourseList () {
		courseList = CourseUtil.LoadAllCourses ().ToList<Course>();
	}

	#region NAVIGATING COURSES

	public void LoadCourseView(string _cvID)
	{
		SetCourseProgressData(curCourse.ID, _cvID);
		applicationManager.TransitionToCourseViewScene();
	}

	public void LoadCourseViewByIndex(int _index)
	{
		LoadCourseView(curCourse.GetCourseViewIDByIndex (_index));
	}

	public void LoadNextCourseView()
	{
		LoadCourseViewByIndex(GetCourseViewIndex(curCourseView) + 1);
	}

	public void CompleteCurCourseView()
	{
		int _completeIndex = CompleteCourseView(curCourse, curCourseView.ID);

		// The course view is already completed
		if (_completeIndex == -2)
		{
			CodiumAPI.Console.PrintSystem("CORRECT: Continue to the next challenge at any time.");
		}
		else
		{
			if (_completeIndex >= curCourse.courseViews.Count - 1)
			{
				//Entire course completed
				achievementManager.CourseCompleted(curCourse);
			}
			else
			{
				//Course view completed
				achievementManager.CourseViewCompleted();
			}

			courseViewLoader.UpdateCourseView();
		}
	}

	//Completes a course view, returns its index
	//Returns -2 if already completed
	public int CompleteCourseView(Course _course, string _cvID)
	{
		CourseProgressData _pData = GetCourseProgressData(_course.ID);
		List<CourseViewStateData> _stateDataList = _pData.GetStateDataList();
		for (int i = 0; i < _stateDataList.Count; i++)
		{
			if (_stateDataList[i].ID == _cvID)
			{
				//If already completed return -2
				if (_stateDataList[i].isCompleted)
					return -2;

				// Match found: Complete it
				_stateDataList[i].isCompleted = true;
				
				int _completeIndex = _course.GetCourseViewIndexByID(_cvID);

				_pData.SetStateDataList(_stateDataList);
				SetCourseProgressData(_pData);
				return _completeIndex;
			}
		}

		Debug.LogError("CompleteCourseView: Course View not found! Returning -1");
		return -1;
	}

	public bool CourseViewIsUnlocked(int _index)
	{
		// The first CV is always unlocked
		if (_index == 0)
			return true;

		// Check the predecessor's complete status
		string _cvID = curCourse.GetCourseViewIDByIndex (_index - 1);

		CourseProgressData _pData = GetCourseProgressData(curCourse.ID);
		CourseViewStateData _sData = _pData.GetStateDataByID(_cvID);

		return _sData.isCompleted;
	}

	#endregion

	#region COURSE PROGRESS DATA
	public CourseProgressData GetCourseProgressData(string _ID)
	{
		string _fileName = CourseProgressHelper.GetFileName (_ID);
		CourseProgressData _pData = Serializer.Load<CourseProgressData>(_fileName);
		return _pData;
	}

	// Sets entire progress data
	public void SetCourseProgressData(CourseProgressData _pData)
	{
		string _fileName = CourseProgressHelper.GetFileName(_pData.courseID);
		Serializer.Save<CourseProgressData>(_pData, _fileName);
	}
	// OVERLOAD: Sets the current course view
	public void SetCourseProgressData(string _courseID, string _curCVID)
	{
		CourseProgressData _pData = GetCourseProgressData(_courseID);
		_pData.SetCurrentCourseViewID(_curCVID);

		string _fileName = CourseProgressHelper.GetFileName(_pData.courseID);
		Serializer.Save<CourseProgressData>(_pData, _fileName);
	}

	//Optional return of default data
	public CourseProgressData SetDefaultCourseProgressData(Course _course)
	{
		CourseProgressData _pData = new CourseProgressData(_course.ID, _course.GetCourseViewIDByIndex(0));
		for (int i = 0; i < _course.courseViews.Count; i++)
		{
			_pData.AddStateData(new CourseViewStateData(_course.courseViews[i].ID));
		}
		SetCourseProgressData(_pData);
		return _pData;
	}

	public void LoadCourseProgressData(Course _course)
	{
		CourseProgressData _pData;
		if (CourseProgressDataExists(_course.ID))
		{
			_pData = GetCourseProgressData(_course.ID);
			//Debug.Log("Course Progress Data exists, let's check that it's valid and updated.");

			if (_pData.GetStateDataCount() > _course.courseViews.Count)
			{
				Debug.LogError("Progress Data has more CV states than there are CV's. TODO: Delete them.");
			}
			else if (_pData.GetStateDataCount() < _course.courseViews.Count)
			{
				Debug.LogError("Progress Data has less CV states than there are CV's. TODO: Add them.");
			}

			if (!_course.CourseViewExists(_pData.GetCurrentCourseViewID()))
			{
				Debug.Log("Current course view doesn't exist. Setting to first one.");
				_pData.SetCurrentCourseViewID( _course.GetCourseViewIDByIndex(0) );
			}
		}
		else
		{
			Debug.Log("NO Course Progress Data exists: Let's set the default");
			_pData = SetDefaultCourseProgressData(_course);
		}

		//Debug.Log("Now let's load it in!");
		SetCurCourseView(_pData.GetCurrentCourseViewID());
	}

	public bool CourseProgressDataExists(string _ID)
	{
		string _fileName = CourseProgressHelper.GetFileName(_ID);
		return Serializer.PathExists(_fileName);
	}

	public float GetCourseProgressPercent(Course _course)
	{
		CourseProgressData _pData = GetCourseProgressData(_course.ID);
		return _pData.GetCourseProgressPercent();
	}

	#endregion

	#region Save and load current course data

	public void SaveCurCourseDataIfEmpty()
	{
		CurCourseData _cData = new CurCourseData ();
		if (!Serializer.PathExists(_cData.fileName))
		{
			Debug.Log ("CourseData: No Course Data found. Saving first Course and CourseView found. Returning to Menu.");
			Application.LoadLevel("MainMenu");
			SetCurCourseByIndex(0);
			SetCurCourseView(0);
			_cData.ID = curCourse.ID;
			Serializer.Save<CurCourseData>(_cData, _cData.fileName);
		}
	}

	public void SaveCurCourseData ()
	{
		CurCourseData _cData = new CurCourseData(curCourse.ID);
		Serializer.Save<CurCourseData>(_cData, _cData.fileName);

		// Debug.Log("CurCourseData saved.");
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

		// Debug.Log("CurCourseData loaded.");
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
	public void SetCurCourseView(string _cvID)
	{
		int _cvIndex = curCourse.GetCourseViewIndexByID(_cvID);

		if (_cvIndex == -1)
		{
			Debug.LogError("Course View not found.");
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
		int _cvIndex = curCourse.courseViews.IndexOf(view);
		if (_cvIndex == -1)
		{
			Debug.LogError("CourseView couldn't be found in courseViews list.");
			return -1;
		} else {
			return _cvIndex;
		}
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
