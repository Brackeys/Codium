//-----------------------------------------------------------------
// Hosts utility methods for handling / loading course assets at runtime.
//-----------------------------------------------------------------

using UnityEngine;

public class CourseUtil {

	public static Course LoadCourse (string name) {
		Course course = Resources.LoadAssetAtPath<Course>("Assets/Courses/"
		                                                 + name + ".asset");

		if (course == null) {
			Debug.LogError ("Course with name of "+ name +
			                " doesn't exist. Returning null.");
			return null;
		}

		return course;
	}
}
