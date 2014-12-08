using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleConfig : MonoBehaviour, IPointerUpHandler
{
	Slider theSlider;
	Toggle theToggle;

	void Start ()
	{
		theSlider = gameObject.GetComponentInChildren<Slider> ();
		theToggle = gameObject.GetComponent<Toggle> ();
	}

	public void OnPointerUp (PointerEventData data)
	{
		if (theSlider.value > 0.5f)
		{
			theToggle.isOn = true;
		}
		else
		{
			theToggle.isOn = false;
		}
	}
}
