using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIFollowSize : MonoBehaviour {

	public RectTransform target;

	private RectTransform rt;

	void Awake () {
		if (target == null) {
			Debug.LogWarning ("No target assigned, disabling.");
			this.enabled = false;
		}

		rt = GetComponent<RectTransform>();
	}

	void OnGUI () {
		rt.position = target.position;
		rt.sizeDelta = target.sizeDelta;
	}

}
