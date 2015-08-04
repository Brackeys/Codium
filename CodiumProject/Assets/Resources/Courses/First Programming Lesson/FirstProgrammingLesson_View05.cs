using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View05 : CEValidator {

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		string _code = codeManager.GetUserCode();
		if (_latestLog.message == "False" && _code.Contains ("42"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
