using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RadioGroupConfig : MonoBehaviour
{
	public bool inkBlotEnabled = true;
	public bool autoInkBlotSize = true;
	public int inkBlotSize;
	public float inkBlotSpeed = 8f;
	public Color inkBlotColor = Color.black;
	public float inkBlotStartAlpha = 0.5f;
	public float inkBlotEndAlpha = 0.3f;
	public Color radioOnColor = Color.black;

	void Start ()
	{
		foreach (RadioConfig config in gameObject.GetComponentsInChildren<RadioConfig> ())
		{
			config.inkBlotEnabled = inkBlotEnabled;
			config.autoInkBlotSize =  autoInkBlotSize;
			config.inkBlotSize =  inkBlotSize;
			config.inkBlotSpeed =  inkBlotSpeed;
			config.inkBlotColor =  inkBlotColor;
			config.inkBlotStartAlpha =  inkBlotStartAlpha;
			config.inkBlotEndAlpha =  inkBlotEndAlpha;
			config.radioOnColor = radioOnColor;

			config.Setup ();
		}
	}
}
