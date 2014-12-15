//-----------------------------------------------------------------
// Hosts utility methods for handling / loading course assets at runtime.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

public class CourseUtil {


	// ------- COURSE LOADING -------

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


	// ------- COURSE SORTING -------


	// Sort course list by course title
	public static List<Course> SortCoursesByTitle (List<Course> courses) {
		courses.Sort ( (x, y) => string.Compare (x.title, y.title) );
		return courses;
	}
	
	// Sort course list by course category
	public static List<Course> SortCoursesByCategory (List<Course> courses) {
		courses.Sort ( (x, y) => string.Compare (x.category.ToString(), y.category.ToString()) );
		return courses;
	}
	
	// Sort course list by first category, then title
	public static List<Course> SortCourses (List<Course> courses) {
		Debug.Log ("TODO: Get this working.");
		
		List <Course> sortedList = new List<Course>();
		
		sortedList = SortCoursesByCategory (courses);
		
		for (int i = 0; i < courses.Count; i++) {
			Debug.Log (sortedList [i]);
		}
		
		return sortedList;
	}
}
