using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class SnapButtonToText : MonoBehaviour
{
	[Header("Works best if you save, snap, then save again")]
	public RectTransform buttonLayerRect;
	public RectTransform textRect;

	public bool buttonPadding = true;

	RectTransform thisRect;
	HorizontalLayoutGroup layoutGroup;
	ContentSizeFitter sizeFitter;
	
	public void Snap ()
	{
		if (!thisRect)
		{
			thisRect = gameObject.GetComponent<RectTransform> ();
		}

		if (thisRect && buttonLayerRect && textRect)
		{
			StartCoroutine(SnapEnum());
		}
		else
		{
			Debug.Log("Missing components!");
		}
	}

	IEnumerator SnapEnum()
	{
		layoutGroup = buttonLayerRect.gameObject.AddComponent("HorizontalLayoutGroup") as HorizontalLayoutGroup;
		layoutGroup.padding = new RectOffset (16, 16, 9, 7);
		layoutGroup.childAlignment = TextAnchor.MiddleCenter;
		layoutGroup.childForceExpandWidth = true;
		layoutGroup.childForceExpandHeight = true;

		sizeFitter = buttonLayerRect.gameObject.AddComponent ("ContentSizeFitter") as ContentSizeFitter;
		sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		yield return new WaitForEndOfFrame();

		DestroyImmediate (layoutGroup);

		DestroyImmediate (sizeFitter);

		Vector2 buttonSize = new Vector2 (textRect.sizeDelta.x + 24, textRect.sizeDelta.y + 16);

		if (buttonSize.x < 88f)
			buttonSize.x = 88f;

		if (buttonPadding)
			thisRect.sizeDelta = new Vector2 (buttonSize.x + 32, buttonSize.y + 32);
		else
			thisRect.sizeDelta = buttonSize;

		buttonLayerRect.sizeDelta = buttonSize;

		textRect.anchorMin = new Vector2 (0.5f, 0.5f);
		textRect.anchorMax = new Vector2 (0.5f, 0.5f);
		textRect.anchoredPosition = new Vector2 (0f, 0f);

		buttonLayerRect.anchorMin = new Vector2 (0.5f, 0.5f);
		buttonLayerRect.anchorMax = new Vector2 (0.5f, 0.5f);
		buttonLayerRect.anchoredPosition = new Vector2 (0f, 0f);
	}
}
