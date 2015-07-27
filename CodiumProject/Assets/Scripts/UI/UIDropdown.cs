//-----------------------------------------------------------------
// Fairly generic UIDropdown component
// Also hosts a method for setting it up to navigate CourseViews: ConfigureCourseViewElements()
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDropdown : MonoBehaviour {

	[SerializeField]
	private RectTransform elementPrefab;

	[SerializeField]
	private List<DropdownElement> elements;

	[SerializeField]
	private GameObject gfx;

	public bool open = false;


	public Image backgroundImage;
	private CanvasGroup backgroundCanvasGroup;

	private byte state;
	private float animStartTime;
	private float animDeltaTime;
	public float animationDuration = 0.5f;

	private float currentBackgroundAlpha;

	void Awake()
	{
		backgroundCanvasGroup = backgroundImage.GetComponent<CanvasGroup>();
	}

	public void Toggle()
	{
		if (open)
			Close();
		else
			Open();
	}

	public void Open()
	{
		open = true;
		gfx.SetActive (true);

		currentBackgroundAlpha = backgroundCanvasGroup.alpha;
		backgroundCanvasGroup.blocksRaycasts = true;
		animStartTime = Time.realtimeSinceStartup;
		state = 1;
	}

	public void Close()
	{
		open = false;
		gfx.SetActive(false);

		currentBackgroundAlpha = backgroundCanvasGroup.alpha;
		backgroundCanvasGroup.blocksRaycasts = false;
		animStartTime = Time.realtimeSinceStartup;
		state = 2;
	}

	//Animations
	void Update()
	{
		if (state == 1)
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (animDeltaTime <= animationDuration)
			{
				backgroundCanvasGroup.alpha = MaterialUI.Anim.Quint.Out(currentBackgroundAlpha, 1f, animDeltaTime, animationDuration);
			}
			else
			{
				backgroundCanvasGroup.alpha = 1f;
				state = 0;
			}
		}
		else if (state == 2)
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (animDeltaTime <= animationDuration)
			{
				backgroundCanvasGroup.alpha = MaterialUI.Anim.Quint.Out(currentBackgroundAlpha, 0f, animDeltaTime, animationDuration);
			}
			else
			{
				backgroundCanvasGroup.alpha = 0f;
				state = 0;
			}
		}
	}

	public void RegisterCloseEvents()
	{
		for (int i = 0; i < elements.Count; i++)
		{
			Button _button = elements[i].elementRef.GetComponent<Button>();
			if (_button == null)
			{
				Debug.LogError("No button component on the element object");
			}
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(() => { Close(); });
		}

		Close();
	}

	// This method gets called by the CourseViewLoader
	// It configures the UIDropdown to handle the Course Views
	public void ConfigureCourseViewElements()
	{
		CourseManager _courseManager;
		_courseManager = CourseManager.ins;
		if (_courseManager == null)
		{
			Debug.LogError("No CourseManager found in the HandleCourseViewElements() method");
		}

		for (int i = 0; i < elements.Count; i++)
		{
			RectTransform _ref = elements[i].elementRef;
			Button _button = _ref.GetComponent<Button>();
			if (_button == null)
			{
				Debug.LogError("No button component on the element object");
			}

			int _index = i;
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(() => { _courseManager.LoadCourseViewByIndex(_index); });

			if (!_courseManager.CourseViewIsCompleted(i))
			{
				_button.interactable = false;
				CanvasGroup _cg = _ref.GetComponent<CanvasGroup>();
				_cg.alpha = 0.5f;
				_cg.interactable = false;
				_cg.blocksRaycasts = false;
			}
		}

		Close();
	}

	#region Adding/removing elements

	public void AddElement(int _index, string _title)
	{
		RectTransform _ref = Instantiate(elementPrefab) as RectTransform;
		_ref.SetParent (gfx.transform);
		_ref.localScale = Vector3.one;
		Transform _titleObj = _ref.FindChild("Title");
		if (_titleObj == null)
		{
			Debug.LogError("No object called 'Title' found under the element prefab");
			return;
		}
		Text _titleText = _titleObj.GetComponent<Text>();
		if (_titleText == null)
		{
			Debug.LogError("No Text component on the title object");
			return;
		}
		_titleText.text = _title;
		Transform _indexObj = _ref.FindChild("Index");
		if (_indexObj == null)
		{
			Debug.LogError("No object called 'Index' found under the element prefab");
			return;
		}
		Text _indexText = _indexObj.GetComponent<Text>();
		if (_indexText == null)
		{
			Debug.LogError("No Text component on the index object");
			return;
		}
		_indexText.text = (_index + 1).ToString("D2");
		DropdownElement _element = new DropdownElement(_index, _title, _ref);
		elements.Add(_element);
	}

	public void RemoveElementAt(int _index)
	{
		Destroy(elements[_index].elementRef);
		elements.RemoveAt(_index);
	}

	public List<DropdownElement> GetList()
	{
		return elements;
	}

	#endregion

}
