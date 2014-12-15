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
	
	// Sort course list by category first, then title
	public static List<Course> SortCourses (List<Course> courses) {

		courses = SortCoursesByCategory (courses);	// Sort by category

		// Final list to return
		List <Course> sortedList = new List<Course>();

		// Temp list for storing courses in same category
		List <Course> coursesInCategory = new List<Course>();
		Course.Category lastCourse = new Course.Category ();

		for (int i = 0; i < courses.Count; i++) {

			if (i == 0) {	// On first iteration
				coursesInCategory.Add ( courses[i]);	// Add course to temp list
				lastCourse = courses[i].category;		// Store this iteration's category
				continue;
			}

			if (lastCourse != courses[i].category) {	// The course category hasn't changed
				// Sort category and add to final list, then clear the temp list
				sortedList.AddRange ( SortCoursesByTitle (coursesInCategory) );
				coursesInCategory.Clear();
			}

			coursesInCategory.Add ( courses[i]);	// Add course to temp list
			lastCourse = courses[i].category;		// Store this iteration's category
		}

		// Add the remainder of our list
		sortedList.AddRange ( SortCoursesByTitle (coursesInCategory) );
		
		return sortedList;
	}
}
