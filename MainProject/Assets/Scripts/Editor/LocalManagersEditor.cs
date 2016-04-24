using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Codium
{
	[CustomEditor(typeof(LocalManagers))]
	public class LocalManagersEditor : Editor {

		public override void OnInspectorGUI () {
			
			base.OnInspectorGUI();
			
			LocalManagers lm = (LocalManagers)target;
			Dictionary<string, Dictionary<GameObject, bool>> managers = lm.GetManagers();
			
			//Add elements if they aren't there
			while (managers.Count < EditorBuildSettings.scenes.Length) {
				EditorBuildSettingsScene scene = EditorBuildSettings.scenes[managers.Count];
				
				string name = scene.path.Substring(scene.path.LastIndexOf('/')+1);
				name = name.Substring(0, name.Length-6);
				
				Dictionary<GameObject, bool> managerIsOn = new Dictionary<GameObject, bool>();
				
				managers.Add (name, managerIsOn);
			}
			
			//Remove elements if there are too many
			foreach(KeyValuePair<string, Dictionary<GameObject, bool>> manager in managers)
			{
				bool isContained = false;
				for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
					EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
			
					string name = scene.path.Substring(scene.path.LastIndexOf('/')+1);
					name = name.Substring(0, name.Length-6);
					
					if (name == manager.Key) {
						isContained = true;
					}
				}
				
				if (!isContained) {
					managers.Remove(manager.Key);
				}
			}
			
			int managerIndex = 0;
			foreach(KeyValuePair<string, Dictionary<GameObject, bool>> manager in managers)
			{
				GUILayout.BeginHorizontal();
				
				GUILayout.Label(managerIndex.ToString() + ". " + manager.Key, GUILayout.Width(80));
				
				Dictionary<GameObject, bool> isOnDictionary = manager.Value;
				if (isOnDictionary == null) {
					isOnDictionary = new Dictionary<GameObject, bool>();
				}
				
				while (isOnDictionary.Count < lm.managerTypes.Length) {
					isOnDictionary.Add (lm.managerTypes[isOnDictionary.Count], false);
				}
				
				foreach(KeyValuePair<GameObject, bool> isOn in isOnDictionary) {
					
					isOnDictionary[isOn.Key] = EditorGUILayout.Toggle(isOn.Value, GUILayout.Width(60));;
					
				}
				
				managerIndex++;
				
				GUILayout.EndHorizontal();
			}
			
			lm.SetManagers(managers);
			
		}
		
	}
	
}