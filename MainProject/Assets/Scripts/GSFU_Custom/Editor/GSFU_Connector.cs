// *************************************
// Handles setting up a connection to Google Spreadsheet.
// Allows for retrieving data from at spreadsheet.
// *************************************

using UnityEngine;
using LitJson;

namespace Codium.GSFU
{
	public class GSFU_Connector {
		
		//Constructor
		public GSFU_Connector (string spreadsheetID, string worksheetName) {
			m_spreadsheetID = spreadsheetID;
			m_worksheetName = worksheetName;
		}
		
		private string m_spreadsheetID;
		private string m_worksheetName;
		
		public delegate void SetDataDelegate(JsonData[] ssObjects);

		//Update data from Google Spreadsheet
		//Takes in a setDataMethod which will be called when the data is retrieved
		public void UpdateFromSpreadsheet(SetDataDelegate setDataMethod)
		{
			string connectionString = GSFU_Utility.WebServiceURL + "?ssid=" + m_spreadsheetID + "&sheet=" + m_worksheetName + "&pass=" + GSFU_Utility.Password + "&action=GetData";
			Debug.Log("Connecting to webservice on " + connectionString);
			
			WWW www = new WWW(connectionString);

			ContinuationManager.Add( () => www.isDone, () =>
			{
				if (!string.IsNullOrEmpty(www.error))
					Debug.Log("WWW failed: " + www.error);
				
				JsonData[] ssObjects = new JsonData[0];
				
				try 
				{
					ssObjects = JsonMapper.ToObject<JsonData[]>(www.text);
					Debug.Log("Data Successfully Retrieved!");
					
				}
				catch
				{
					Debug.LogError("Data error: could not parse retrieved data as json.");
				}
				
				if (ssObjects.Length != 0) {
					setDataMethod(ssObjects);
				}
			}
			);
			
		}
	
	}
}
