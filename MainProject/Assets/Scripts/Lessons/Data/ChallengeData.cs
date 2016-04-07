namespace Codium.Challenges
{
	[System.Serializable]
	public class ChallengeData {

		public ChallengeType type;
		
		public string mission;
		
		public QuizChallengeData quizChallengeData;
		
	}

	public enum ChallengeType
	{
		QUIZ, FITB
	}
	
}