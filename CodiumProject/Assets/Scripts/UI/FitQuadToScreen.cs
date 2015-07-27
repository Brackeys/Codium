using UnityEngine;
using System.Collections;

public class FitQuadToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (FitQuad (transform, Camera.main));
	}

	private IEnumerator FitQuad (Transform quad, Camera cam) {
		while(true)
		{
			// Scale the quad to fit the size of the viewport
			float quadHeight = cam.orthographicSize * 2.0f;
			float quadWidth = quadHeight * Screen.width / Screen.height;
			quad.localScale = new Vector3(quadWidth, quadHeight, 1);

			yield return new WaitForSeconds(1);
		}
	}
	
}
