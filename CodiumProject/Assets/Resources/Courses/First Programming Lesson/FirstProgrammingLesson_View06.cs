using UnityEngine;
using CodeEnvironment;
using GameView;
using System.Linq;

public class FirstProgrammingLesson_View06 : CEValidator {

	public override bool Validate()
	{
		Log[] _latestLogs = console.GetLogs(2);
		string _code = codeManager.GetUserCode();

		if (!_code.Contains("Console.Print"))
			return false;

		if (_latestLogs == null)
			return false;

		if (_latestLogs[1].message.Any(c => char.IsDigit(c)))
			return false;

		int n;
		bool isNumeric = int.TryParse(_latestLogs[0].message, out n);
		if (!isNumeric)
			return false;

		Debug.Log("TRUE");
		return false;
	}

}
