using UnityEngine;
using System.Collections;

public class NebulaGenerator : MonoBehaviour {

	public int width = 256;
	public int height = 256;

	public int seed;
	public float scale = 10f;
	public float reduction = 10f;

	void Start () {
		GenerateNebula ();
	}

	// Use this for initialization
	public void GenerateNebula () {
		Texture2D noiseTex = GenerateNebulaTexture ();
		transform.renderer.material.mainTexture = noiseTex;
	}
	
	public Texture2D GenerateNebulaTexture () {
		if (seed != 0) {
			Random.seed = seed;
		} else { Random.seed = Random.Range (-1000, 1000); }

		float xOrg = Random.Range (0f, 1000f);
		float yOrg = Random.Range (0f, 1000f);

		Texture2D noiseTex = new Texture2D (width, height);

		Color[] pixels = new Color [width * height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float xCoord = xOrg + (float)x / noiseTex.width * scale;
				float yCoord = yOrg + (float)y / noiseTex.height * scale;

				float noiseSample = Mathf.PerlinNoise(xCoord, yCoord);

				float xCoord2 = xOrg + 100 + (float)x / noiseTex.width * scale/2f;
				float yCoord2 = yOrg + 100 + (float)y / noiseTex.height * scale/2f;

				float noiseSample2 = Mathf.PerlinNoise(xCoord2, yCoord2);

				noiseSample *= noiseSample2;

				noiseSample /= reduction;

				Color pix = new Color (1, 1, 1, noiseSample);
				pixels [(y * width) + x] = pix;
			}
		}

		noiseTex.SetPixels(pixels);
		noiseTex.Apply();

		Debug.Log ("Calculated NoiseTex with seed: " + Random.seed);

		return noiseTex;
	}
}
