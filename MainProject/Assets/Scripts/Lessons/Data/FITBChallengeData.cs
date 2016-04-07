namespace Codium.Challenges
{
	public enum FITBAnswer {ONE, TWO, THREE};
	
	[System.Serializable]
	public class FITBChallengeData {
		
		// The code with a blank element
		public string fillCode;
		
		//The three options
		public string fillOne;
		public string fillTwo;
		public string fillThree;
		
		public FITBAnswer answer;
		
	}
	
}