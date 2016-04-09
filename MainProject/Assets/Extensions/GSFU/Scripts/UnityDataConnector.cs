using System.Collections;
using UnityEngine;
using LitJson;
 

public class UnityDataConnector : MonoBehaviour
{
	public string webServiceUrl = "";
	public string spreadsheetId = "";
	public string worksheetName = "";
	public string password = "";
	public float maxWaitTime = 10f;
	public GameObject dataDestinationObject;
	public string statisticsWorksheetName = "Statistics";
	public bool debugMode;

	bool updating;
	string currentStatus;
	JsonData[] ssObjects;
	bool saveToGS; 

	Rect guiBoxRect;
	Rect guiButtonRect;
	Rect guiButtonRect2;
	Rect guiButtonRect3;
	
	void Start ()
	{
		updating = false;
		currentStatus = "Offline";
		saveToGS = false;

		guiBoxRect = new Rect(10, 10, 310, 140);
		guiButtonRect = new Rect(30, 40, 270, 30);
		guiButtonRect2 = new Rect(30, 75, 270, 30);
		guiButtonRect3 = new Rect(30, 110, 270, 30);
	}
	
	void OnGUI()
	{
		GUI.Box(guiBoxRect, currentStatus);
		if (GUI.Button(guiButtonRect, "Update From Google Spreadsheet"))
		{
			Connect();
		}

		saveToGS = GUI.Toggle(guiButtonRect2, saveToGS, "Save Stats To Google Spreadsheet");

		if (GUI.Button(guiButtonRect3, "Reset Balls values"))
		{
			dataDestinationObject.SendMessage("ResetBalls");
		}
	}
	
	void Connect()
	{
		if (updating)
			return;
		
		updating = true;
		StartCoroutine(GetData());   
	}
	
	IEnumerator GetData()
	{
		string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + worksheetName + "&pass=" + password + "&action=GetData";
		if (debugMode)
			Debug.Log("Connecting to webservice on " + connectionString);

		WWW www = new WWW(connectionString);
		
		float elapsedTime = 0.0f;
		currentStatus = "Stablishing Connection... ";
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				currentStatus = "Max wait time reached, connection aborted.";
				Debug.Log(currentStatus);
				updating = false;
				break;
			}
			
			yield return null;  
		}
	
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}
	
		string response = www.text;
		Debug.Log(elapsedTime + " : " + response);
		currentStatus = "Connection stablished, parsing data...";

		if (response == "\"Incorrect Password.\"")
		{
			currentStatus = "Connection error: Incorrect Password.";
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}

		try 
		{
			ssObjects = JsonMapper.ToObject<JsonData[]>(response);
		}
		catch
		{
			currentStatus = "Data error: could not parse retrieved data as json.";
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}

		currentStatus = "Data Successfully Retrieved!";
		updating = false;
		
		// Finally use the retrieved data as you wish.
		dataDestinationObject.SendMessage("DoSomethingWithTheData", ssObjects);
	}




	public void SaveDataOnTheCloud(string ballName, float collisionMagnitude)
	{
		if (saveToGS)
			StartCoroutine(SendData(ballName, collisionMagnitude)); 
	} 

	IEnumerator SendData(string ballName, float collisionMagnitude)
	{
		if (!saveToGS)
			yield break;

		string connectionString = 	webServiceUrl +
									"?ssid=" + spreadsheetId +
									"&sheet=" + statisticsWorksheetName +
									"&pass=" + password +
									"&val1=" + ballName +
									"&val2=" + collisionMagnitude.ToString() +
									"&action=SetData";

		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;

		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				// Error handling here.
				break;
			}

			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			// Error handling here.
			yield break;
		}
		
		string response = www.text;

		if (response.Contains("Incorrect Password"))
		{
			// Error handling here.
			yield break;
		}

		if (response.Contains("RCVD OK"))
		{
			// Data correctly sent!
			yield break;
		}
	}
}
	
	