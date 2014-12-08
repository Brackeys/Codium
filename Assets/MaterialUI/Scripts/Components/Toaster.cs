using UnityEngine;
using System.Collections;

public class Toaster : MonoBehaviour
{
	public string text = "This is a toast";
	public float duration = 1.5f;
	public Color panelColor = new Color (1f, 1f, 1f);
	public Color textColor = new Color (0.15f, 0.15f, 0.15f);
	public int fontSize = 16;

	void Start ()
	{
		ToastControl.InitToastSystem (gameObject.GetComponentInParent<Canvas>());
	}

	public void PopupToast ()
	{
		ToastControl.MakeToast (text, duration, panelColor, textColor, fontSize);
	}
}
