using UnityEngine;
using System.Collections.Generic;

namespace Codium
{
	[System.Serializable]
	public class SkillLevelData
	{

		public List<SkillData> skills;

		public SkillLevelData ()
		{
			skills = new List<SkillData>();
		}
		
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
