using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	public float updateInterval = 0.5f;

	float accum = 0f; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval

	public Text theText;
	
	void Start()
	{
		timeleft = updateInterval;  
	}
	
	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0f )
		{
			// display two fractional digits (f2 format)
			theText.text = "" + (accum/frames).ToString("f2") + " FPS";
			if ((accum/frames) < 1)
			{
				guiText.text = "";
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
