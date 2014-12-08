//	Used to automatically snap a shadow to a target (you could position it manually, this just makes it easier)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ShadowSnap : MonoBehaviour
{
	public RectTransform targetRect;
	RectTransform thisRect;

	public float xPadding = 0f;
	public float yPadding = 0f;

	public bool snapEveryFrame;
	
	void Start ()
	{
		if (!thisRect)
		{
			thisRect = gameObject.GetComponent<RectTransform> ();
		}
	}

	void LateUpdate ()
	{
		if (snapEveryFrame && targetRect)
		{
			thisRect.position = targetRect.position;
			
			Vector2 tempVect2 = targetRect.sizeDelta;
			tempVect2.x = targetRect.sizeDelta.x + xPadding;
			tempVect2.y = targetRect.sizeDelta.y + yPadding;
			
			thisRect.sizeDelta = tempVect2;
		}
	}

	public void Snap ()
	{
		if (targetRect)
		{
			if (!thisRect)
			{
				thisRect = gameObject.GetComponent<RectTransform> ();
			}

			thisRect.position = targetRect.position;

			Vector2 tempVect2 = targetRect.sizeDelta;
			tempVect2.x = targetRect.sizeDelta.x + xPadding;
			tempVect2.y = targetRect.sizeDelta.y + yPadding;

			thisRect.sizeDelta = tempVect2;
		}
		else
		{
			Debug.Log("No target rect! Please attach one.");
		}
	}
}
