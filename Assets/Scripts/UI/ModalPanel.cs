using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalPanel : MonoBehaviour {

	public Text question;
	public Button yesButton;
	public Button noButton;
	public Button cancelButton;
	public Button okButton;
	public GameObject modalPanelObject;

	// References to anims
	[Header("End animations require a UIAlphaAnim on each object:")]
	[SerializeField]
	private float questionFadeOutDelay = 0.1f;
	UIAlphaAnim questionUIAA;
	[SerializeField]
	private float yesButtonFadeOutDelay = 0.1f;
	UIAlphaAnim yesButtonUIAA;
	[SerializeField]
	private float noButtonFadeOutDelay = 0.1f;
	UIAlphaAnim noButtonUIAA;
	[SerializeField]
	private float cancelButtonFadeOutDelay = 0.1f;
	UIAlphaAnim cancelButtonUIAA;
	[SerializeField]
	private float okButtonFadeOutDelay = 0.1f;
	UIAlphaAnim okButtonUIAA;
	[SerializeField]
	private float bgFadeOutDelay = 0.1f;
	UIAlphaAnim modalPanelObjectUIAA;

	[Header("Panel end animation: (required)")]
	[SerializeField]
	private GameObject panelObject;
	private UIScaleAnim panelObjectUISA;
	private UIAlphaAnim panelObjectUIAA;

	#region Singleton Pattern (Awake)
	private static ModalPanel _ins;
	public static ModalPanel ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<ModalPanel>();

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

	void Start()
	{

		if (question == null)
		{
			Debug.LogError("No question object referenced");
		}
		if (yesButton == null)
		{
			Debug.LogError("No yesButton object referenced");
		}
		if (noButton == null)
		{
			Debug.LogError("No noButton object referenced");
		}
		if (cancelButton == null)
		{
			Debug.LogError("No cancelButton object referenced");
		}
		if (okButton == null)
		{
			Debug.LogError("No okButton object referenced");
		}
		if (modalPanelObject == null)
		{
			Debug.LogError("No modalPanelObject object referenced");
		}
		if (panelObject == null)
		{
			Debug.LogError("No panelObject object referenced");
		}

		// Cache all UIAlphaAnims
		//questionUIAA = question.GetComponent<UIAlphaAnim>();
		//yesButtonUIAA = yesButton.transform.parent.GetComponent<UIAlphaAnim>();
		//noButtonUIAA = noButton.transform.parent.GetComponent<UIAlphaAnim>();
		//cancelButtonUIAA = cancelButton.transform.parent.GetComponent<UIAlphaAnim>();
		//okButtonUIAA = okButton.transform.parent.GetComponent<UIAlphaAnim>();
		modalPanelObjectUIAA = modalPanelObject.GetComponent<UIAlphaAnim>();
		panelObjectUISA = panelObject.GetComponent<UIScaleAnim>();
		panelObjectUIAA = panelObject.GetComponent<UIAlphaAnim>();
	}

	// Yes/No/Cancel: string, yes event, no event, cancel event
	public void Choice(string _question, UnityAction _yesEvent, UnityAction _noEvent, UnityAction _cancelEvent)
	{
		OpenPanel();

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener(_yesEvent);
		yesButton.onClick.AddListener(ClosePanel);

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener(_noEvent);
		noButton.onClick.AddListener(ClosePanel);

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(_cancelEvent);
		cancelButton.onClick.AddListener(ClosePanel);

		question.text = _question;

		yesButton.transform.parent.gameObject.SetActive(true);
		noButton.transform.parent.gameObject.SetActive(true);
		cancelButton.transform.parent.gameObject.SetActive(true);
	}

	// Yes/No: string, yes event, no event
	public void Choice(string _question, UnityAction _yesEvent, UnityAction _noEvent)
	{
		OpenPanel();

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener(_yesEvent);
		yesButton.onClick.AddListener(ClosePanel);

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener(_noEvent);
		noButton.onClick.AddListener(ClosePanel);

		question.text = _question;

		yesButton.transform.parent.gameObject.SetActive(true);
		noButton.transform.parent.gameObject.SetActive(true);
	}

	// OK/Cancel: string, ok event, cancel event
	public void CalmChoice(string _question, UnityAction _okEvent, UnityAction _cancelEvent)
	{
		OpenPanel();

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(_okEvent);
		okButton.onClick.AddListener(ClosePanel);

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(_cancelEvent);
		cancelButton.onClick.AddListener(ClosePanel);

		question.text = _question;

		okButton.transform.parent.gameObject.SetActive(true);
		cancelButton.transform.parent.gameObject.SetActive(true);
	}

	// OK: string, ok event
	public void Notification(string _question, UnityAction _okEvent)
	{
		OpenPanel();

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(_okEvent);
		okButton.onClick.AddListener(ClosePanel);

		question.text = _question;

		okButton.transform.parent.gameObject.SetActive(true);
	}

	private void ClosePanel()
	{
		// Disable objects, some via animation
		if (modalPanelObjectUIAA != null)
		{
			modalPanelObjectUIAA.Animate(bgFadeOutDelay, true, true);
		}
		else
		{
			modalPanelObject.SetActive(false);
		}

		if (yesButtonUIAA != null && yesButtonUIAA.gameObject.activeInHierarchy == true)
		{
			yesButtonUIAA.Animate(yesButtonFadeOutDelay, true, true);
		}
		else
		{
			yesButton.transform.parent.gameObject.SetActive(false);
		}

		if (noButtonUIAA != null && noButtonUIAA.gameObject.activeInHierarchy == true)
		{
			noButtonUIAA.Animate(noButtonFadeOutDelay, true, true);
		}
		else
		{
			noButton.transform.parent.gameObject.SetActive(false);
		}

		if (cancelButtonUIAA != null && cancelButtonUIAA.gameObject.activeInHierarchy == true)
		{
			cancelButtonUIAA.Animate(cancelButtonFadeOutDelay, true, true);
		}
		else
		{
			cancelButton.transform.parent.gameObject.SetActive(false);
		}

		if (okButtonUIAA != null && okButtonUIAA.gameObject.activeInHierarchy == true)
		{
			okButtonUIAA.Animate(okButtonFadeOutDelay, true, true);
		}
		else
		{
			okButton.transform.parent.gameObject.SetActive(false);
		}

		if (panelObjectUIAA != null)
		{
			panelObjectUIAA.Animate(0f, true, false);
		}
		if (panelObjectUISA != null)
		{
			//panelObjectUISA.Animate(0f, true);
			Debug.LogWarning("FIX THIS ANIMATION PLZZZ");
		}
	}

	private void OpenPanel()
	{
		modalPanelObject.SetActive(true);
		panelObject.SetActive(true);
	}
}
