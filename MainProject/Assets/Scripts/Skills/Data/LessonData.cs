// *************************************
// Data class storing information about a Lesson.
// A lesson contains a bunch of challenges.
// *************************************

using UnityEngine;
using Codium.Challenges;

namespace Codium
{
	[CreateAssetMenu(fileName = "Lesson", menuName = "Codium/Lesson", order = 1)]
	public class LessonData : ScriptableObject
	{

		//ID for the google spreadsheet to fetch data from.
		public string spreadsheetID;

		//Name of the lesson
		public string lessonName;

		//Array of all challenges in the lesson
		public ChallengeData[] challenges;

	}
}
