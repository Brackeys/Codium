// *************************************
// Custom inspector for LessonData objects.
// This adds functionality for loading data from Google Spreadsheets.
// *************************************

using UnityEngine;
using UnityEditor;
using Codium.Challenges;
using LitJson;
using Codium.GSFU;

namespace Codium
{
	[CustomEditor(typeof(LessonData))]
	public class LessonDataEditor : Editor
	{
		//Currently selected lesson
		private LessonData m_selectedLesson;

		//Draw the inspector
		public override void OnInspectorGUI()
		{
			m_selectedLesson = (LessonData)target;

			//If we want to update the data
			if (GUILayout.Button("Update from Google Spreadsheet"))
			{
				//Set up a connection to Google Spreadsheet
				GSFU_Connector connector = new GSFU_Connector(m_selectedLesson.spreadsheetID, GSFU_Utility.LessonWorksheetName);

				//Call the update method and tell it to call SetLessonData with all the information.
				connector.UpdateFromSpreadsheet(SetLessonData);
			}

			base.OnInspectorGUI();
		}

		//This method updates the information on the LessonData object.
		//It is invoked by the GSFU_Connector class when it is done fetching the data.
		public void SetLessonData(JsonData[] ssObjects)
		{

			if (ssObjects == null)
			{
				Debug.LogError("Data is null.");
				return;
			}

			ChallengeData[] challenges = new ChallengeData[ssObjects.Length];

			for (int i = 0; i < ssObjects.Length; i++)
			{
				challenges[i] = new ChallengeData();

				if (ssObjects[i].Keys.Contains("mission"))
				{
					challenges[i].mission = ssObjects[i]["mission"].ToString();
				}

				if (ssObjects[i].Keys.Contains("challengeType"))
				{
					challenges[i].type = GetChallengeType(ssObjects[i]["challengeType"].ToString());
					switch (challenges[i].type)
					{
						case ChallengeType.QUIZ:
							challenges[i].quizChallengeData = GetQuizChallengeData(ssObjects[i]);
							break;
						case ChallengeType.FITB:
							challenges[i].fitbChallengeData = GetFITBChallengeData(ssObjects[i]);
							break;
						default:
							Debug.LogError("Challenge type " + challenges[i].type + " not found.");
							continue;
					}
				}

			}

			m_selectedLesson.challenges = challenges;
		}

		//This method extracts QuizChallengeData from the JsonData
		private QuizChallengeData GetQuizChallengeData(JsonData ssObject)
		{
			QuizChallengeData quizData = new QuizChallengeData();

			if (ssObject.Keys.Contains("optionOne"))
			{
				quizData.optionOne = ssObject["optionOne"].ToString();
			}

			if (ssObject.Keys.Contains("optionTwo"))
			{
				quizData.optionTwo = ssObject["optionTwo"].ToString();
			}

			if (ssObject.Keys.Contains("optionThree"))
			{
				quizData.optionThree = ssObject["optionThree"].ToString();
			}

			if (ssObject.Keys.Contains("quizAnswer"))
			{
				quizData.answer = (QuizAnswer)int.Parse(ssObject["quizAnswer"].ToString());
			}

			return quizData;
		}

		//This method extract FITBChallengeData from JsonData object
		private FITBChallengeData GetFITBChallengeData(JsonData ssObject)
		{
			FITBChallengeData fitbData = new FITBChallengeData();

			if (ssObject.Keys.Contains("fillCode"))
			{
				fitbData.fillCode = ssObject["fillCode"].ToString();
			}

			if (ssObject.Keys.Contains("fillOne"))
			{
				fitbData.fillOne = ssObject["fillOne"].ToString();
			}

			if (ssObject.Keys.Contains("fillTwo"))
			{
				fitbData.fillTwo = ssObject["fillTwo"].ToString();
			}

			if (ssObject.Keys.Contains("fillThree"))
			{
				fitbData.fillThree = ssObject["fillThree"].ToString();
			}

			if (ssObject.Keys.Contains("fillAnswer"))
			{
				fitbData.answer = (FITBAnswer)int.Parse(ssObject["fillAnswer"].ToString());
			}

			return fitbData;
		}

		//This method converts from a string to a ChallengeType (enum)
		private ChallengeType GetChallengeType(string type)
		{
			switch (type)
			{
				case "QUIZ":
					return ChallengeType.QUIZ;
				case "FITB":
					return ChallengeType.FITB;
				default:
					Debug.LogError("Challenge type: " + type + " not found.");
					return ChallengeType.QUIZ;
			}
		}

	}
}
