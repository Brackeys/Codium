// *************************************
// Data class storing information about a Lesson.
// A lesson contains a bunch of challenges.
// *************************************

using UnityEngine;
using Codium.Challenges;
using System.Collections.Generic;
using System.Linq;

namespace Codium
{
	[CreateAssetMenu(fileName = "Lesson", menuName = "Codium/Lesson", order = 1)]
	public class LessonData : ScriptableObject
	{

		//ID for the google spreadsheet to fetch data from.
		public string spreadsheetID;

		//Name of the lesson
		public string lessonName = "Hello, World";

		//The number of challenges the user must complete to finish the lesson
		public int challengesToBeCompleted = 8;

		//Array of all challenges in the lesson
		public ChallengeData[] challenges;
		
		public List<ChallengeData> GetChallengesList () {
			return challenges.ToList();
		}

	}
}
