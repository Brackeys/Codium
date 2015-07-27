//-----------------------------------------------------------------
// This ScriptableObject defines the class 'Course' that will store information
// about the different courses. It is meant to be used in conjunction with
// 'CourseEditor' to create Custom Data Assets in Unity, each representing a course
// that can be loaded into Codium at launch or maybe through some kind of import process.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	public string ID;

	public int LPValue
	{
		get { return courseViews.Count * NumberMaster.courseViewLPValue; }
	}
	
	// Init default values
	// Reset is called upon initialization and when the 'Reset to Default' button is pressed.
	public void Reset () {
		title = "Intro to Game Programming";
		author = "Asbjørn Thirslund";
		description = "This course teaches the fundamentals of programming games. It demonstrates techniques on setting up basic game logic.";
		category = Category.LevelDesign;
		difficulty = Difficulty.Beginner;
		courseViews = new List<CourseView>();

		Debug.Log ("Course values reset.");
	}
	public void ResetID()
	{
		ID = GenerateID();

		Debug.Log("ID reset.");
	}

	public string GenerateID()
	{
		return Guid.NewGuid().ToString("N");
	}

	public float GetCompletionPercent(int _index)
	{
		if (_index == 0)
			return 0f;
		int _count = courseViews.Count;
		return (float)_index / _count * 100f;
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
	public string goal;
	public string[] instructionBulletPoints;
	public string defaultCode;
	public string solutionCode;
	public string hint;

	public CodeEnvironment.CESettings ceSettings;

	public UnityEngine.Object gameScene;

	// Class constructor
	public CourseView () {
		subject = "GameObjects";
		explaination = "Everything you can see in the game is called a GameObject." +
						"\n\nGameObjects have different properties which can be manipulated through code.";

		codeBulletPoints = new string[3] {"To access properties in a GameObject we use the '<b>.</b>'' operator.",
											"Sometimes you have to use the '<b>.</b>'' multiple times to access the property you want.",
											""};

		exampleBulletPoints = new string[3] {"Cube.transform.position accesses the position of our Cube.",
												"Cube.renderer.material accesses the way the object looks.",
												""};
		goal = "Change the color of the Cube to your liking.";
		instructionBulletPoints = new string[8] {"Access the Cubes color property under Cube.renderer.material.",
													"Set the color property equal to Color.red.",
													"(Optional) Try the same with blue, green and yellow.",
													"", "", "", "", ""};

		defaultCode = "GameObject Cube = GameObject.Find(\"Cube\");\t//Find the Cube GameObject & name it 'Cube'\n\n";
		solutionCode = "GameObject Cube = GameObject.Find(\"Cube\");\t//Find the Cube GameObject & name it 'Cube'\n\n" + 
									"Cube.renderer.material.color = Color.red;";

		hint = "Remember to put ';' at the end of the line.";

		ceSettings = new CodeEnvironment.CESettings();
	}
}
