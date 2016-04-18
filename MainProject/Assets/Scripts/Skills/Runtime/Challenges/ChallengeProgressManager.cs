// *************************************
// This class keeps track of the users progress through the different
// challenges in a lesson.
// It is also responsible for deciding what challenge is up next.
// *************************************

using UnityEngine;
using System.Collections.Generic;

namespace Codium.Challenges
{
	public class ChallengeProgressManager : MonoBehaviour {

		//Private fields
		private ChallengeData m_currentChallenge;
		private List<ChallengeData> m_unfinishedChallenges;	//List of challenges yet to be completed
		
		//The number of completed challenges
		private int m_completedChallengeCount = 0;
		
		//Properties
		private LessonData CurrentLesson { get { return m_lessonManager.CurrentLesson; } }
		public ChallengeData CurrentChallenge { get { return m_currentChallenge; } }
		
		//Caching
		private LessonManager m_lessonManager;
		
		void Awake () {
			//Caching
			m_lessonManager = LessonManager.Instance;
			if (m_lessonManager == null) {
				Debug.LogError ("No LessonManager found!");
			}
		}
		
		//Return the percentage of challenges completed so far
		public float GetCompletionPercentage () {
			return (float)m_completedChallengeCount/CurrentLesson.challengesToBeCompleted * 100f;
		}
		
		//Completes a challenge
		public void CompleteChallenge () {
			//Remove the challenge from the unfinished list
			m_unfinishedChallenges.Remove(m_currentChallenge);
			
			//Increase the number of completed challenges
			m_completedChallengeCount++;
			
			if (m_completedChallengeCount >= CurrentLesson.challengesToBeCompleted) {
				Debug.Log ("LESSON COMPLETED!");
			}
		}
		
		//Finds the next challenge to be completed by the user
		public void ProgressToNextChallenge () {
			
			//If the list of challenges yet to be completed hasn't been initialized:
			//Load all of them in.
			if (m_unfinishedChallenges == null || m_unfinishedChallenges.Count == 0) {
				m_unfinishedChallenges = CurrentLesson.GetChallengesList();
			}
			
			//Find random challenge
			//Use do while loop to avoid getting the same challenge twice in a row
			ChallengeData _randChallenge = new ChallengeData();
			do {
				//Get random element index
				int _randIndex = Random.Range(0, m_unfinishedChallenges.Count);
				
				//Store the element in a temp var
				_randChallenge = m_unfinishedChallenges[_randIndex];
			} while (_randChallenge == m_currentChallenge);
			
			m_currentChallenge = _randChallenge;	//Return that element
		}
		
	}
	
}