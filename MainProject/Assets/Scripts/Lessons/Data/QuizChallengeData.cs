namespace Codium.Challenges
{
	
	public enum QuizAnswer {ONE, TWO, THREE};
	
	[System.Serializable]
	public class QuizChallengeData {
		
		public string optionOne;
		public string optionTwo;
		public string optionThree;
		
		public QuizAnswer answer;
		
	}
	
}
