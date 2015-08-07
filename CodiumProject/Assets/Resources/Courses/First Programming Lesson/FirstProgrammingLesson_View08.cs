using UnityEngine;
using CodeEnvironment;
using GameView;
using System.Linq;

public class FirstProgrammingLesson_View08 : CEValidator {

	public override bool Validate()
	{

		Log[] _logs = console.GetLogs(4);

		if (_logs == null)
			return false;
		if (_logs.Length < 4)
			return false;

		bool intCorrect = false;
		bool floatCorrect = false;
		bool stringCorrect = false;
		bool boolCorrect = false;
		for (int i = 0; i < _logs.Length; i++)
		{
			string _msg = _logs[i].message.ToLower();

			if (!intCorrect)
			{
				if (_msg.Contains("integer") && _msg.Any(char.IsDigit)) {
					intCorrect = true;
					continue;
				}
			}
			if (!floatCorrect)
			{
				if (_msg.Contains("float") && _msg.Any(char.IsDigit) && _msg.Contains(".")) {
					floatCorrect = true;
					continue;
				}
			}
			if (!stringCorrect)
			{
				if (_msg.Contains("string") && _msg.Contains("hello")) {
					stringCorrect = true;
					continue;
				}
			}
			if (!boolCorrect)
			{
				if (_msg.Contains("boolean") && (_msg.Contains("false") || _msg.Contains("true"))) {
					boolCorrect = true;
					continue;
				}
			}
		}

		if (intCorrect && floatCorrect && boolCorrect && stringCorrect) {
			return true;
		} else {
			return false;
		}
		
	}

}
