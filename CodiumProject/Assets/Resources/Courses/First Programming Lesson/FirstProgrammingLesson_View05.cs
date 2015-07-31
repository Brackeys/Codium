using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View05 : CEValidator {

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		Debug.Log("TODO: Get access to user code. Check for //");
		if (_latestLog.message == ("I am your father".Length + 2).ToString())
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
