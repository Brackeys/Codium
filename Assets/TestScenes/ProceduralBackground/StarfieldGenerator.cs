using UnityEngine;
using System.Collections;

public class StarfieldGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D noiseTex = GenerateStarfield (512, 512, 250);
		transform.renderer.material.mainTexture = noiseTex;
	}
	
	public static Texture2D GenerateStarfield (int width, int height, int starCount) {
		Texture2D noiseTex = new Texture2D (width, height);

		Color[] pixels = new Color [width * height];

		for (int i = starCount; i > 0; i--) {
			int x = Random.Range (2, width-2);
			int y = Random.Range (2, height-2);
			float alpha = Random.Range (0.1f, 1f);

			// Create center star
			pixels [(y * width) + x] = new Color (1,1,1, alpha);

			// Create sparkle around star
			pixels [(y * width) + (x + 1)] = new Color (1,1,1, alpha/2);
			pixels [((y + 1) * width) + x] = new Color (1,1,1, alpha/2);
			pixels [(y * width) + (x - 1)] = new Color (1,1,1, alpha/2);
			pixels [((y - 1) * width) + x] = new Color (1,1,1, alpha/2);
		}

		noiseTex.SetPixels(pixels);
		noiseTex.Apply();

		return noiseTex;
	}
}
