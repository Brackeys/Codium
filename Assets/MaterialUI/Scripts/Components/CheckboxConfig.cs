﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckboxConfig : MonoBehaviour
{
	public bool inkBlotEnabled = true;
	public bool autoInkBlotSize = true;
	public int inkBlotSize;
	public float inkBlotSpeed = 8f;
	public Color inkBlotColor = Color.black;
	public float inkBlotStartAlpha = 0.5f;
	public float inkBlotEndAlpha = 0.3f;

	public Image frameImage;
	public Image boxImage;
	public Image checkImage;

	RectTransform boxRect;
	RectTransform checkRect;

	float boxSize;
	float checkSize;

	float animStartTime;
	float animDeltaTime;

	Vector2 tempVec2;
	Vector3 tempVec3;

	int state;
	
	InkBlotsControl inkBlotsControl;
	Toggle toggle;

	void Start ()
	{
		toggle = gameObject.GetComponent <Toggle> ();
		boxRect = boxImage.gameObject.GetComponent<RectTransform> ();
		checkRect = checkImage.gameObject.GetComponent<RectTransform> ();

		Setup ();
	}

	public void Setup ()
	{
//		Pass values to InkBlotsControl
		if (inkBlotEnabled)
		{
			inkBlotsControl = gameObject.AddComponent<InkBlotsControl> ();
			
//			Calculate ink blot size based on RectTransform size
			if (autoInkBlotSize)
			{
				Vector2 size = gameObject.GetComponent<RectTransform> ().sizeDelta;
				
				if (size.x > size.y)
				{
					inkBlotSize = Mathf.RoundToInt(size.x / 1.5f);
				}
				else
				{
					inkBlotSize =  Mathf.RoundToInt(size.y / 1.5f);
				}
			}
			
			inkBlotsControl.inkBlotSize = inkBlotSize;
			inkBlotsControl.inkBlotSpeed = inkBlotSpeed;
			inkBlotsControl.inkBlotColor = inkBlotColor;
			inkBlotsControl.inkBlotStartAlpha = inkBlotStartAlpha;
			inkBlotsControl.inkBlotEndAlpha = inkBlotEndAlpha;
			inkBlotsControl.moveTowardCenter = true;
			
			MaterialUI.InitializeInkBlots ();
			ToggleCheckbox (false);
		}
	}
	
	public void ToggleCheckbox (bool state)
	{
		if (toggle.isOn)
			TurnOn ();
		else
			TurnOff ();
	}

	void TurnOn ()
	{
		boxImage.enabled = true;
		checkImage.enabled = true;
		checkSize = checkRect.localScale.x;
		boxSize = boxRect.localScale.x;
		animStartTime = Time.realtimeSinceStartup;
		state = 1;
	}

	void TurnOff ()
	{
		frameImage.enabled = true;
		checkSize = checkRect.localScale.x;
		boxSize = boxRect.localScale.x;
		animStartTime = Time.realtimeSinceStartup;
		state = 2;
	}

	void Update ()
	{
		animDeltaTime = Time.realtimeSinceStartup - animStartTime;

		if (state == 1)    // Turning on
		{
			if (animDeltaTime <= 0.3f)
			{
				tempVec3 = boxRect.localScale;
				tempVec3.x = Anims.EaseOutQuint(boxSize, 1f, animDeltaTime, 0.3f);
				tempVec3.y = tempVec3.x;
				tempVec3.z = 1f;
				boxRect.localScale = tempVec3;

				tempVec3 = checkRect.localScale;
				tempVec3.x = Anims.EaseInQuint(checkSize, 1f, animDeltaTime, 0.3f);
				tempVec3.y = tempVec3.x;
				tempVec3.z = 1f;
				checkRect.localScale = tempVec3;
			}
			else
			{
				boxRect.localScale = new Vector3 (1f, 1f, 1f);
				checkRect.localScale = new Vector3 (1f, 1f, 1f);

				frameImage.enabled = false;
			}
		}
		else if (state == 2)    // Turning off
		{
			if (animDeltaTime <= 0.3f)
			{
				tempVec3 = boxRect.localScale;
				tempVec3.x = Anims.EaseInQuint(boxSize, 0f, animDeltaTime, 0.3f);
				tempVec3.y = tempVec3.x;
				tempVec3.z = 1f;
				boxRect.localScale = tempVec3;
				
				tempVec3 = checkRect.localScale;
				tempVec3.x = Anims.EaseOutQuint(checkSize, 0f, animDeltaTime, 0.3f);
				tempVec3.y = tempVec3.x;
				tempVec3.z = 1f;
				checkRect.localScale = tempVec3;
			}
			else
			{
				boxRect.localScale = Vector3.zero;
				checkRect.localScale = Vector3.zero;
				
				boxImage.enabled = false;
				checkImage.enabled = false;
			}
		}
	}
}
