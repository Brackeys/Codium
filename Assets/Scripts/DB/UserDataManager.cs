//-----------------------------------------------------------------
// This component implements UserData to interact with and retrieve its data
// It also uses the serializer to safely store and load this data
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class UserDataManager : MonoBehaviour
{
	#region Singleton pattern (Awake)

	private static UserDataManager _ins;
	public static UserDataManager ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<UserDataManager>();

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

	[SerializeField]
	private bool showGUI = false;

	public UserData user;
	private string dataFileName = "userData";

	[Header("Optional:")]
	[SerializeField]
	private Text usernameText;
	[SerializeField]
	private Text learnPointsText;

	void Start()
	{
		user = new UserData();
		Debug.Log("User " + user.name + " signed up " + user.signupDate.Date.ToString("d"));

		if (usernameText != null)
			usernameText.text = user.name;
		if (learnPointsText != null)
			learnPointsText.text = user.learnPoints.ToString() + "   Learn Points";

		InvokeRepeating("UpdateData", 3, 3);
	}

	public void SaveUserData()
	{
		Serializer.Save<UserData>(user, dataFileName);
	}

	public void LoadUserData()
	{
		user = Serializer.Load<UserData>(dataFileName);
	}

	void OnGUI()
	{
		if (!showGUI)
			return;

		GUILayout.BeginArea(new Rect(300, 200, 150, 300));
		if (GUILayout.Button("Save Data"))
		{
			SaveUserData();
		}
		if (GUILayout.Button("Load Data"))
		{
			LoadUserData();
		}
		GUILayout.EndArea();
	}

	// Update user data that might change
	private void UpdateData()
	{
		if (learnPointsText != null)
			learnPointsText.text = user.learnPoints.ToString() + "   Learn Points";
	}

	#region Helpful getter/setter methods

	public string GetName()
	{
		if (user == null)
		{
			Debug.LogError("User is null. Cannot get name");
			return "Error: user is null";
		}
		return user.name;
	}

	public int GetLearnPoints()
	{
		if (user == null)
		{
			Debug.LogError("User is null. Cannot get learn points");
			return -1;
		}
		return user.learnPoints;
	}


	public void GiveLearnPoints(int amount)
	{
		if (user == null)
		{
			Debug.LogError("User is null. Cannot set learn points");
			return;
		}
		user.learnPoints += amount;
		SaveUserData();
	}
	#endregion
}
