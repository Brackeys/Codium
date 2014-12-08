using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class ToastControl
{
	static GameObject theToast;
	public static string toastText;
	public static float toastDuration;
	public static Color toastPanelColor;
	public static Color toastTextColor;
	public static int toastFontSize;
	public static Canvas parentCanvas;

	public static void InitToastSystem (Canvas theCanvas)
	{
		theToast = Resources.Load ("MaterialUI/Toast", typeof(GameObject)) as GameObject;
		parentCanvas = theCanvas;
	}

	public static void MakeToast (string content, float duration, Color panelColor, Color textColor, int fontSize)
	{
		toastText = content;
		toastDuration = duration;
		toastPanelColor = panelColor;
		toastTextColor = textColor;
		toastFontSize = fontSize;
		GameObject.Instantiate (theToast);
	}
}
