using UnityEngine;

namespace Codium
{
	[CreateAssetMenu(fileName = "Skill", menuName = "Codium/Skill", order = 1)]
	public class SkillData : ScriptableObject
	{

		public string skillName;
		public LessonData[] lessons;
		
	}
}
