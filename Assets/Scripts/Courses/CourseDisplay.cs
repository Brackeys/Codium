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
	[SerializeField]
	private Image completionBar;
	[SerializeField]
	private Text completionText;

	private CourseManager courseManager;
	private ApplicationManager applicationManager;

	void Init()
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
		if (completionBar == null)
		{
			Debug.LogError("No completionBar object referenced");
		}
		if (completionText == null)
		{
			Debug.LogError("No completionText object referenced");
		}

		courseManager = CourseManager.ins;
		if (courseManager == null)
		{
			Debug.LogError("No courseManager?");
		}
		applicationManager = ApplicationManager.ins;
		if (applicationManager == null)
		{
			Debug.LogError("Panic... No applicaitonManager found.");
		}
	}

	public void Load(Course _course)
	{
		Init();

		title.text = _course.title;
		desc.text = _course.description;
		category.text = _course.category.ToString();
		difficulty.text = _course.difficulty.ToString();

		float _pct = courseManager.GetCompletionPercent(_course);
		completionBar.fillAmount = _pct/100f;
		int _pctInt = Mathf.RoundToInt(_pct);
		completionText.text = string.Format("{0}% mastered", _pctInt);

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
