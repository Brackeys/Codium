using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View01 : CEValidator
{

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		int n;
		bool isNumeric = int.TryParse(_latestLog.message, out n);
		if (isNumeric)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
