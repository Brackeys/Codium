//-----------------------------------------------------------------
// This serializable class hosts relevant information about the current course view
//-----------------------------------------------------------------

[System.Serializable]
public class CurCourseData
{

	public string ID;

	public string fileName;

	public CurCourseData(string _ID)
	{
		ID = _ID;

		fileName = "curCourse";
	}

	public CurCourseData()
	{
		fileName = "curCourse";
	}

}
