//-----------------------------------------------------------------
// This serializable class hosts relevant information about the progress of a course
//-----------------------------------------------------------------

[System.Serializable]
public class CourseCompletionData
{

	public string ID;			// Unique ID to identify the course
	public int nextCVIndex;		// The index of the next course view to complete
	public int currentCVIndex;	// The index of the current course view

	public string fileName;

	public CourseCompletionData(string _ID, int _cvIndex)
	{
		ID = _ID;
		Init(_cvIndex);

		fileName = "courseCompletion_" + _ID;
	}

	public CourseCompletionData(string _ID)
	{
		fileName = "courseCompletion_" + _ID;
	}

	public void Init(int _cvIndex)
	{
		if (_cvIndex > nextCVIndex)
		{
			nextCVIndex = _cvIndex;
		}
		currentCVIndex = _cvIndex;
	}

}
