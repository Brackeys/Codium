//-----------------------------------------------------------------
// Automatically adjusts the size of a shadow created by SoftshadowEffect (autoassigned)
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class SoftshadowAutoSize : MonoBehaviour {

	public Vector2 sizeRatio = new Vector2 (1f, 1f);

	[HideInInspector]
	public RectTransform imgrt;

	RectTransform rt;
	
	void Update () {
		if (rt == null) {
			rt = GetComponent<RectTransform> ();
			return;
		}

		if (imgrt == null) {
			Debug.LogError ("No image recttransform referenced.");
			return;
		}

		rt.sizeDelta = new Vector2 (imgrt.rect.width * sizeRatio.x, imgrt.rect.height * sizeRatio.y);
	}
}
