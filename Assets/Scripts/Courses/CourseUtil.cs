//-----------------------------------------------------------------
// Hosts utility methods for handling / loading course assets at runtime.
//-----------------------------------------------------------------

using UnityEngine;

public class CourseUtil {

	public static Course LoadCourse (string name) {
		Course course = Resources.Load<Course>("Courses" + name);

		if (course == null) {
			Debug.LogError ("Course with name of "+ name +
			                " doesn't exist. Returning null.");
		}

		return course;
	}

	public static Course[] LoadAllCourses () {
		Course[] courses = Resources.LoadAll<Course>("Courses");
		if (courses.Length == 0) {
			Debug.LogWarning ("No courses found. Returning empty Array.");
		}

		return courses;
	}
}
