//-----------------------------------------------------------------
// Creates a procedurally generated background with the ability to be animated.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ProceduralBackground : MonoBehaviour {

	public int pixWidth;
	public int pixHeight;
	public float xOrg, yOrg;
	public float minSample = 0.2f, maxSample = 0.8f;
	public float scale = 1.0F;
	public Gradient cc;

	public float fadeSpeed = 1;
	float fadeIndex = 0;
	int fadeDirection = -1;

	void Start() {

		// Scale the quad to fit the size of the viewport
		float quadHeight = Camera.main.orthographicSize * 2.0f;
		float quadWidth = quadHeight * Screen.width / Screen.height;
		transform.localScale = new Vector3(quadWidth, quadHeight, 1);
		
		GenerateTexture ("_MainTex");
		GenerateTexture ("_Texture2");
	}

	void Update() {
		TextureInterpolation ();
	}

	void TextureInterpolation () {
		if (fadeIndex >= 1) {
			GenerateTexture ("_MainTex");
			fadeDirection = -1;
		} else if (fadeIndex <= 0) {
			GenerateTexture ("_Texture2");
			fadeDirection = 1;
		}

		fadeIndex += fadeSpeed * Time.deltaTime * fadeDirection;

		transform.GetComponent<Renderer>().material.SetFloat("_Blend", Mathf.Clamp01(fadeIndex));
	}

	void GenerateTexture (string texName) {
		Texture2D noiseTex = new Texture2D(pixWidth, pixHeight);
		noiseTex.SetPixels(CalcNoise());
		noiseTex.Apply();
		transform.GetComponent<Renderer>().material.SetTexture (texName, noiseTex);
	}

	Color[] CalcNoise() {
		Color[] pix = new Color[pixWidth * pixHeight];

		xOrg = Random.Range (0, 1000);
		yOrg = Random.Range (0, 1000);

		float averageSample = 0f;
		int safety = 0;
		while (averageSample < minSample || averageSample > maxSample) {
			averageSample = 0f;
			float y = 0.0F;
			while (y < pixHeight) {
				float x = 0.0F;
				while (x < pixWidth) {
					float xCoord = xOrg + x / pixWidth * scale;
					float yCoord = yOrg + y / pixHeight * scale;
					float sample = Mathf.PerlinNoise(xCoord, yCoord);
					int index = (int)y * pixWidth + (int)x;
					Color pixelColor = cc.Evaluate(sample);
					pix[index] = pixelColor;
					averageSample += sample;
					x++;
				}
				y++;
			}

			// See if the average sample fits, if not: offset the texture
			averageSample /= pixHeight * pixWidth;
			if (averageSample < minSample || averageSample > maxSample) {
				xOrg += 10;
				yOrg += 10;
			}

			// Make sure the loop doesn't run for too long
			safety++;
			if (safety > 10) {
				Debug.LogWarning("CalcNoise had to run too many times.");
				break;
			}	
		} 

		return pix;
	}
}
