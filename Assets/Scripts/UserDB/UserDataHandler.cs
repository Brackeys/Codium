//-----------------------------------------------------------------
// This component implements UserData to interact with and retrieve its data
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class UserDataHandler : MonoBehaviour
{
	[SerializeField]
	private bool showGUI = false;

	public UserData user = new UserData();

	[SerializeField]
	private Text usernameText;
	[SerializeField]
	private Text learnPointsText;

	void Start()
	{
		if (usernameText == null)
		{
			Debug.LogError("No usernameText object referenced.");
			return;
		}
		if (learnPointsText == null)
		{
			Debug.LogError("No learnPointsText object referenced");
			return;
		}

		usernameText.text = user.name;
		learnPointsText.text = user.learnPoints.ToString() + "   Learn Points";

		InvokeRepeating("UpdateData", 3, 3);
	}

	void OnGUI()
	{
		if (!showGUI)
			return;

		GUILayout.BeginArea(new Rect(300, 200, 150, 300));
		if (GUILayout.Button("Save Data"))
		{
			Serializer.Save<UserData>(user, "userData");
		}
		if (GUILayout.Button("Load Data"))
		{
			user = Serializer.Load<UserData>("userData");
		}
		GUILayout.EndArea();
	}

	// Update user data that might change
	void UpdateData()
	{
		learnPointsText.text = user.learnPoints.ToString() + "   Learn Points";
	}
}
