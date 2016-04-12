using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

namespace Codium
{
	public class SkillTreeEditorWindow : EditorWindow
	{
		private const string SKILL_TREE_NAME = "SkillTree";

		private SkillTreeData m_skillTree;

		private Vector2 m_scrollPos = Vector2.zero;

		[MenuItem("Window/Skill Editor")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			SkillTreeEditorWindow window = (SkillTreeEditorWindow)EditorWindow.GetWindow(typeof(SkillTreeEditorWindow));
			window.position = new Rect(100, 100, 1000, 700);
			window.Show();
			window.titleContent.text = "Skill Editor";

			window.Setup();
        }

		public void Setup()
		{
			m_skillTree = (SkillTreeData)Resources.Load(SKILL_TREE_NAME);
        }

		void OnGUI ()
		{
			if (m_skillTree == null)
				Setup();

			EditorGUI.DrawRect(new Rect(0,0, maxSize.x, maxSize.y), new Color(.1f, .1f, .1f));

			GUI.color = Color.white;

			if (m_skillTree.skillLevels.Count == 0)
			{
				if (GUI.Button(new Rect(5, 5, 100, 50), "ADD LEVEL"))
				{
					m_skillTree.AddSkillLevel(0);
				}
			}

			EditorGUILayout.BeginVertical();
			m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Height(position.height));

			for (int i = 0; i < m_skillTree.skillLevels.Count; i++)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				
				SkillLevelData _skillLevel = m_skillTree.skillLevels[i];

				GUILayout.Label("Skill Level " + i.ToString(), EditorStyles.largeLabel);

				GUILayout.Space(5);

				EditorGUILayout.BeginHorizontal();

				if (_skillLevel.skills.Count == 0)
				{
					if (GUILayout.Button("+"))
					{
						_skillLevel.AddSkill(0);
					}
				}

				for (int j = 0; j < _skillLevel.skills.Count; j++)
				{
					EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

					SkillData _skillData = _skillLevel.skills[j];

					EditorGUILayout.BeginVertical();

					_skillData = (SkillData)EditorGUILayout.ObjectField(_skillData, typeof(SkillData), false);

					if (_skillData != null)
					{
						_skillData.skillName = EditorGUILayout.TextField(_skillData.skillName);
					}

					EditorGUILayout.EndVertical();

					_skillLevel.skills[j] = _skillData;

					if (GUILayout.Button("X"))
					{
						_skillLevel.RemoveSkill(j);
						continue;
					}
					if (GUILayout.Button("+"))
					{
						_skillLevel.AddSkill(j + 1);
					}

					EditorGUILayout.EndHorizontal();
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.Space(5);

				GUILayout.BeginHorizontal();

				if (GUILayout.Button("ADD"))
				{
					m_skillTree.AddSkillLevel(i + 1);
				}
				if (GUILayout.Button("REMOVE"))
				{
					m_skillTree.RemoveSkillLevel(i);
					continue;
				}

				GUILayout.EndHorizontal();

				m_skillTree.skillLevels[i] = _skillLevel;

				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

	}
}
