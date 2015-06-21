using UnityEngine;
using UnityEngine.UI;

public class CourseDisplay : MonoBehaviour {

	[HideInInspector]
	public Course course;

	[SerializeField]
	private Text title;
	[SerializeField]
	private Text desc;
	[SerializeField]
	private Text category;
	[SerializeField]
	private Text difficulty;

	private CourseManager courseManager;
	private ApplicationManager applicationManager;

	void Start()
	{
		if (title == null)
		{
			Debug.LogError("No title object referenced");
		}
		if (desc == null)
		{
			Debug.LogError("No desc object referenced");
		}
		if (category == null)
		{
			Debug.LogError("No category object referenced");
		}
		if (difficulty == null)
		{
			Debug.LogError("No difficulty object referenced");
		}

		courseManager = CourseManager.ins;
		if (courseManager == null)
		{
			Debug.LogError("No courseManager?");
		}
		applicationManager = ApplicationManager.ins;
		if (applicationManager == null)
		{
			Debug.LogError("Panik... No applicaitonManager found.");
		}
	}

	public void Load(Course _course)
	{
		title.text = _course.title;
		desc.text = _course.description;
		category.text = _course.category.ToString();
		difficulty.text = _course.difficulty.ToString();

		course = _course;
	}

	public void OpenInCourseView()
	{
		courseManager.SetCurCourse(course);
		courseManager.SaveCurCourseData();
		courseManager.SaveCourseCompletionDataIfEmpty();
		courseManager.LoadCourseCompletionData();
		applicationManager.TransitionToCourseViewScene();
	}
}
