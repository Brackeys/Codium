using UnityEditor;
using UnityEngine;
using LitJson;

namespace Codium.GSFU
{
	public class GSFU_DataEditor : EditorWindow
	{
		// Add menu named "My Window" to the Window menu
		[MenuItem ("GSFU/Data Editor")]
		static void Init () {
			// Get existing open window or if none, make a new one:
			GSFU_DataEditor window = (GSFU_DataEditor)EditorWindow.GetWindow (typeof (GSFU_DataEditor));
			window.Show();
			
			window.InitConnector();
		}
		
		void OnGUI () {
			
			if (Selection.objects.Length == 0) {
				GUILayout.Label("Please select a LessonData object.");
				return;
			}
			
			if (Selection.objects[0].GetType() != typeof(LessonData)) {
				GUILayout.Label ("Please select a LessonData object.");	
						return;
			}
			
			LessonData lesson = (LessonData)Selection.objects[0];
			GUILayout.Label ("Selection: " + Selection.objects[0].name);
			
			if (GUILayout.Button ("Update from Google Spreadsheet")) {
				UpdateFromSpreadsheet(lesson);
			}
			
			GUILayout.Box ("STATUS: " + currentStatus);

		}

		void UpdateFromSpreadsheet(LessonData lesson)
		{
			Connect(lesson.spreadsheetID, "Lesson", "Y5nw5cuv4hPq");
			
			//UnityDataConnector conn = GameObject.Find("ConnectionExample").GetComponent<UnityDataConnector>();

			/*
			string connectionString = conn.webServiceUrl + "?ssid=" + conn.spreadsheetId + "&sheet=" + conn.worksheetName + "&pass=" + conn.password + "&action=GetData";
			Debug.Log("Connecting to webservice on " + connectionString);
			
			WWW www = new WWW(connectionString);

			ContinuationManager.Add( () => www.isDone, () =>
			{
				if (!string.IsNullOrEmpty(www.error))
					Debug.Log("WWW failed: " + www.error);
				
				try 
				{
					JsonData[] ssObjects = JsonMapper.ToObject<JsonData[]>(www.text);
					Debug.Log("Data Successfully Retrieved!");
					GameObject dataUseExample = Selection.activeObject as GameObject;
					dataUseExample.GetComponent<AdjustBalls>().DoSomethingWithTheData(ssObjects);
				}
				catch
				{
					Debug.LogError("Data error: could not parse retrieved data as json.");
				}
			}
			);
			*/
		}

		/*

		// Validation of menu items.
		[MenuItem ("GSFU/Reset", true)]
		[MenuItem ("GSFU/Update from Google Spreadsheet %g", true)]
		static bool ValidateUpdate() 
		{ 
			// Return false if no transform is selected.
			if (Selection.activeObject == null)
				return false;

			if (Selection.activeObject.name != "DataUseExample")
				return false;

			return true;
		}
		
		*/
		
		private string m_spreadsheetId = "";
		private string m_worksheetName = "";
		private string m_password = "";
		
		private const float MAX_WAIT_TIME = 5f;
		private const bool DEBUG_MODE = true;

		bool updating;
		string currentStatus;
		JsonData[] ssObjects;
		
		void InitConnector ()
		{
			updating = false;
			currentStatus = "Offline";
		}
		
		void Connect(string spreadsheetId, string worksheetName, string password)
		{
			if (updating)
				return;
			
			m_spreadsheetId = spreadsheetId;
			m_worksheetName = worksheetName;
			m_password = password;
			
			updating = true; 
		}
		
		void Update () {
			if (updating)
				GetData();
		}
		
		void GetData()
		{
			string connectionString = GSFU_Utility.WebServiceURL + "?ssid=" + m_spreadsheetId + "&sheet=" + m_worksheetName + "&pass=" + m_password + "&action=GetData";
			if (DEBUG_MODE) {
				//Debug.Log("Connecting to webservice on " + connectionString);
			}

			WWW www = new WWW(connectionString);
			
			double startTime = EditorApplication.timeSinceStartup;
			currentStatus = "Stablishing Connection... ";
			
			while (!www.isDone)
			{		
				if (EditorApplication.timeSinceStartup >= startTime + MAX_WAIT_TIME)
				{
					currentStatus = "Max wait time reached, connection aborted.";
					Debug.Log(currentStatus);
					updating = false;
					break;
				}
				
				//Debug.Log (EditorApplication.timeSinceStartup);
			}
		
			if (!www.isDone || !string.IsNullOrEmpty(www.error))
			{
				currentStatus = "Connection error after" + (EditorApplication.timeSinceStartup - startTime) + "seconds: " + www.error;
				Debug.LogError(currentStatus);
				updating = false;
				return;
			}
		
			string response = www.text;
			Debug.Log((EditorApplication.timeSinceStartup - startTime) + " : " + response);
			currentStatus = "Connection stablished, parsing data...";

			if (response == "\"Incorrect Password.\"")
			{
				currentStatus = "Connection error: Incorrect Password.";
				Debug.LogError(currentStatus);
				updating = false;
				return;
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
				return;
			}

			currentStatus = "Data Successfully Retrieved!";
			updating = false;
			
			// Finally use the retrieved data as you wish.
			//dataDestinationObject.SendMessage("DoSomethingWithTheData", ssObjects);
			
			Debug.Log ("YAY!");
		}
	
	}
}