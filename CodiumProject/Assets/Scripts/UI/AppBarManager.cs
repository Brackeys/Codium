//-----------------------------------------------------------------
// Manages general AppBar functionality
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using AudioSystem;
using System.Collections.Generic;

public class AppBarManager : MonoBehaviour {

	private ApplicationManager applicationManager;

	void Start()
	{
		applicationManager = ApplicationManager.ins;
		if (applicationManager == null)
		{
			Debug.LogError("No ApplicationManager found!");
		}
	}

	public void Quit()
	{
		applicationManager.QuitAndSave();
	}

	public void Menu()
	{
		applicationManager.TransitionToMainMenuScene();
	}

	public void Settings() {
		applicationManager.TransitionToSettingsScene();
	}

}
