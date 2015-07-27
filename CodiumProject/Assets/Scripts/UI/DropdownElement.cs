using UnityEngine;

[System.Serializable]
public class DropdownElement
{
	public int index;
	public string title;

	public RectTransform elementRef;

	public DropdownElement()
	{
		index = 0;
		title = "Default";

		elementRef = null;
	}

	public DropdownElement(int _index, string _title, RectTransform _ref)
	{
		index = _index;
		title = _title;

		elementRef = _ref;
	}
}
