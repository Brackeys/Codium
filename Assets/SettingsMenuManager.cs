//-----------------------------------------------------------------
// Manages the settings menu. Sets up the UI correctly. Hosts methods
// for responding to setting changes. Stores changes in private variables
// and then applies them with the ApplySettings() method.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenuManager : MonoBehaviour {

	#region Singleton pattern (Awake)

	private static SettingsMenuManager _ins;
	public static SettingsMenuManager ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<SettingsMenuManager>();
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

	private bool fullScreen;
	[SerializeField]
	private Toggle fullScreenToggle;

	private Resolution resolution;
	[SerializeField]
	private RectTransform resolutionItemPrefab;
	[SerializeField]
	private RectTransform resolutionParent;
	private List<ResolutionListItem> resolutionList;

	void Start()
	{
		if (fullScreenToggle == null)
		{
			Debug.LogError("No fullscreentoggle referenced.");
			return;
		}
		if (resolutionItemPrefab == null)
		{
			Debug.LogError("No resolutionItemPrefab referenced");
			return;
		}
		if (resolutionParent == null)
		{
			Debug.LogError("No resolutionParent referenced");
			return;
		}

		// Sync the private settings with the current settings
		DiscardSettings();

		//Setup the resolution list GUI
		SetupResolutionList();
	}

	//Setup the resolution list GUI
	void SetupResolutionList()
	{
		resolutionList = new List<ResolutionListItem>();
		for (int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution _res = Screen.resolutions[i];

			RectTransform _resIns = Instantiate(resolutionItemPrefab);
			_resIns.name = resolutionItemPrefab.name;
			_resIns.SetParent(resolutionParent);

			ResolutionListItem _resItem = _resIns.GetComponent<ResolutionListItem>();
			if (_resItem == null)
			{
				Debug.LogError("No ResolutionListItem found on resolutionItemPrefab instance.");
				continue;
			}
			_resItem.SetResolution (_res);
			_resItem.index = i;

			// If the _resItem is equal to our current resolution, mark
			// it as selected (interactable = false). I check using
			// width and height because the object wont compare.
			if (_resItem.res.width == resolution.width && _resItem.res.height == resolution.height)
			{
				_resItem.SetInteractable(false);
			}
			else
			{
				_resItem.SetInteractable(true);
			}
			resolutionList.Add(_resItem);

			//Disable non-supported resolutions
			if (_res.width < 800 || _res.height < 600)
			{
				_resIns.gameObject.SetActive(false);
			}
		}
	}

	//Set the private resolution by indexing into the resolutionList
	public void SetResolution(int _index)
	{
		for (int i = 0; i < resolutionList.Count; i++)
		{
			if (i == _index)
			{
				resolutionList[i].SetInteractable(false);
				resolution = resolutionList[i].res;
			}
			else
			{
				resolutionList[i].SetInteractable(true);
			}
		}
	}

	//Set the private fullscreen state
	public void SetFullscreen(bool _state)
	{
		fullScreen = _state;
	}

	// Set the current settings to the private settings
	public void ApplySettings()
	{
		Screen.SetResolution(resolution.width, resolution.height, fullScreen);
		PrintSettings();
	}

	// Set the private settings to the
	// current settings
	public void DiscardSettings()
	{
		fullScreen = Screen.fullScreen;
		fullScreenToggle.isOn = fullScreen;

		resolution = Screen.currentResolution;
	}

	//Print current settings to the console
	void PrintSettings()
	{
		Debug.Log(string.Format("Fullscreen: {0} - Resolution: {1}", fullScreen, resolution));
	}

}
