using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShadowsControl : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
	[HideInInspector()]
	public ShadowAnim[] shadows;
	[HideInInspector()]
	public int shadowNormalSize;
	[HideInInspector()]
	public int shadowHoverSize;

	[HideInInspector()]
	public bool isEnabled = true;
	
	public void OnPointerDown (PointerEventData data)
	{
		SetShadows(shadowHoverSize);
	}
	
	public void OnPointerEnter (PointerEventData data)
	{
		SetShadows(shadowHoverSize);
	}

	public void OnPointerExit (PointerEventData data)
	{
		SetShadows(shadowNormalSize);
	}

	public void SetShadows (int shadowOn)
	{
		if (isEnabled)
		{
			foreach (ShadowAnim shadow in shadows)
			{
				shadow.SetShadow(false);
			}
			
			if (shadowOn - 1 >= 0)
			{
				shadows [shadowOn - 1].SetShadow (true);
			}
		}
	}
}
