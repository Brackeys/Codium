using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class LayoutElementHandler : MonoBehaviour {

	public bool setMinWidth = true, setMinHeight = true;
	public RectTransform target;

	private LayoutElement le;

	void Start () {
		le = GetComponent<LayoutElement>();

		if (setMinWidth)
			le.minWidth = target.rect.width;

		if (setMinHeight)
			le.minHeight = target.rect.height;
	}

}
