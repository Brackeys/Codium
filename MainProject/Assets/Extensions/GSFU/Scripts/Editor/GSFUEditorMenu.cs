using UnityEditor;
using UnityEngine;
using LitJson;

public class GSFUEditorMenu : MonoBehaviour
{
	
	[MenuItem ("GSFU/Update from Google Spreadsheet %g")]
	static void UpdateFromSpreadsheet()
	{
		UnityDataConnector conn = GameObject.Find("ConnectionExample").GetComponent<UnityDataConnector>();

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
	}

	[MenuItem("GSFU/Reset")]
	static void Reset()
	{
		GameObject dataUseExample = Selection.activeObject as GameObject;
		dataUseExample.GetComponent<AdjustBalls>().ResetBalls();
	}


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

} 