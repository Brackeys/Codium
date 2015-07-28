//-----------------------------------------------------------------
// This script loads relevant data into the CourseView scene.
// It uses a singleton pattern but will not persist between scenes.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;
using System;
using GameView;
using CodeEnvironment;

public class CourseViewLoader : MonoBehaviour {

	#region Singleton pattern (Awake)

	private static CourseViewLoader _ins;
	public static CourseViewLoader ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<CourseViewLoader>();
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
		}
		else
		{
			// Another instance exists, destroy
			if (this != _ins)
				Destroy(this.gameObject);
		}
	}

	#endregion

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

	public UIDropdown courseViewDropdown;

	public GVControls gvControls;

	// Modal panel
	private ModalPanel modalPanel;
	private UnityAction yesResetCode;
	private UnityAction noResetCode;

	// Course manager
	private CourseManager courseManager;

	//GameView manager
	private GVManager gvManager;

	//Code Environment Manager
	private CEManager ceManager;

	void Start () {
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
		if (courseViewDropdown == null)
		{
			Debug.LogError("No courseViewDropdown referenced");
		}
		if (gvControls == null)
		{
			Debug.LogError("No gvControls referenced");
		}

		// Modal panel
		modalPanel = ModalPanel.ins;
		if (modalPanel == null)
		{
			Debug.LogError("No modal panel?");
		}
		yesResetCode = new UnityAction(_ResetCode);
		noResetCode = new UnityAction(_DoNothing);

		// Course manager
		courseManager = CourseManager.ins;
		if (courseManager == null)
		{
			Debug.LogError("No CourseManager?");
		}

		//GameView manager
		gvManager = GVManager.ins;
		if (gvManager == null)
		{
			Debug.LogError("No GVManager?");
		}

		//CodeEnvironment manager
		ceManager = CEManager.ins;
		if (ceManager == null)
		{
			Debug.LogError("No CEManager?");
		}
	}

	#region Methods for course load on start
	public void LoadCurrentCourseView()
	{
		if (courseManager.curCourseView == null)
		{
			Debug.LogError("No current course view.");
			return;
		}

		int _cvIndex = courseManager.GetCourseViewIndex(courseManager.curCourseView);
		if (_cvIndex == -1) {
			Debug.LogError ("CourseView couldn't be found in courseList.");
			return;
		}
		LoadCourseView(courseManager.curCourse, _cvIndex);
	}

	public void LoadCourseView(Course course, int index)
	{
		// Load title
		title.text = "<i><b>" + (index + 1).ToString("D2") + "</b></i>  " + course.title;

		// LOAD COURSE VIEW:

		if (course.courseViews.Count <= index)
		{
			Debug.LogWarning("CourseView couldn't be loaded because the index isn't within range.");
			return;
		}

		CourseView cv = course.courseViews[index];

		subject.text = cv.subject;	// Load subject
		explaination.text = cv.explaination;	// Load explaination

		bool _hasCodeBulletPoint = false;
		// Load codeBulletPoints
		for (int i = 0; i < cv.codeBulletPoints.Length; i++)
		{
			if (cv.codeBulletPoints[i].Length == 0)
			{
				continue;
			}

			_hasCodeBulletPoint = true;

			RectTransform bp = Instantiate(descBulletPoint) as RectTransform;
			bp.name = descBulletPoint.name;
			Text bpText = bp.GetComponent<Text>();
			if (bpText == null)
			{
				Debug.LogError("No Text component on the descBulletPoint prefab.");
				break;
			}
			bpText.text = "    " + cv.codeBulletPoints[i];
			bp.transform.SetParent(codeDesc);
			bp.localScale = Vector3.one;
		}
		if (!_hasCodeBulletPoint)
		{
			codeDesc.gameObject.SetActive(false);
		}

		bool _hasExampleBulletPoint = false;
		// Load example bullet points
		for (int i = 0; i < cv.exampleBulletPoints.Length; i++)
		{
			if (cv.exampleBulletPoints[i].Length == 0)
			{
				continue;
			}

			_hasExampleBulletPoint = true;

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
			bp.localScale = Vector3.one;
		}
		if (!_hasExampleBulletPoint)
		{
			examples.gameObject.SetActive(false);
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
			bp.localScale = Vector3.one;
		}

		// Load Code Environment settings
		ceManager.SetCESettings(cv.ceSettings);

		// Load course views into dropdown list
		for (int i = 0; i < course.courseViews.Count; i++)
		{
			courseViewDropdown.AddElement(i, course.courseViews[i].subject);
			courseViewDropdown.ConfigureCourseViewElements();
		}

		// Load the game scene && game view
		if (cv.gameScene != null)
		{
			gvManager.SetupGameScene(cv.gameScene.name);
		}
		else
		{
			gvControls.ConfigureConsoleOnly();
		}

		// Load the validator into empty gameobject
		if (cv.validator == null)
		{
			Debug.LogError("No validator referenced in course view?!");
			return;
		}
		GameObject _validatorGM = new GameObject("_Validator");
		CEValidator _validator = _validatorGM.AddComponent(cv.validator.GetClass()) as CEValidator;
		if (_validator == null)
		{
			Debug.LogError("Referenced validator is not valid.");
		}
		ceManager.SetCEValidator(_validator);

	}
	#endregion

	#region Methods for interfacing with course data

	public void LoadCurrentCourseViewDefaultCode()
	{
		if (courseManager.curCourseView == null)
		{
			Debug.LogError("No current course view.");
			return;
		}

		LoadCourseViewDefaultCode(courseManager.curCourse, courseManager.GetCourseViewIndex(courseManager.curCourseView));
	}

	private void LoadCourseViewDefaultCode(Course _course, int _index)
	{
		CourseView _cv = _course.courseViews[_index];
		codeField.text = _cv.defaultCode;
	}

	public void LoadCurrentCourseViewSolutionCode()
	{
		if (courseManager.curCourseView == null)
		{
			Debug.LogError("No current course view.");
			return;
		}

		LoadCourseViewSolutionCode(courseManager.curCourse, courseManager.GetCourseViewIndex(courseManager.curCourseView));
	}

	private void LoadCourseViewSolutionCode(Course _course, int _index)
	{
		CourseView _cv = _course.courseViews[_index];
		codeField.text = _cv.solutionCode;
	}

	#endregion

	#region Modal Panel methods

	public void ResetCode()
	{
		modalPanel.Choice("Are you sure you want to reset your code?", yesResetCode, noResetCode);
	}

	private void _ResetCode()
	{
		LoadCurrentCourseViewDefaultCode();
	}

	private void _DoNothing()
	{
		// Watch som tv?
	}

	#endregion
}
