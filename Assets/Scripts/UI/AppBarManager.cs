using UnityEngine;

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

}
