using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UICopyPos : MonoBehaviour {

	public RectTransform target;

	public bool xPos = false, yPos = true, zPos = false;

	private RectTransform rt;

	void Awake () {
		rt = GetComponent<RectTransform>();
	}

	void Update () {
		float x = rt.position.x;
		float y = rt.position.y;
		float z = rt.position.z;

		if (xPos)
			x = target.position.x;
		if (yPos)
			y = target.position.y;
		if (zPos)
			z = target.position.z;

		rt.position = new Vector3 (x, y, z);
	}
}
