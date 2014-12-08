using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InkBlotsControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	[HideInInspector()]
	public int inkBlotSize;
	[HideInInspector()]
	public float inkBlotSpeed;
	[HideInInspector()]
	public Color inkBlotColor;
	[HideInInspector()]
	public float inkBlotStartAlpha;
	[HideInInspector()]
	public float inkBlotEndAlpha;
	[HideInInspector()]
	public bool moveTowardCenter;

	[HideInInspector()]
	public bool dontTurnOffMask;

	InkBlot currentInkBlot;
	Mask thisMask;
	
	void OnEnable ()
	{
		if (!moveTowardCenter)
		{
			if(gameObject.GetComponent<Mask> ())
				thisMask = gameObject.GetComponent<Mask>();
			else
				thisMask = gameObject.AddComponent<Mask>();
			
			thisMask.enabled = false;
		}

		if (gameObject.GetComponent<SelectionBoxConfig> ())
		{
			dontTurnOffMask = true;
			thisMask.enabled = true;
		}

		MaterialUI.InitializeInkBlots ();
	}

	public void OnPointerDown (PointerEventData data)
	{
		MakeInkBlot (data.position);

		if (!moveTowardCenter && !dontTurnOffMask)
			thisMask.enabled = true;
	}
	
	public void OnPointerUp (PointerEventData data)
	{
		if (!moveTowardCenter && !dontTurnOffMask)
			StartCoroutine (DelayedMaskCheck());
			
		if (currentInkBlot)
		{
			currentInkBlot.ClearInkBlot ();
		}
		
		currentInkBlot = null;
	}

	public void OnPointerExit (PointerEventData data)
	{
		if (!moveTowardCenter && !dontTurnOffMask)
			StartCoroutine (DelayedMaskCheck());
			
		if (currentInkBlot)
		{
			currentInkBlot.ClearInkBlot ();
		}
		
		currentInkBlot = null;
	}

	void MakeInkBlot (Vector2 pos)
	{
		if (currentInkBlot)
		{
			currentInkBlot.ClearInkBlot ();
		}

		if (moveTowardCenter)
			currentInkBlot = MaterialUI.MakeInkBlot (pos, transform, inkBlotSize, inkBlotSpeed, inkBlotStartAlpha, inkBlotEndAlpha, inkBlotColor, gameObject.GetComponent<RectTransform>().position).GetComponent<InkBlot>();
		else
			currentInkBlot = MaterialUI.MakeInkBlot (pos, transform, inkBlotSize, inkBlotSpeed, inkBlotStartAlpha, inkBlotEndAlpha, inkBlotColor).GetComponent<InkBlot>();
	}
	
	IEnumerator DelayedMaskCheck()
	{
		yield return new WaitForSeconds(1f);
		if (!gameObject.GetComponentInChildren<InkBlot>())
		{
			thisMask.enabled = false;
		}
	}
}
