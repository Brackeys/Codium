using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View04 : CEValidator {

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		string msg = _latestLog.message;
		if (msg == null)
			return false;

		msg = msg.ToLower();

		if (codeManager.GetUserCode().Contains("+") == false)
			return false;

		switch (msg)
		{
			case "hello, world":
				return true;
			case "hello, world!":
				return true;
			case "hello world":
				return true;
			case "hello world!":
				return true;
			default:
				return false;
		}
	}

}
