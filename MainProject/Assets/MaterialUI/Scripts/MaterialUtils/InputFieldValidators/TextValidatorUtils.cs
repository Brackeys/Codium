//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace MaterialUI
{
	public class BaseTextValidator
	{
		protected MaterialInputField m_MaterialInputField;

		public void Init(MaterialInputField materialInputField)
		{
			m_MaterialInputField = materialInputField;
		}
	}

	public class EmptyTextValidator : BaseTextValidator, ITextValidator
	{
		public bool IsTextValid()
		{
			if (string.IsNullOrEmpty(m_MaterialInputField.inputField.text))
			{
				m_MaterialInputField.validationText.text = "Can't be empty";
				return false;
			}

			return true;
		}
	}

	public class EmailTextValidator : BaseTextValidator, ITextValidator
	{
		public bool IsTextValid()
		{
			Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
			Match match = regex.Match(m_MaterialInputField.inputField.text);
			if (!match.Success)
			{
				m_MaterialInputField.validationText.text = "Format is invalid";
				return false;
			}

			return true;
		}
	}

	public class NameTextValidator : BaseTextValidator, ITextValidator
	{
		private int m_MinimumLength = 3;

		public NameTextValidator() { }
		public NameTextValidator(int minimumLength)
		{
			m_MinimumLength = minimumLength;
		}

		public bool IsTextValid()
		{
			if (m_MaterialInputField.inputField.text.Length < m_MinimumLength)
			{
				m_MaterialInputField.validationText.text = "Format is invalid";
				return false;
			}

			Regex regex = new Regex(@"^\p{L}+(\s+\p{L}+)*$");
			Match match = regex.Match(m_MaterialInputField.inputField.text);
			if (!match.Success)
			{
				m_MaterialInputField.validationText.text = "Format is invalid";
				return false;
			}

			return true;
		}
	}

	public class PasswordTextValidator : BaseTextValidator, ITextValidator
	{
		private int m_MinimumLength = 6;

		public PasswordTextValidator() { }
		public PasswordTextValidator(int minimumLength)
		{
			m_MinimumLength = minimumLength;
		}

		public bool IsTextValid()
		{
			if (m_MaterialInputField.inputField.text.Length < m_MinimumLength)
			{
				m_MaterialInputField.validationText.text = "Require at least " + m_MinimumLength + " characters";
				return false;
			}

			return true;
		}
	}

	public class SamePasswordTextValidator : BaseTextValidator, ITextValidator
	{
		private InputField m_OriginalPasswordInputField;

		public SamePasswordTextValidator() { }
		public SamePasswordTextValidator(InputField originalPasswordInputField)
		{
			m_OriginalPasswordInputField = originalPasswordInputField;
		}

		public bool IsTextValid()
		{
			if (!m_MaterialInputField.inputField.text.Equals(m_OriginalPasswordInputField.text))
			{
				m_MaterialInputField.validationText.text = "Passwords are different!";
				return false;
			}

			return true;
		}
	}
}