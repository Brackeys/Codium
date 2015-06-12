//-----------------------------------------------------------------
// UITabs is a fairly generic UI component that handles the toggling
// between x-amount of tabs. It hosts the UITab class.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITabs : MonoBehaviour
{

	[System.Serializable]
	public class UITab
	{
		public Button button;
		public GameObject content;
	}

	public UITab[] tabs;
	[HideInInspector]
	public int currentTab = 0;

	[SerializeField]
	private Color textOnColor;
	[SerializeField]
	private Color textOffColor;

	void Start()
	{
		SwitchTo(currentTab);
	}

	// Switch tab using index
	public void SwitchTo(int tab)
	{
		currentTab = tab;

		for (int i = 0; i < tabs.Length; i++)
		{
			if (tabs[i].content == null || tabs[i].button == null)
			{
				Debug.LogError("Tab " + i + " incorrectly setup. Missing reference.");
				return;
			}

			if (i == tab)
			{
				tabs[i].button.interactable = false;
				Text t = tabs[i].button.GetComponentInChildren<Text>();
				if (t != null)
					t.color = textOnColor;
				tabs[i].content.SetActive(true);
			}
			else
			{
				tabs[i].button.interactable = true;
				Text t = tabs[i].button.GetComponentInChildren<Text>();
				if (t != null)
					t.color = textOffColor;
				tabs[i].content.SetActive(false);
			}
		}
	}

}
