using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionListItemConfig : MonoBehaviour
{
	public bool inkBlotEnabled;
	public int inkBlotSize;
	public float inkBlotSpeed;
	public Color inkBlotColor;
	public float inkBlotStartAlpha;
	public float inkBlotEndAlpha;
	public bool highlightOnClick;
	public bool highlightOnHover;
	public int listId;

	InkBlotsControl inkBlotsControl;

	SelectionBoxConfig selectionBoxConfig;

	public void Setup()
	{
		selectionBoxConfig = gameObject.GetComponentInParent<SelectionBoxConfig> ();

//		Pass values to InkBlotsControl

		if (inkBlotEnabled)
		{
			if (gameObject.GetComponent<InkBlotsControl> ())
				inkBlotsControl = gameObject.GetComponent<InkBlotsControl> ();
			else
				inkBlotsControl = gameObject.AddComponent<InkBlotsControl> ();

			inkBlotsControl.inkBlotSize = inkBlotSize;
			inkBlotsControl.inkBlotSpeed = inkBlotSpeed;
			inkBlotsControl.inkBlotColor = inkBlotColor;
			inkBlotsControl.inkBlotStartAlpha = inkBlotStartAlpha;
			inkBlotsControl.inkBlotEndAlpha = inkBlotEndAlpha;

			MaterialUI.InitializeInkBlots ();
		}

//		Setup Button highlight and pressed colors to match inkblot

		if (highlightOnClick || highlightOnHover)
		{
			gameObject.GetComponentInChildren<Button> ().transition = Selectable.Transition.ColorTint;

			Color highlightColor = inkBlotColor;
			
			HSBColor highlightColorHSB = HSBColor.FromColor (highlightColor);
			

			if (highlightColorHSB.s <= 0.05f)
			{
				highlightColorHSB.s = 0f;
				highlightColorHSB.b = 0.9f;
			}
			else
			{
				highlightColorHSB.s = 0.1f;
				highlightColorHSB.b = 1f;
			}
			
			highlightColor = HSBColor.ToColor (highlightColorHSB);
			
			highlightColor.a = 1f;

			ColorBlock tempColorBlock = gameObject.GetComponent<Button> ().colors;
			tempColorBlock.normalColor = Color.white;

			if (highlightOnHover)
				tempColorBlock.highlightedColor = highlightColor;
			else
				tempColorBlock.highlightedColor = Color.white;

			if (highlightOnClick)
				tempColorBlock.pressedColor = highlightColor;
			else
				tempColorBlock.pressedColor = Color.white;

			tempColorBlock.disabledColor = Color.white;
			gameObject.GetComponent<Button> ().colors = tempColorBlock;
		}
	}

	public void SelectMe ()
	{
		selectionBoxConfig.Select (listId);
	}
}
