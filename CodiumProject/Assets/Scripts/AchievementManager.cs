using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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

	private const string rewardTextColor = "<color=#FF9D58>";

	private UserDataManager userDatamanager;
	private ApplicationManager applicationManager;
	private CourseManager courseManager;

	private ModalPanel modalPanel;
	private UnityAction courseCompletedOKEvent;
	private UnityAction courseViewCompletedOKEvent;
	private UnityAction courseViewCompletedCancelEvent;

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
		modalPanel = ModalPanel.ins;
		if (modalPanel == null)
		{
			Debug.LogError("No ModalPanel?!");
		}
		courseCompletedOKEvent = new UnityAction(_CourseCompleted);
		courseViewCompletedOKEvent = new UnityAction(_CourseViewContinue);
		courseViewCompletedCancelEvent = new UnityAction(_CourseViewStay);
	}

	public void CourseViewCompleted()
	{
		Print("Course View Completed!");
		Invoke("_CourseViewCompleted", 1f); 
		
	}
	private void _CourseViewCompleted()
	{
		int _lpReward = NumberMaster.courseViewLPValue;
		string _msg = "Step Completed!\nYou've earned  " + rewardTextColor + _lpReward.ToString() + "</color>" + "  Learn Points.\nDo you want to continue immediately?";
		modalPanel.CalmChoice(_msg, courseViewCompletedOKEvent, courseViewCompletedCancelEvent);
		userDatamanager.GiveLearnPoints(_lpReward);	// Give learn points for view
	}
	private void _CourseViewContinue()
	{
		courseManager.SaveCourseCompletionData(1);
		applicationManager.TransitionToCourseViewScene();
	}
	private void _CourseViewStay()
	{
		// Do nothing
	}

	public void CourseCompleted(Course _course)
	{
		Print("Course '" + _course.title + "' Complete!");
		int _lpReward = _course.LPValue + NumberMaster.courseViewLPValue;	// Give learn points for course + for view
		string _msg = "Entire Course Completed!\nYou've earned  " + rewardTextColor + _lpReward.ToString() + "</color>" + "  Learn Points.";
		modalPanel.Notification(_msg, courseCompletedOKEvent);
		userDatamanager.GiveLearnPoints(_lpReward);
	}
	private void _CourseCompleted()
	{
		//Do nothing so far
	}

	private void Print(string msg)
	{
		Debug.Log("ACHIEVEMENT: " + msg);
	}
}
