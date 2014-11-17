using UnityEngine;
using System.Collections;

public class FitQuadToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FitQuad (transform, Camera.main);
	}

	public static void FitQuad (Transform quad, Camera cam) {
		// Scale the quad to fit the size of the viewport
		float quadHeight = cam.orthographicSize * 2.0f;
		float quadWidth = quadHeight * Screen.width / Screen.height;
		quad.localScale = new Vector3(quadWidth, quadHeight, 1);
	}
	
}
