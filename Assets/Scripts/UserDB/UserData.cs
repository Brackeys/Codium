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

	private DateTime signupDate;

	public UserData()
	{
		ID = GenerateID();

		name = "Dwayne Johnson";
		learnPoints = 1400;

		signupDate = DateTime.Now;

		Debug.Log("User " + name + " signed up " + signupDate.Date.ToString("d"));
	}

	public string GenerateID()
	{
		return Guid.NewGuid().ToString("N");
	}
}
