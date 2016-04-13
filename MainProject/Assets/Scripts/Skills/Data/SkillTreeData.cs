using UnityEngine;
using System.Collections.Generic;

namespace Codium
{
	[CreateAssetMenu(fileName = "Skill Tree", menuName = "Codium/Skill Tree", order = 1)]
	public class SkillTreeData : ScriptableObject
	{

		public List<SkillLevelData> skillLevels;

		public SkillTreeData ()
		{
			skillLevels = new List<SkillLevelData>();
		}

	#if UNITY_EDITOR

		public void AddSkillLevel (int index)
		{
			skillLevels.Insert(index, new SkillLevelData());
		}
		
		public void RemoveSkillLevel(int index)
		{
			if (index >= skillLevels.Count)
			{
				Debug.LogError("RemoveSkillLevel: Index out of bounds!");
				return;
			}
			skillLevels.RemoveAt(index);
		}
		
		public void MoveSkillLevelUp (int index) {
			SkillLevelData _levelUp = skillLevels[index];
			skillLevels[index] = skillLevels[index - 1];
			skillLevels[index - 1] = _levelUp;
		}
		
		public void MoveSkillLevelDown (int index) {
			SkillLevelData _levelUp = skillLevels[index];
			skillLevels[index] = skillLevels[index + 1];
			skillLevels[index + 1] = _levelUp;
		}
		
	#endif

	}
}
