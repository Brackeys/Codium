using UnityEditor;
using UnityEngine;
using LitJson;
using Codium.Challenges;

namespace Codium.GSFU
{
	public class GSFU_DataEditor : EditorWindow
	{
		// Add menu named "My Window" to the Window menu
		[MenuItem ("Window/GSFU Data Editor")]
		static void Init () {
			// Get existing open window or if none, make a new one:
			GSFU_DataEditor window = (GSFU_DataEditor)EditorWindow.GetWindow (typeof (GSFU_DataEditor));
			window.titleContent.text = "GSFU Data Editor";
			window.Show();
		}
		
		private LessonData m_selectedLesson;
		
		void OnGUI () {
			
			if (Selection.objects.Length == 0) {
				GUILayout.Label("Please select a LessonData object.");
				return;
			}
			
			if (Selection.objects[0].GetType() != typeof(LessonData)) {
				GUILayout.Label ("Please select a LessonData object.");	
						return;
			}
			
			m_selectedLesson = (LessonData)Selection.objects[0];
			GUILayout.Label ("Selection: " + Selection.objects[0].name);
			
			if (GUILayout.Button ("Update from Google Spreadsheet")) {
				GSFU_Connector connector = new GSFU_Connector(m_selectedLesson.spreadsheetID, "Lesson");
				
				connector.UpdateFromSpreadsheet(SetLessonData);
			}

		}
		
		public void SetLessonData (JsonData[] ssObjects) {
			
			if (ssObjects == null) {
				Debug.LogError ("Data is null.");
				return;
			}
			
			ChallengeData[] challenges = new ChallengeData[ssObjects.Length];
			
			for (int i = 0; i < ssObjects.Length; i++)
			{
				challenges[i] = new ChallengeData();
				
				if (ssObjects[i].Keys.Contains("mission")) {
					challenges[i].mission = ssObjects[i]["mission"].ToString();
				}
				
				if (ssObjects[i].Keys.Contains("challengeType")) {
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
							Debug.LogError ("Challenge type " + challenges[i].type + " not found.");
							continue;
					}
				}
				
			}
			
			m_selectedLesson.challenges = challenges;
		}
		
		private QuizChallengeData GetQuizChallengeData (JsonData ssObject) {
			QuizChallengeData quizData = new QuizChallengeData();
			
			if (ssObject.Keys.Contains("optionOne")) {
					quizData.optionOne = ssObject["optionOne"].ToString();
			}
			
			if (ssObject.Keys.Contains("optionTwo")) {
					quizData.optionTwo = ssObject["optionTwo"].ToString();
			}
			
			if (ssObject.Keys.Contains("optionThree")) {
					quizData.optionThree = ssObject["optionThree"].ToString();
			}
			
			if (ssObject.Keys.Contains("quizAnswer")) {
					quizData.answer = (QuizAnswer)int.Parse(ssObject["quizAnswer"].ToString());
			}
			
			return quizData;
		}
		
		private FITBChallengeData GetFITBChallengeData (JsonData ssObject) {
			FITBChallengeData fitbData = new FITBChallengeData();
			
			if (ssObject.Keys.Contains("fillCode")) {
					fitbData.fillCode = ssObject["fillCode"].ToString();
			}
			
			if (ssObject.Keys.Contains("fillOne")) {
					fitbData.fillOne = ssObject["fillOne"].ToString();
			}
			
			if (ssObject.Keys.Contains("fillTwo")) {
					fitbData.fillTwo = ssObject["fillTwo"].ToString();
			}
			
			if (ssObject.Keys.Contains("fillThree")) {
					fitbData.fillThree = ssObject["fillThree"].ToString();
			}
			
			if (ssObject.Keys.Contains("fillAnswer")) {
					fitbData.answer = (FITBAnswer)int.Parse(ssObject["fillAnswer"].ToString());
			}
			
			return fitbData;
		}
		
		private ChallengeType GetChallengeType (string type) {
			switch (type)
			{
				case "QUIZ":
					return ChallengeType.QUIZ;
				case "FITB":
					return ChallengeType.FITB;
				default:
					Debug.LogError ("Challenge type: " + type + " not found.");
					return ChallengeType.QUIZ;
			}
		}

	}
}