using UnityEngine;

public class AchievementManager : MonoBehaviour {

	#region Singleton pattern (Awake)

	private static AchievementManager _ins;
	public static AchievementManager ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<AchievementManager>();

				DontDestroyOnLoad(_ins.gameObject);
			}

			return _ins;
		}
		set
		{
			_ins = value;
		}
	}

	void Awake()
	{
		if (_ins == null)
		{
			// Populate with first instance
			_ins = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			// Another instance exists, destroy
			if (this != _ins)
				Destroy(this.gameObject);
		}
	}

	#endregion

	private UserDataManager userDatamanager;
	private ApplicationManager applicationManager;
	private CourseManager courseManager;

	void Start()
	{
		// Caching
		userDatamanager = UserDataManager.ins;
		if (userDatamanager == null)
		{
			Debug.LogError("No UserDataManager?!");
		}
		applicationManager = ApplicationManager.ins;
		if (applicationManager == null)
		{
			Debug.LogError("No ApplicationManager?!");
		}
		courseManager = CourseManager.ins;
		if (courseManager == null)
		{
			Debug.LogError("No CourseManager?!");
		}
	}

	public void CourseViewCompleted()
	{
		Print("Course View Completed!");
		userDatamanager.GiveLearnPoints(NumberMaster.courseViewLPValue);	// Give learn points for view
		applicationManager.TransitionToCourseViewScene();
	}

	public void CourseCompleted(Course _course)
	{
		Print("Course Complete!");
		userDatamanager.GiveLearnPoints(_course.LPValue + NumberMaster.courseViewLPValue);	// Give learn points for course + for view
		applicationManager.TransitionToMainMenuScene();
	}

	private void Print(string msg)
	{
		Debug.Log("ACHIEVEMENT: " + msg);
	}
}
