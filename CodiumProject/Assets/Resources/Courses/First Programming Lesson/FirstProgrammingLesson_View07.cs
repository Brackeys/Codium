using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View07 : CEValidator {

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		string _code = codeManager.GetUserCode();
		if (_latestLog.message == ("I am your father".Length + 2).ToString() && _code.Contains("//"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
