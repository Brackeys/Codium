using UnityEngine;
using CodeEnvironment;
using GameView;

public class FirstProgrammingLesson_View02 : CEValidator
{

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		float n;
		bool isNumeric = float.TryParse(_latestLog.message, out n);
		if (isNumeric && (n % 1) != 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
