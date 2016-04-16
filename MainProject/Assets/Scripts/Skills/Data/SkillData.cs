// *************************************
// Data class storing information about a skill.
// A skill hosts a certain amount of lessons.
// *************************************

using UnityEngine;

namespace Codium
{
	[CreateAssetMenu(fileName = "Skill", menuName = "Codium/Skill", order = 1)]
	public class SkillData : ScriptableObject
	{

		//Name of the skill
		public string skillName;
		
		//Array of all lessons in the skill
		public LessonData[] lessons;
		
	}
}
