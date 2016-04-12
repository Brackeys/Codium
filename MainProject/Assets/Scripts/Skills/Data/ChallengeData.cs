namespace Codium.Challenges
{
	[System.Serializable]
	public class ChallengeData {
		
		public ChallengeData () {
			
		}

		public ChallengeType type;
		
		public string mission;
		
		public QuizChallengeData quizChallengeData;
		public FITBChallengeData fitbChallengeData;
		
	}

	public enum ChallengeType
	{
		QUIZ, FITB
	}
	
}