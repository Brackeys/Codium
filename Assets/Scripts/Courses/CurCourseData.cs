//-----------------------------------------------------------------
// This serializable class hosts relevant information about the current course view
//-----------------------------------------------------------------

[System.Serializable]
public class CurCourseData
{

	public string ID;
	public int curCVIndex;

	public string fileName;

	public CurCourseData(string _ID, int _curCVIndex)
	{
		ID = _ID;
		curCVIndex = _curCVIndex;

		fileName = "curCourse";
	}

	public CurCourseData()
	{
		fileName = "curCourse";
	}

}
