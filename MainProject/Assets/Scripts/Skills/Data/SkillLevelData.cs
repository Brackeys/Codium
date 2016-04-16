// *************************************
// Data class for information about the SkillLevel.
// A skill level hosts a certain amount of skills.
// *************************************

using UnityEngine;
using System.Collections.Generic;

namespace Codium
{
	[System.Serializable]
	public class SkillLevelData
	{

		//A list of skills in the SkillLevel
		public List<SkillData> skills;

		public SkillLevelData ()
		{
			skills = new List<SkillData>();
		}
		
		//In the unity editor I've added some utility methods for managing skills in the list
	#if UNITY_EDITOR

		public void AddSkill(int index)
		{
			skills.Insert(index, null);
		}
		public void RemoveSkill(int index)
		{
			if (index >= skills.Count)
			{
				Debug.LogError("RemoveSkill: Index out of bounds!");
				return;
			}
			skills.RemoveAt(index);
		}
		
		public void MoveSkillUp (int index) {
			SkillData _levelUp = skills[index];
			skills[index] = skills[index + 1];
			skills[index + 1] = _levelUp;
		}
		
	#endif

	}
}
