//-----------------------------------------------------------------
// This ScriptableObject defines the class 'Course' that will store information
// about the different courses. It is meant to be used in conjunction with
// 'CourseEditor' to create Custom Data Assets in Unity, each representing a course
// that can be loaded into Codium at launch or maybe through some kind of import process.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CourseView {
	public string subject = "GameObjects";
	public string explaination = "Everything you can see in the game is called a GameObject. \nGameObjects have different properties which can be manipulated through code.";
	public string[] codeBulletPoints = new string[3] {"To access properties in a GameObject we use the '<b>.</b>'' operator.",
										"Sometimes you have to use the '<b>.</b>'' multiple times to access the property you want.",
										""};
	public string[] exampleBulletPoints = new string[3] {"Cube.transform.position accesses the position of our Cube.",
											"Cube.renderer.material accesses the way the object looks.",
											""};
	public string[] instructionBulletPoints = new string[3] {"Access the Cubes color property under Cube.renderer.material.",
												"Set the color property equal to Color.red.",
												"(Optional) Try the same with blue, green and yellow."};
	public string defaultCode = "GameObject Cube = GameObject.Find(INSERT QUOTES);\t//Find the Cube GameObject & name it 'Cube'\n\n";
	public string solutionCode = "GameObject Cube = GameObject.Find(INSERT QUOTES);\t//Find the Cube GameObject & name it 'Cube'\n\n" + 
								"Cube.renderer.material.color = Color.red;";
	public string hint = "Remember to get put ';' at the end of the line.";
}

[System.Serializable]
public class Course : ScriptableObject {

	public enum Category {
		LevelDesign,
		Math,
		Language
	}

	public enum Difficulty {
		Beginner,
		Intermediate,
		Advanced
	}

	public string title;
	public string author;
	public string description;
	public Category category;
	public Difficulty difficulty;
	public List<CourseView> courseViews;

	public void InitSettings () {
		title = "Intro to Game Programming";
		author = "Asbjørn Thirslund";
		description = "This is a course description.\nThe author can put a short explaination here to let the user know what the course is about.";
		category = Category.Language;
		difficulty = Difficulty.Intermediate;
		courseViews = new List<CourseView>();
	}
}
