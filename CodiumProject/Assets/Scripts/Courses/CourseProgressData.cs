//-----------------------------------------------------------------
// This serializable class hosts relevant information about the progress of a course
//-----------------------------------------------------------------

using System.Collections.Generic;

[System.Serializable]
public class CourseProgressData
{

	public string courseID;			// Unique ID to identify the course

	public List<CourseViewStateData> cvStateDataList;	// List of course view states

	public string curCourseViewID;		// The course view the user last visited

	public CourseProgressData(string _ID, string _curCVID)
	{
		courseID = _ID;
		cvStateDataList = new List<CourseViewStateData>();
		curCourseViewID = _curCVID;
	}

	public void SetStateDataList(List<CourseViewStateData> _stateDataList)
	{
		cvStateDataList = _stateDataList;
	}

	public List<CourseViewStateData> GetStateDataList()
	{
		return cvStateDataList;
	}

	public void AddStateData(CourseViewStateData _sData)
	{
		cvStateDataList.Add(_sData);
	}

	public void RemoveStateDataAt(int _index)
	{
		cvStateDataList.RemoveAt(_index);
	}

	public void SetStateData(CourseViewStateData _sData, int _index)
	{
		cvStateDataList[_index] = _sData;
	}

	public CourseViewStateData GetStateDataByID(string _ID)
	{
		for (int i = 0; i < cvStateDataList.Count; i++)
		{
			if (cvStateDataList[i].ID == _ID)
			{
				return cvStateDataList[i];
			}
		}

		return null;
	}

	public int GetStateDataCount()
	{
		return cvStateDataList.Count;
	}

	public string GetCurrentCourseViewID()
	{
		return curCourseViewID;
	}

	public void SetCurrentCourseViewID(string _ID)
	{
		curCourseViewID = _ID;
	}

	public float GetCourseProgressPercent()
	{
		int _count = cvStateDataList.Count;
		if (_count == 0) {
			return 0f;
		}

		int _completed = 0;
		for (int i = 0; i < _count; i++)
		{
			if (cvStateDataList[i].isCompleted)
			{
				_completed++;
			}
		}

		return (float)_completed / _count * 100f;
	}
}

public class CourseProgressHelper
{
	public static string GetFileName(string _ID)
	{
		return "courseProgress_" + _ID;
	}
}
