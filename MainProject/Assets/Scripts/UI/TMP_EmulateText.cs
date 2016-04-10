using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode()]
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TMP_EmulateText : MonoBehaviour
	{

		public Text text;

		private TextMeshProUGUI m_tmpText;

		void Update()
		{
			if (text == null)
				return;

			if (m_tmpText == null)
			{
				m_tmpText = GetComponent<TextMeshProUGUI>();
				return;
			}

			m_tmpText.text = text.text;
			m_tmpText.fontSize = text.fontSize;
			m_tmpText.color = new Color (text.color.r, text.color.g, text.color.b);

		}

	}

}
