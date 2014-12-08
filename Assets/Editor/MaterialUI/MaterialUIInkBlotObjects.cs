using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public static class MaterialUIInkBlotObjects
{
	[MenuItem("Component/MaterialUI/Custom Inkblot Creator")]
	[MenuItem("MaterialUI/Add Component/Custom Inkblot Creator")]
	static void AddCustomInkBlotCreator()
	{
		GameObject selectedObject = Selection.activeGameObject;
		
		if (selectedObject)
		{
			if (GameObject.Find(selectedObject.name))
			{
				selectedObject.AddComponent("CustomInkBlotCreator");
			}
		}
	}
}
