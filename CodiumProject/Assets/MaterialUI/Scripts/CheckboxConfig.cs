﻿//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class CheckboxConfig : MonoBehaviour
	{
		[HideInInspector]
		public float animationDuration = 0.5f;

		[HideInInspector]
		public Color onColor;
		[HideInInspector]
		public Color offColor;
		[HideInInspector]
		public Color disabledColor;

		[HideInInspector]
		public bool changeTextColor;

		[HideInInspector]
		public Color textNormalColor;
		[HideInInspector]
		public Color textDisabledColor;

		[HideInInspector]
		public bool changeRippleColor;


		[HideInInspector]
		[SerializeField] private Image checkImage;
		[HideInInspector]
		[SerializeField] private Image frameImage;
		[HideInInspector]
		[SerializeField] private Text text;

		private RectTransform checkRectTransform;
		private CanvasGroup frameCanvasGroup;
		private CheckBoxToggler checkBoxToggler;
		private RippleConfig rippleConfig;

		Toggle toggle;

		private bool lastToggleInteractableState;
		private bool lastToggleState;

		private float currentCheckSize;
		private Color currentColor;
		private Color currentTextColor;
		private float currentFrameAlpha;

		private Vector3 tempVector3;
		private Color tempColor;

		private int state;
		private float animStartTime;
		private float animDeltaTime;

		void OnEnable()
		{
			//	Set references
			toggle = gameObject.GetComponent<Toggle>();
			checkRectTransform = checkImage.GetComponent<RectTransform>();
			frameCanvasGroup = frameImage.GetComponent<CanvasGroup>();
			checkBoxToggler = text.GetComponent<CheckBoxToggler>();
			rippleConfig = gameObject.GetComponent<RippleConfig>();
		}

		void Start()
		{
            lastToggleInteractableState = toggle.interactable;

            //  Makes sure the CheckboxConfig is in sync with the Toggle component's 'isOn' bool
			if (lastToggleInteractableState)
			{
				if (lastToggleState != toggle.isOn)
				{
					lastToggleState = toggle.isOn;

					if (lastToggleState)
						TurnOnSilent();
					else
						TurnOffSilent();
				}
			}

            if (changeRippleColor)
				rippleConfig.rippleColor = frameImage.color;
		}

		public void ToggleCheckbox ()
		{
			if (toggle.isOn)
				TurnOn ();
			else
				TurnOff ();
		}

		public void TurnOn()
		{
			checkImage.enabled = true;

            //  Sets the values to animate from
			currentCheckSize = checkRectTransform.localScale.x;
			currentColor = frameImage.color;
			currentTextColor = text.color;
			currentFrameAlpha = frameCanvasGroup.alpha;

            //  Starts animation
			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

        //  Turns on the checkbox, but doesn't play any animations
		private void TurnOnSilent()
		{
			checkImage.enabled = true;
			if (checkRectTransform.localScale != new Vector3(1f, 1f, 1f))
				checkRectTransform.localScale = new Vector3(1f, 1f, 1f);
			
			frameCanvasGroup.alpha = 0f;

			if (lastToggleInteractableState)
			{

				frameImage.color = onColor;
				if (changeTextColor)
					text.color = onColor;

				if (changeRippleColor)
					rippleConfig.rippleColor = onColor;
			}

			frameImage.enabled = false;
		}

		public void TurnOff()
		{
			frameImage.enabled = true;

            //  Sets the values to animate from
			currentCheckSize = checkRectTransform.localScale.x;
			currentColor = frameImage.color;
			currentTextColor = text.color;
			currentFrameAlpha = frameCanvasGroup.alpha;

            //  Starts animation
			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

        //  Turns off the checkbox without playing animations
		private void TurnOffSilent()
		{
			frameImage.enabled = true;
			if (checkRectTransform.localScale != new Vector3(0f, 0f, 1f))
				checkRectTransform.localScale = new Vector3(0f, 0f, 1f);
			
			frameCanvasGroup.alpha = 1f;

			if (lastToggleInteractableState)
			{
				frameImage.color = offColor;
				if (changeTextColor)
					text.color = textNormalColor;

				if (changeRippleColor)
					rippleConfig.rippleColor = offColor;
			}

			checkImage.enabled = false;
		}

        //  Called in Update() when the 'interactable' bool on the Slider is changed
		private void EnableCheckbox()
		{
			if (toggle.isOn)
			{
				frameImage.color = onColor;
				if (changeTextColor)
					text.color = onColor;
				else
					text.color = textNormalColor;
			}
			else
			{
				frameImage.color = offColor;
				text.color = textNormalColor;
			}

			checkBoxToggler.enabled = true;
			rippleConfig.enabled = true;
		}

        //  Called in Update() when the 'interactable' bool on the Slider is changed
        private void DisableCheckbox()
		{
			frameImage.color = disabledColor;
			text.color = disabledColor;

			checkBoxToggler.enabled = false;
			rippleConfig.enabled = false;
		}

		void Update()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1) //  Animating on
			{
				if (animDeltaTime <= animationDuration)
				{
                    //  Updates values
					checkRectTransform.localScale = Anim.Overshoot(new Vector3(currentCheckSize, currentCheckSize, 1f), new Vector3(1f, 1f, 1f), animDeltaTime, animationDuration);
					frameImage.color = Anim.Quint.SoftOut(currentColor, onColor, animDeltaTime, animationDuration);
					frameCanvasGroup.alpha = Anim.Cube.SoftOut(currentFrameAlpha, 0f, animDeltaTime, animationDuration);

					if (changeTextColor)
						text.color = Anim.Quint.SoftOut(currentTextColor, onColor, animDeltaTime, animationDuration);

					if (changeRippleColor)
						rippleConfig.rippleColor = frameImage.color;
				}
				else
				{
                    //  Animation has finished

					checkRectTransform.localScale = new Vector3(1f, 1f, 1f);
					frameImage.color = onColor;
					frameCanvasGroup.alpha = 0f;

					if (changeTextColor)
						text.color = onColor;

					if (changeRippleColor)
						rippleConfig.rippleColor = onColor;

					frameImage.enabled = false;
					state = 0;
				}
			}
			else if (state == 2)    //  Animating off
			{
				if (animDeltaTime <= animationDuration * 0.75f) // * 0.75f to make it shorter than the on animation (looks better)
				{
                    //  Update values
					checkRectTransform.localScale = Anim.Sept.InOut(new Vector3(currentCheckSize, currentCheckSize, 1f), new Vector3(0f, 0f, 1f), animDeltaTime, animationDuration * 0.75f);
					frameImage.color = Anim.Sept.InOut(currentColor, offColor, animDeltaTime, animationDuration * 0.75f);
					frameCanvasGroup.alpha = Anim.Sept.InOut(currentFrameAlpha, 1f, animDeltaTime, animationDuration * 0.75f);

					if (changeTextColor)
						text.color = Anim.Quint.SoftOut(currentTextColor, textNormalColor, animDeltaTime, animationDuration * 0.75f);

					if (changeRippleColor)
						rippleConfig.rippleColor = frameImage.color;
				}
				else
				{
                    //  Animation completed
					checkRectTransform.localScale = new Vector3(0f, 0f, 1f);
					frameImage.color = offColor;
					frameCanvasGroup.alpha = 1f;
					
					if (changeTextColor)
						text.color = textNormalColor;

					if (changeRippleColor)
						rippleConfig.rippleColor = offColor;

					checkImage.enabled = false;
					state = 0;
				}
			}

            //  If the 'interactable' toggle on the Toggle has changed, call EnableCheckbox() or DisableCheckbox() accordingly
			if (lastToggleInteractableState != toggle.interactable)
			{
				lastToggleInteractableState = toggle.interactable;

				if (lastToggleInteractableState)
					EnableCheckbox();
				else
					DisableCheckbox();
			}

            //  If the Toggle is toggled, update values and appearance accordingly
			if (!Application.isPlaying)
			{
				if (lastToggleState != toggle.isOn)
				{
					lastToggleState = toggle.isOn;

					if (lastToggleState)
						TurnOnSilent();
					else
						TurnOffSilent();
				}

				if (changeRippleColor)
					rippleConfig.rippleColor = frameImage.color;
			}
		}
	}
}
