//-----------------------------------------------------------------
// Generates a box shadow effect on any given UI element
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class SoftshadowEffect : MonoBehaviour {

	public int size = 50;
	public Color color;
	public int offset = -1;

	public bool genOnStart = true;
	public bool sizeIsStatic = true;

	void Start () {
		if (genOnStart)
			GenerateBoxShadow ();
	}
	
	public void GenerateBoxShadow () {
		RectTransform rt = GetComponent<RectTransform>();

		int width = (int)rt.rect.width + size;
		int height = (int)rt.rect.height + size;

		Texture2D shadowTex = new Texture2D (width, height);

		Color[] pixels = new Color [width * height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float alpha = 1f;
				bool insideRect = true;

				if (x - offset + size/2f >= width) {
					alpha *= 1f - ((float)((x) - width + size/2f) / (size/2f));
					insideRect = false;
				}
				if (x + offset - size/2f < 0) {
					alpha *= (float)((x) / (size/2f));
					insideRect = false;
				}
				if (y - offset + size/2f >= height) {
					alpha *= 1f - ((float)((y) - height + size/2f) / (size/2f));
					insideRect = false;
				}
				if (y + offset - size/2f < 0) {
					alpha *= (float)((y) / (size/2f));
					insideRect = false;
				}

				if (!insideRect) {
					pixels [(y * width) + x]  = new Color (color.r, color.g, color.b, alpha * color.a);
				}
			}
		}

		shadowTex.SetPixels(pixels);
		shadowTex.Apply();

		Transform child = rt.transform.FindChild (rt.name + "_shadow");
		if (child != null) {
			DestroyImmediate (child.gameObject);
		}

		GameObject go = new GameObject();
		go.name = rt.name + "_shadow";
		go.transform.parent = rt;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		Image img = go.AddComponent<Image>();
		RectTransform imgrt = img.GetComponent<RectTransform>();
		imgrt.anchorMin = Vector2.zero;
		imgrt.anchorMax = Vector2.one;
		imgrt.sizeDelta = new Vector2 (size, size);

		if (!sizeIsStatic) {
			SoftshadowAutoSize autoSize = go.AddComponent<SoftshadowAutoSize>();
			autoSize.sizeRatio = new Vector2 ((float)size/shadowTex.width, (float)size/shadowTex.height);
			autoSize.imgrt = imgrt;
		}

		Sprite shadowSprite = Sprite.Create(shadowTex, new Rect (0,0, shadowTex.width, shadowTex.height), new Vector2 (0.5f, 0.5f));
		img.sprite = shadowSprite;
	}
}
