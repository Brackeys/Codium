using UnityEngine;
using System.Collections;

public class CourseLoader : MonoBehaviour {

	public Course course;
	public new string name;

	void Awake () {
		name = course.name;
	}
}
