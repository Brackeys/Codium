using UnityEngine;
using CodeEnvironment;
using GameView;

public class CSharpForBeginners_View03 : CEValidator {

	public override bool Validate()
	{
		Log _latestLog = console.GetLatestLog();
		int n;
		bool isNumeric = int.TryParse(_latestLog.message, out n);
		if (n >= 0 && isNumeric)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
