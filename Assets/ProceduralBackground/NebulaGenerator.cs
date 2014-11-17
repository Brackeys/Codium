using UnityEngine;
using System.Collections;

public class NebulaGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D noiseTex = GenerateNebulaTexture (256, 256);
		transform.renderer.material.mainTexture = noiseTex;
	}
	
	public static Texture2D GenerateNebulaTexture (int width, int height) {
		Texture2D noiseTex = new Texture2D (width, height);

		Color[] pixels = new Color [width * height];

		noiseTex.SetPixels(pixels);
		noiseTex.Apply();

		return noiseTex;
	}
}
