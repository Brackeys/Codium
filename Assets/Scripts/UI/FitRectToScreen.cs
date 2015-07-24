using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class FitRectToScreen : MonoBehaviour {

	private RectTransform rt;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
		StartCoroutine (FitRect ());
	}
	
	private IEnumerator FitRect () {
		while(true)
		{
			rt.sizeDelta = new Vector2(Screen.width, Screen.height);
			rt.position = new Vector3 (0,0);
			rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);
			
			yield return new WaitForSeconds(1);
		}
	}
	
}
