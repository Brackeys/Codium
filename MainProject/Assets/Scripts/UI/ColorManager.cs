using UnityEngine;

namespace Codium.UI
{
	public class ColorManager : MonoBehaviour
	{

		[SerializeField]
		CodiumColorPalette m_colorPalette;

		private static CodiumColorPalette ColorPalette;

		void Awake ()
		{
			ColorPalette = m_colorPalette;
		}

		public static Color GetColor(string name)
		{
			return ColorPalette.GetColor(name);
		}

	}
}
