using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MaterialUIScale : MonoBehaviour
{
	float resolutionProduct;
	float baseResProduct;

	public enum FormFactor{Desktop, Mobile};
	public FormFactor formFactor;

	CanvasScaler scaler;

	[HideInInspector()]
	public float scaleFactor = 1f;

	public delegate void CalculateScaleEvent(float scaleFactor);
	public static event CalculateScaleEvent RefreshScale;
	
	void Start ()
	{
		if (formFactor == FormFactor.Mobile)
		{
			scaler = gameObject.GetComponent<CanvasScaler> ();

			scaleFactor = scaler.scaleFactor;

			resolutionProduct = Screen.width * Screen.height;
			baseResProduct = resolutionProduct / (1280f * 720f);

			scaleFactor *= baseResProduct;

			if (scaleFactor >= 4f)
				scaleFactor = 4f;
			else if (scaleFactor >= 3.5f)
				scaleFactor = 3.5f;
			else if (scaleFactor >= 3f)
				scaleFactor = 3f;
			else if (scaleFactor >= 2.5f)
				scaleFactor = 2.5f;
			else if (scaleFactor >= 2f)
				scaleFactor = 2f;
			else if (scaleFactor >= 1.5f)
				scaleFactor = 1.5f;
			else if (scaleFactor >= 1f)
				scaleFactor = 1f;
			else if (scaleFactor >= 0.5f)
				scaleFactor = 0.5f;
			else
				scaleFactor = 0.25f;

			scaler.scaleFactor = scaleFactor;

			if (RefreshScale != null)
				RefreshScale(scaleFactor);
		}
		else
		{
			scaleFactor = gameObject.GetComponent<CanvasScaler> ().scaleFactor;

			if (RefreshScale != null)
				RefreshScale(scaleFactor);
		}
	}
}
