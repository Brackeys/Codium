using UnityEngine;
using Codium.Challenges;

namespace Codium
{
	[CreateAssetMenu(fileName = "Lesson", menuName = "Codium/Lesson", order = 1)]
	public class LessonData : ScriptableObject
	{

		public string spreadsheetID;

		public string lessonName;

		public ChallengeData[] challenges;

	}
}
