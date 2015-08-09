//-----------------------------------------------------------------
// This serializable class hosts relevant information about the state of a course view
//-----------------------------------------------------------------

[System.Serializable]
public class CourseViewStateData
{
	public string ID;

	public bool isCompleted;

	public CourseViewStateData(string _ID, bool _isCompleted)
	{
		ID = _ID;
		isCompleted = _isCompleted;
	}

	public CourseViewStateData(string _ID)
	{
		ID = _ID;
	}

}
