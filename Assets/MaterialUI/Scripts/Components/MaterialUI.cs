using UnityEngine;
using System.Collections;

public static class MaterialUI
{
	static GameObject inkBlot;
	static GameObject currentInkBlot;

	public static void InitializeInkBlots ()
	{
		inkBlot = Resources.Load ("MaterialUI/InkBlot", typeof(GameObject)) as GameObject;
	}

	public static GameObject MakeInkBlot (Vector2 position, Transform parent, int size, Color color)
	{
		currentInkBlot = GameObject.Instantiate (inkBlot) as GameObject;
		
		currentInkBlot.transform.parent = parent;
		currentInkBlot.transform.position = position;
		
		currentInkBlot.GetComponent<InkBlot> ().MakeInkBlot (size, 6f, 0.5f, 0.3f, color, new Vector3 (0, 0, 0));
		
		return currentInkBlot;
	}

	public static GameObject MakeInkBlot (Vector2 position, Transform parent, int size, float animSpeed, float startAlpha, float endAlpha, Color color)
	{
		currentInkBlot = GameObject.Instantiate (inkBlot) as GameObject;

		currentInkBlot.transform.SetParent (parent);
		currentInkBlot.transform.position = position;

		currentInkBlot.GetComponent<InkBlot> ().MakeInkBlot (size, animSpeed, startAlpha, endAlpha, color, new Vector3 (0, 0, 0));

		return currentInkBlot;
	}

	public static GameObject MakeInkBlot (Vector2 position, Transform parent, int size, float animSpeed, float startAlpha, float endAlpha, Color color, Vector3 endPosition)
	{
		currentInkBlot = GameObject.Instantiate (inkBlot) as GameObject;
		
		currentInkBlot.transform.SetParent (parent);
		currentInkBlot.transform.position = position;
		
		currentInkBlot.GetComponent<InkBlot> ().MakeInkBlot (size, animSpeed, startAlpha, endAlpha, color, endPosition);
		
		return currentInkBlot;
	}
}
