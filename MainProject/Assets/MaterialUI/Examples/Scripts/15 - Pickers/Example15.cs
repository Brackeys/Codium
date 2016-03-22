//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using MaterialUI;

public class Example15 : MonoBehaviour
{
	public void OnTimePickerButtonClicked()
	{
		DialogManager.ShowTimePicker(Random.Range(0, 12), Random.Range(0, 60), Random.value > 0.5f, (int hour, int minute, bool isAM) =>
		{
			ToastManager.Show(hour + ":" + minute.ToString("00") + " " + (isAM ? "AM" : "PM"));
		}, MaterialColor.Random500());
	}

	public void OnDatePickerButtonClicked()
	{
		DialogManager.ShowDatePicker(Random.Range(1980, 2050), Random.Range(1, 12), Random.Range(1, 30), (System.DateTime date) =>
		{
			ToastManager.Show(date.ToString("dd MMM, yyyy"));
		}, MaterialColor.Random500());
	}
}
