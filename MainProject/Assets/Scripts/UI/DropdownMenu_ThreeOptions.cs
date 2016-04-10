using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Codium.UI
{
	public class DropdownMenu_ThreeOptions : MonoBehaviour
	{

		private bool isToggled = false;

		[SerializeField]
		private TextMeshProUGUI m_optionOne;
		[SerializeField]
		private TextMeshProUGUI m_optionTwo;
		[SerializeField]
		private TextMeshProUGUI m_optionThree;

		[SerializeField]
		private GameObject m_dropdown;

		[SerializeField]
		private Text m_selectedText;

		[System.Serializable]
		private class OnValueChanged : UnityEvent<int> { }
		[SerializeField]
		private OnValueChanged m_onValueChanged;

		void Awake ()
		{
			m_selectedText.text = "";
		}

		public void SetOptions  (string optionOne, string optionTwo, string optionThree)
		{
			m_optionOne.text = optionOne;
			m_optionTwo.text = optionTwo;
			m_optionThree.text = optionThree;

			m_selectedText.text = "";
		}

		public void SetSelected (int selection)
		{
			switch (selection)
			{
				case 1:
					m_selectedText.text = m_optionOne.text;
					break;
				case 2:
					m_selectedText.text = m_optionTwo.text;
					break;
				case 3:
					m_selectedText.text = m_optionThree.text;
					break;
				default:
					Debug.LogError("What? Can't set selection to: " + selection);
					break;
			}

			m_onValueChanged.Invoke(selection - 1);

			Close();
		}

		public void Close ()
		{
			isToggled = false;

			m_dropdown.SetActive(false);
		}

		void Open()
		{
			isToggled = true;

			m_dropdown.SetActive(true);
		}

		public void Toggle ()
		{
			if (isToggled)
				Close();
			else
				Open();
		}

	}

}
