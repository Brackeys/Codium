//-----------------------------------------------------------------
// This script loads relevant data into the MainMenu scene.
// It uses a singleton pattern but will not persist between scenes.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuLoader : MonoBehaviour
{

	#region Singleton pattern (Awake)

	private static MainMenuLoader _ins;
	public static MainMenuLoader ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<MainMenuLoader>();
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
	public Transform courseListObject;
	public RectTransform courseDisplayPrefab;
	public Text welcome;

	// Course manager
	private CourseManager courseManager;
	private UserDataManager userDataManager;

	void Start()
	{
		if (courseListObject == null)
		{
			Debug.LogError("No courseListObject referenced");
		}
		if (courseDisplayPrefab == null)
		{
			Debug.LogError("No courseDisplayPrefab referenced");
		}
		if (welcome == null)
		{
			Debug.LogError("No welcome object referenced");
		}

		// Course manager
		courseManager = CourseManager.ins;
		if (courseManager == null)
		{
			Debug.LogError("No CourseManager?");
		}

		// User data manager
		userDataManager = UserDataManager.ins;
		if (userDataManager == null)
		{
			Debug.LogError("No UserDataManager?");
		}

		LoadCoursesIntoList();
		LoadUserInfo();
	}

	private void LoadUserInfo()
	{
		welcome.text = "Welcome <color=\"#fb8717\">" + userDataManager.GetName() + "</color>,";
	}

	private void LoadCoursesIntoList () {
		Course[] _courses = courseManager.courseList.ToArray();

		//Loop through all courses in the list
		for (int i = 0; i < _courses.Length; i++)
		{
			// Instantiate a courseDisplayPrefab and parent it to the list object
			RectTransform _cd = Instantiate(courseDisplayPrefab) as RectTransform;
			_cd.name = courseDisplayPrefab.name;
			// The CourseDisplay makes it easy to setup the course becuase it takes care of
			// local references to UI components
			CourseDisplay _cdComponent = _cd.GetComponent<CourseDisplay>();
			if (_cdComponent == null)
			{
				Debug.LogError("No CourseDisplay on the courseDisplayPrefab");
				break;
			}
			_cdComponent.Load(_courses[i]);
			_cd.transform.SetParent(courseListObject);
		}
	}
}
