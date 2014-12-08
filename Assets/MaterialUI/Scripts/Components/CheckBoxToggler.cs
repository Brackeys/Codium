using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckBoxToggler : MonoBehaviour, IPointerClickHandler
{
	public Toggle theToggle;

	public void OnPointerClick (PointerEventData data)
	{
		theToggle.isOn = !theToggle.isOn;
	}
}
