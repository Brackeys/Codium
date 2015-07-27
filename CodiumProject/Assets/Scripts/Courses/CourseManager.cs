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

		SaveCourseCompletionDataIfEmpty();
		LoadCourseCompletionData();

		//When all course data is set up, load the current one
		courseViewLoader.LoadCurrentCourseView();
	}

	// Populate the courseList variable with Course assets in the /Courses folder
	public void UpdateCourseList () {
		courseList = CourseUtil.LoadAllCourses ().ToList<Course>();
	}


	#region Navigate course views

	// Complete the current course view (or entire course) and transition to the next one
	// This method calls methods on other systems to handle Learn Points, Notification & already
	// completed course views.
	public void CompleteCourseView()
	{
		if (IsLastCourseView())		// If this is the last CV: Mark course as completed
		{
			CourseCompletionData _ccData = new CourseCompletionData(curCourse.ID);
			_ccData = Serializer.Load<CourseCompletionData>(_ccData.fileName);

			if (!_ccData.isCompleted)
			{
				_ccData.Complete();
				Serializer.Save<CourseCompletionData>(_ccData, _ccData.fileName);

				achievementManager.CourseCompleted(curCourse);
			}
			else
			{
				// Do nothing
			}
		}
		else	//else save course and load next view
		{
			if (!CurCourseViewIsCompleted())
			{
				SaveCourseCompletionData(1);
				achievementManager.CourseViewCompleted();
			}
			else
			{
				SaveCourseCompletionData(1);
				applicationManager.TransitionToCourseViewScene();
			}
		}
	}

	public void LoadNextCourseView()
	{
		if (GetCourseCompletionData_Next() <= GetCourseCompletionData_Current()) {
			Debug.LogError("CourseCompletionData: Could not load next course view because it isn't unlocked or course is complete " + (GetCourseCompletionData_Current() + 1));
		}
		else
		{
			SaveCourseCompletionData(1);
			applicationManager.TransitionToCourseViewScene();
		}
	}

	public void LoadPreviousCourseView()
	{
		if (GetCourseCompletionData_Current() == 0)
		{
			Debug.LogError("CourseCompletionData: Could not load previous course view because it doesn't exist (below 0): ");
		}
		else
		{
			SaveCourseCompletionData(-1);
			applicationManager.TransitionToCourseViewScene();
		}
	}

	// Load course view using an index.
	// This might make LoadNextCourseView() and LoadPreviousCourseView() deprecated.
	public void LoadCourseViewByIndex(int _index)
	{
		if (_index > GetCourseCompletionData_Next())
		{
			Debug.LogError("CourseCompletionData: Could not load course view with index '" + _index + "' because it isn't unlocked.");
		}
		else
		{
			int _offset = _index - GetCourseCompletionData_Current();
			SaveCourseCompletionData(_offset);
			applicationManager.TransitionToCourseViewScene();
		}
	}

	private bool IsLastCourseView()
	{
		CourseCompletionData _ccData = new CourseCompletionData(curCourse.ID);
		_ccData = Serializer.Load<CourseCompletionData>(_ccData.fileName);

		if (_ccData.currentCVIndex == curCourse.courseViews.Count - 1)
			return true;
		else
			return false;
	}

	// This method uses the save data so make sure not to override it before checking
	private bool CurCourseViewIsCompleted()
	{
		if (GetCourseCompletionData_Next() <= GetCourseCompletionData_Current())
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public bool CourseViewIsCompleted(int _index)
	{
		if (GetCourseCompletionData_Next() < _index)
			return false;
		else
			return true;
	}

	#endregion

	#region Save and load course completion data

	public void SaveCourseCompletionDataIfEmpty()
	{
		CourseCompletionData _ccData = new CourseCompletionData(curCourse.ID);
		if (!Serializer.PathExists(_ccData.fileName))
		{
			Debug.Log("CourseCompletion: No CourseCompletionData found. Saving first CourseView.");
			SetCurCourseView(0);
			Serializer.Save<CourseCompletionData>(_ccData, _ccData.fileName);
		}
	}

	public void SaveCourseCompletionData(int _offset)
	{
		int _cvIndex = GetCourseViewIndex(curCourseView);
		if (_cvIndex == -1)
		{
			Debug.LogError("CourseView couldn't be found in courseViews list.");
			return;
		}

		CourseCompletionData _ccData = new CourseCompletionData(curCourse.ID);
		_ccData = Serializer.Load<CourseCompletionData>(_ccData.fileName);
		_ccData.Init(_cvIndex + _offset);
		Serializer.Save<CourseCompletionData>(_ccData, _ccData.fileName);
	}

	private CourseCompletionData GetCourseCompletionData()
	{
		CourseCompletionData _ccData = new CourseCompletionData(curCourse.ID);
		_ccData = Serializer.Load<CourseCompletionData>(_ccData.fileName);
		return _ccData;
	}

	private int GetCourseCompletionData_Next()
	{
		CourseCompletionData _ccData = GetCourseCompletionData();
		if (_ccData == null)
		{
			Debug.LogError("CourseCompletionData is null. Returning -1");
			return -1;
		}
		return _ccData.nextCVIndex;
	}

	public void LoadCourseCompletionData()
	{
		SetCurCourseView(GetCourseCompletionData_Current());
	}
	private int GetCourseCompletionData_Current()
	{
		CourseCompletionData _ccData = GetCourseCompletionData();
		if (_ccData == null)
		{
			Debug.LogError("CourseCompletionData is null. Returning -1");
			return -1;
		}
		return _ccData.currentCVIndex;
	}

	public float GetCompletionPercent(Course _course)
	{
		CourseCompletionData _ccData = new CourseCompletionData(_course.ID);
		if (!Serializer.PathExists(_ccData.fileName))
		{
			return 0f;
		}

		_ccData = Serializer.Load<CourseCompletionData>(_ccData.fileName);
		if (_ccData.isCompleted)
		{
			return 100f;
		}
		return _course.GetCompletionPercent(_ccData.nextCVIndex);
	}

	#endregion

	#region Save and load current course data

	public void SaveCurCourseDataIfEmpty()
	{
		CurCourseData _cData = new CurCourseData ();
		if (!Serializer.PathExists(_cData.fileName))
		{
			Debug.Log ("CourseData: No Course Data found. Saving first Course and CourseView found.");
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
