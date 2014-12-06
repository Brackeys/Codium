//-----------------------------------------------------------------
// This ScriptableObject defines the class 'Course' that will store information
// about the different courses. It is meant to be used in conjunction with
// 'CourseEditor' to create Custom Data Assets in Unity, each representing a course
// that can be loaded into Codium at launch or maybe through some kind of import process.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//	The Course class stores ALL information about the course INCLUDING a list of CourseViews
[System.Serializable]
public class Course : ScriptableObject {

	// Enum for the course category
	public enum Category {
		LevelDesign,
		Math,
		Language
	}

	// Enum for course difficulty
	public enum Difficulty {
		Beginner,
		Intermediate,
		Advanced
	}

	// Some general information about the course
	public string title;
	public string author;
	public string description;
	public Category category;
	public Difficulty difficulty;
	public List<CourseView> courseViews;

	// Class constructor
	public Course () {
		Reset ();
	}

	// Init default values
	// Reset is called upon initialization and when the 'Reset to Default' button is pressed.
	public void Reset () {
		title = "Intro to Game Programming";
		author = "Asbjørn Thirslund";
		description = "This is a course description.\nThe author can put a short explaination here to let the user know what the course is about.";
		category = Category.Language;
		difficulty = Difficulty.Intermediate;
		courseViews = new List<CourseView>();
	}
}

//	The CourseView class stores information about a single view in the course.
//	A CourseView is basically a slide which covers a certain part of the course subject.
//	A Course should consist of multiple CourseViews all keeping the Course subject and goal in mind.
[System.Serializable]
public class CourseView {
	public string subject;
	public string explaination;
	public string[] codeBulletPoints;
	public string[] exampleBulletPoints;
	public string[] instructionBulletPoints;
	public string defaultCode;
	public string solutionCode;
	public string hint;

	// Class constructor
	public CourseView () {
		subject = "GameObjects";
		explaination = "Everything you can see in the game is called a GameObject." +
						"\nGameObjects have different properties which can be manipulated through code.";

		codeBulletPoints = new string[3] {"To access properties in a GameObject we use the '<b>.</b>'' operator.",
											"Sometimes you have to use the '<b>.</b>'' multiple times to access the property you want.",
											""};

		exampleBulletPoints = new string[3] {"Cube.transform.position accesses the position of our Cube.",
												"Cube.renderer.material accesses the way the object looks.",
												""};

		instructionBulletPoints = new string[3] {"Access the Cubes color property under Cube.renderer.material.",
													"Set the color property equal to Color.red.",
													"(Optional) Try the same with blue, green and yellow."};

		defaultCode = "GameObject Cube = GameObject.Find(INSERT QUOTES);\t//Find the Cube GameObject & name it 'Cube'\n\n";
		solutionCode = "GameObject Cube = GameObject.Find(INSERT QUOTES);\t//Find the Cube GameObject & name it 'Cube'\n\n" + 
									"Cube.renderer.material.color = Color.red;";

		hint = "Remember to get put ';' at the end of the line.";
	}
}
