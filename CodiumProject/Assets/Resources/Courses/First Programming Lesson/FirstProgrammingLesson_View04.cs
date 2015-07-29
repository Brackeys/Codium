using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View04 : CEValidator {

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		string msg = _latestLog.message;
		msg = msg.ToLower();

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
		}

		return false;
	}

}
