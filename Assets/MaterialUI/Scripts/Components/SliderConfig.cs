using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderConfig : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Color textColor;
	public bool textHasDecimal;
	public bool hasPopup = true;

	public RectTransform handle;
	public RectTransform popup;
	public Text popupText;

	Slider slider;

	float currentPopupScale;
	float currentHandleScale;
	float currentPos;

	bool isSelected;
	int state;

	float animStartTime;
	float animDeltaTime;

	Vector3 tempVec3;

	void Start ()
	{
		slider = gameObject.GetComponent<Slider> ();

		popup.gameObject.GetComponent<Image> ().color = handle.gameObject.GetComponent<Image> ().color;
		popupText.color = textColor;

		UpdateText ();
	}

	void Update ()
	{
		if (state == 1)
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (animDeltaTime <= 0.5f)
			{
				tempVec3 = handle.localScale;
				tempVec3.x = Anims.EaseOutQuint(currentHandleScale, 1f, animDeltaTime, 0.5f);
				tempVec3.y = tempVec3.x;
				tempVec3.z = tempVec3.x;
				handle.localScale = tempVec3;

				if (hasPopup)
				{
					tempVec3 = popup.localScale;
					tempVec3.x = Anims.EaseOutQuint(currentPopupScale, 1f, animDeltaTime, 0.5f);
					tempVec3.y = tempVec3.x;
					tempVec3.z = tempVec3.x;
					popup.localScale = tempVec3;

					tempVec3 = popup.localPosition;
					tempVec3.y = Anims.EaseOutQuint(currentPos, 12f, animDeltaTime, 0.5f);
					popup.localPosition = tempVec3;
				}
			}
			else
			{
				state = 0;
			}
		}
		else if (state == 2)
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;
			
			if (animDeltaTime <= 0.5f)
			{
				tempVec3 = handle.localScale;
				tempVec3.x = Anims.EaseOutQuint(currentHandleScale, 0.6f, animDeltaTime, 0.5f);
				tempVec3.y = tempVec3.x;
				tempVec3.z = tempVec3.x;
				handle.localScale = tempVec3;
				
				if (hasPopup)
				{
					tempVec3 = popup.localScale;
					tempVec3.x = Anims.EaseOutQuint(currentPopupScale, 0f, animDeltaTime, 0.5f);
					tempVec3.y = tempVec3.x;
					tempVec3.z = tempVec3.x;
					popup.localScale = tempVec3;
				
					tempVec3 = popup.localPosition;
					tempVec3.y = Anims.EaseOutQuint(currentPos, 0f, animDeltaTime, 0.5f);
					popup.localPosition = tempVec3;
				}
			}
			else
			{
				state = 0;
			}
		}
	}

	public void UpdateText ()
	{
		if (textHasDecimal)
			popupText.text = slider.value.ToString("0.0");
		else
			popupText.text = slider.value.ToString("0");
	}

	public void OnPointerDown (PointerEventData data)
	{
		currentHandleScale = handle.localScale.x;
		currentPopupScale = popup.localScale.x;
		currentPos = popup.localPosition.y;

		animStartTime = Time.realtimeSinceStartup;

		isSelected = true;
		state = 1;
	}
	
	public void OnPointerUp (PointerEventData data)
	{
		if (isSelected)
		{
			currentHandleScale = handle.localScale.x;
			currentPopupScale = popup.localScale.x;
			currentPos = popup.localPosition.y;
			
			animStartTime = Time.realtimeSinceStartup;

			isSelected = false;
			state = 2;
		}
	}
}
