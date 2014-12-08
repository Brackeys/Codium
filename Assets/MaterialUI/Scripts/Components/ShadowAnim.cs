using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShadowAnim : MonoBehaviour {

	public bool isOn;
	public bool anim;
	CanvasGroup thisGroup;
	Image[] shadows;

	void Awake ()
	{
		thisGroup = gameObject.GetComponent<CanvasGroup> ();
		shadows = gameObject.GetComponentsInChildren<Image> ();
	}
	
	void Update ()
	{
		if (anim)
		{
			if (isOn)
			{
				if (thisGroup.alpha < 1f)
				{
					thisGroup.alpha = Mathf.Lerp(thisGroup.alpha, 1.1f, Time.deltaTime * 6);
				}
				else
				{
					thisGroup.alpha = 1f;
					anim = false;
				}
			}
			else
			{
				if (thisGroup.alpha > 0f)
				{
					thisGroup.alpha = Mathf.Lerp(thisGroup.alpha, -0.1f, Time.deltaTime * 6);
				}
				else
				{
					thisGroup.alpha = 0f;
					anim = false;
					foreach (Image shadow in shadows)
						shadow.enabled = false;
				}
			}
		}
	}

	public void SetShadow (bool set)
	{
		isOn = set;
		anim = true;
		foreach (Image shadow in shadows)
			shadow.enabled = true;
	}
}
