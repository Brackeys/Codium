//-----------------------------------------------------------------
// This serializable class hosts all information about the user
//-----------------------------------------------------------------

using UnityEngine;
using System;

[System.Serializable]
public class UserData {

	public string ID;

	public string name;
	public int learnPoints;

	public DateTime signupDate;

	public UserData()
	{
		// Do nothing
	}

	public void Init()
	{
		ID = GenerateID();

		name = "Dwayne Johnson";
		learnPoints = NumberMaster.startingLP;

		signupDate = DateTime.Now;

		Debug.Log("User " + name + " signed up " + signupDate.Date.ToString("d"));
	}

	public string GenerateID()
	{
		return Guid.NewGuid().ToString("N");
	}
}
