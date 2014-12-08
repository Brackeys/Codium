﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchConfig : MonoBehaviour
{

	public bool inkBlotEnabled = true;
	public bool autoInkBlotSize = true;
	public int inkBlotSize;
	public float inkBlotSpeed = 8f;
	public Color inkBlotColor = Color.black;
	public float inkBlotStartAlpha = 0.5f;
	public float inkBlotEndAlpha = 0.3f;
	public Color switchOnColor;

	public Image switchImage;
	public Image switchBackImage;
	RectTransform switchRect;

	Color switchOffColor;
	Color switchBackOffColor;

	Color color;
	Color color2;

	float switchPos;
	
	float animStartTime;
	float animDeltaTime;
	
	Vector3 tempVec3;
	
	int state;
	
	InkBlotsControl inkBlotsControl;
	Toggle toggle;
	
	void Start ()
	{
		toggle = gameObject.GetComponent <Toggle> ();
		switchRect = switchImage.gameObject.GetComponent<RectTransform> ();
		
		if (inkBlotEnabled)
		{
			inkBlotsControl = gameObject.AddComponent<InkBlotsControl> ();
			
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

			switchOffColor = switchImage.color;
			switchBackOffColor = switchBackImage.color;

			MaterialUI.InitializeInkBlots ();
			ToggleSwitch (false);
		}
	}
	
	public void ToggleSwitch (bool state)
	{
		if (toggle.isOn)
			TurnOn ();
		else
			TurnOff ();
	}
	
	void TurnOn ()
	{
		switchPos = switchRect.localPosition.x;
		color = switchImage.color;
		color2 = switchBackImage.color;
		animStartTime = Time.realtimeSinceStartup;
		state = 1;
	}
	
	void TurnOff ()
	{
		switchPos = switchRect.localPosition.x;
		color = switchImage.color;
		color2 = switchBackImage.color;
		animStartTime = Time.realtimeSinceStartup;
		state = 2;
	}
	
	void Update ()
	{
		animDeltaTime = Time.realtimeSinceStartup - animStartTime;
		
		if (state == 1)	// Turning on
		{
			if (animDeltaTime <= 0.5f)
			{
				tempVec3 = switchRect.localPosition;
				tempVec3.x = Anims.EaseInOutQuint(switchPos, 8f, animDeltaTime, 0.5f);
				tempVec3.z = 1f;
				switchRect.localPosition = tempVec3;

				Color tempColor = switchImage.color;
				tempColor.r = Anims.EaseInOutQuint(color.r, switchOnColor.r, animDeltaTime, 0.5f);
				tempColor.g = Anims.EaseInOutQuint(color.g, switchOnColor.g, animDeltaTime, 0.5f);
				tempColor.b = Anims.EaseInOutQuint(color.b, switchOnColor.b, animDeltaTime, 0.5f);
				tempColor.a = Anims.EaseInOutQuint(color.a, switchOnColor.a, animDeltaTime, 0.5f);
				switchImage.color = tempColor;

				tempColor = switchBackImage.color;
				tempColor.r = Anims.EaseInOutQuint(color2.r, switchOnColor.r, animDeltaTime, 0.5f);
				tempColor.g = Anims.EaseInOutQuint(color2.g, switchOnColor.g, animDeltaTime, 0.5f);
				tempColor.b = Anims.EaseInOutQuint(color2.b, switchOnColor.b, animDeltaTime, 0.5f);
				tempColor.a = Anims.EaseInOutQuint(color2.a, switchOnColor.a, animDeltaTime, 0.5f);
				switchBackImage.color = tempColor;
			}
			else
			{
				switchRect.localPosition = new Vector3 (8f, 0f, 0f);
				switchImage.color = switchOnColor;
				switchBackImage.color = switchOnColor;
				state = 0;
			}
		}
		else if (state == 2)	// Turning off
		{
			if (animDeltaTime <= 0.5f)
			{
				tempVec3 = switchRect.localPosition;
				tempVec3.x = Anims.EaseInOutQuint(switchPos, -8f, animDeltaTime, 0.5f);
				tempVec3.z = 1f;
				switchRect.localPosition = tempVec3;
				
				Color tempColor = switchImage.color;
				tempColor.r = Anims.EaseInOutQuint(color.r, switchOffColor.r, animDeltaTime, 0.5f);
				tempColor.g = Anims.EaseInOutQuint(color.g, switchOffColor.g, animDeltaTime, 0.5f);
				tempColor.b = Anims.EaseInOutQuint(color.b, switchOffColor.b, animDeltaTime, 0.5f);
				tempColor.a = Anims.EaseInOutQuint(color.a, switchOffColor.a, animDeltaTime, 0.5f);
				switchImage.color = tempColor;
				
				tempColor = switchBackImage.color;
				tempColor.r = Anims.EaseInOutQuint(color2.r, switchBackOffColor.r, animDeltaTime, 0.5f);
				tempColor.g = Anims.EaseInOutQuint(color2.g, switchBackOffColor.g, animDeltaTime, 0.5f);
				tempColor.b = Anims.EaseInOutQuint(color2.b, switchBackOffColor.b, animDeltaTime, 0.5f);
				tempColor.a = Anims.EaseInOutQuint(color2.a, switchBackOffColor.a, animDeltaTime, 0.5f);
				switchBackImage.color = tempColor;
			}
			else
			{
				switchRect.localPosition = new Vector3 (-8f, 0f, 0f);
				switchImage.color = switchOffColor;
				switchBackImage.color = new Color (switchBackOffColor.r, switchBackOffColor.g, switchBackOffColor.b, 0.5f);
				state = 0;
			}
		}
	}
}
