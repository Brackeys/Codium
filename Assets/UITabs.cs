using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITabs : MonoBehaviour {

	public Button[] tabs;
	[HideInInspector]
	public int currentTab = 0;

	public Color textOnColor;
	public Color textOffColor;

	void Start()
	{
		SwitchTo(currentTab);
	}

	public void SwitchTo(int tab)
	{
		currentTab = tab;

		for (int i = 0; i < tabs.Length; i++ )
		{
			if (i == tab)
			{
				tabs[i].interactable = false;
				Text t = tabs[i].GetComponentInChildren<Text>();
				if (t != null)
					t.color = textOnColor;
			}
			else
			{
				tabs[i].interactable = true;
				Text t = tabs[i].GetComponentInChildren<Text>();
				if (t != null)
					t.color = textOffColor;
			}
		}
	}

}
