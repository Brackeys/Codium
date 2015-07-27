//-----------------------------------------------------------------
// Sits on every ResolutionListItem in the settings menu.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class ResolutionListItem : MonoBehaviour {

	[HideInInspector]
	public Resolution res;
	[HideInInspector]
	public int index;

	[SerializeField]
	private Text resText;

	private SettingsMenuManager settingsMenuManager;

	void Start()
	{
		if (resText == null)
		{
			Debug.LogError("No resText referenced.");
		}

		settingsMenuManager = SettingsMenuManager.ins;
		if (settingsMenuManager == null)
		{
			Debug.LogError("No SettingsMenuManager in the scene?");
		}
	}

	public void SetResolution(Resolution _res)
	{
		res = _res;
		resText.text = string.Format("{0} X {1}", res.width, res.height);
	}

	public void SendResolutionToSettingsManager()
	{
		settingsMenuManager.SetResolution(index);
	}

	public void SetInteractable(bool _state)
	{
		Button _button = GetComponent<Button>();
		if (_button == null)
		{
			Debug.LogError("No button on the ResolutionListItem?!");
			return;
		}
		_button.interactable = _state;
	}

}
