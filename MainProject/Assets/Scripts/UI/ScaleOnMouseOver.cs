using UnityEngine;
using UnityEngine.EventSystems;
using MaterialUI;

namespace Codium.UI
{
	public class ScaleOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private float m_scaleFactor = 1.1f;
		
		[SerializeField]
		private float m_speed = 4f;
		
		public void OnPointerExit (PointerEventData eventData) 
		{
			Vector3 targetSize = Vector3.one;
        	TweenManager.TweenVector3(size => transform.localScale = size, transform.localScale, targetSize, 1f/m_speed);
		}
		
		public void OnPointerEnter (PointerEventData eventData) 
		{
			//Color targetColor = isRed ? MaterialColor.red500 : MaterialColor.blue500;
			Vector3 targetSize = Vector3.one * m_scaleFactor;
        	TweenManager.TweenVector3(size => transform.localScale = size, transform.localScale, targetSize, 1f/m_speed);
		}
	}
}
