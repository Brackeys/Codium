using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ApplicationManager : MonoBehaviour {


	#region Singleton pattern (Awake)

	private static ApplicationManager _ins;
	public static ApplicationManager ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<ApplicationManager>();

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

	private UserDataManager userDataManager;
	private CourseManager courseManager;

	private ModalPanel modalPanel;
	private UnityAction yesQuitAction;
	private UnityAction noQuitAction;
	private UnityAction yesReloadCourseViewScene;
	private UnityAction noReloadCourseViewScene;

	private Fade fade;

	void Start()
	{
		userDataManager = UserDataManager.ins;
		if (userDataManager == null)
		{
			Debug.LogError("No UserDataManager found");
		}
		courseManager = CourseManager.ins;
		if (courseManager == null)
		{
			Debug.LogError("No CourseManager found");
		}

		modalPanel = ModalPanel.ins;
		yesQuitAction = new UnityAction(_QuitAndSave);
		noQuitAction = new UnityAction(_DoNothing);
		yesReloadCourseViewScene = new UnityAction(_ReloadCourseViewScene);
		noReloadCourseViewScene = new UnityAction(_DoNothing);

		fade = Fade.ins;
		if (fade == null)
		{
			Debug.LogError("No Fade found");
		}
	}

	public void ReloadCourseViewScene()
	{
		modalPanel.Choice("Are you sure you want to reset all?\nThis Course Step will reload.", yesReloadCourseViewScene, noReloadCourseViewScene);
	}
	private void _ReloadCourseViewScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void TransitionToCourseViewScene()
	{
		StartCoroutine(_FadeOutLoadLevel("CourseView"));
	}
	public void TransitionToMainMenuScene()
	{
		StartCoroutine(_FadeOutLoadLevel("MainMenu"));
	}
	private IEnumerator _FadeOutLoadLevel(string _level)
	{
		yield return new WaitForSeconds(fade.BeginFade(1));
		Application.LoadLevel(_level);
	}

	public void QuitAndSave()
	{
		modalPanel.Choice("Are you sure you want to quit?\nThere is so much to do!", yesQuitAction, noQuitAction);
	}
	private void _QuitAndSave()
	{
		userDataManager.SaveUserData();
		StartCoroutine(_FadeOutQuit());
		Application.Quit();
	}
	private IEnumerator _FadeOutQuit()
	{
		yield return new WaitForSeconds( fade.BeginFade(1) );
		Debug.Log("APPLICATION QUIT");
		Application.Quit();
	}

	private void _DoNothing()
	{
		// Maybe eat some chips?
	}
}
