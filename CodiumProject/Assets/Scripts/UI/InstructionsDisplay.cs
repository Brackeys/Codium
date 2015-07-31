using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InstructionsDisplay : MonoBehaviour {

	private List<string> steps;
	private int nextStep = 0;

	[SerializeField]
	private RectTransform stepBulletPoint;
	[SerializeField]
	private Text goalText;

	[SerializeField]
	private Button getHelpButton;
	[SerializeField]
	private RectTransform getHelpButtonRT;
	[SerializeField]
	private Text getHelpButtonText;

	private CourseViewLoader cvLoader;

	void Awake()
	{
		if (stepBulletPoint == false)
		{
			Debug.LogError("No stepBulletPoint referenced!");
		}
		if (goalText == false)
		{
			Debug.LogError("No goalText referenced!");
		}
		if (getHelpButtonText == false)
		{
			Debug.LogError("No getHelpButtonText referenced!");
		}
		if (getHelpButtonRT == false)
		{
			Debug.LogError("No getHelpButtonRT referenced!");
		}
		if (getHelpButton == false)
		{
			Debug.LogError("No getHelpButton referenced!");
		}

		cvLoader = CourseViewLoader.ins;
		if (cvLoader == null)
		{
			Debug.LogError("No course view loader?!! PANIK");
		}
	}

	public void RegisterSteps(string[] _steps)
	{
		if (steps == null)
			steps = new List<string>();

		foreach (string _step in _steps)
		{
			if (_step == "")
				continue;
			steps.Add(_step);
		}

		if (!HasNextStep())
		{
			ChangeToShowSolutionButton();
		}
	}

	public void SetGoal(string _goal)
	{
		goalText.text = _goal;
	}

	public void HelpUser()
	{
		if (HasNextStep())
		{
			RevealNextStep();
		}
		else
		{
			ShowSolution();
		}
	}

	private void ShowSolution()
	{
		cvLoader.ShowSolution();
	}
	public void SolutionShown()
	{
		getHelpButton.interactable = false;
	}

	private void RevealNextStep()
	{
		string _step = steps[nextStep];

		RectTransform bp = Instantiate(stepBulletPoint) as RectTransform;
		bp.name = stepBulletPoint.name;
		Text bpText = bp.GetComponent<Text>();
		if (bpText == null)
		{
			Debug.LogError("No Text component on the descBulletPoint prefab.");
			return;
		}
		bpText.text = _step;
		Transform bpCount = bp.FindChild("StepCount");
		if (bpCount == null)
		{
			Debug.LogError("No child called StepCount found under " + bp.name);
			return;
		}
		Text bpCountText = bpCount.GetComponent<Text>();
		if (bpCountText == null)
		{
			Debug.LogError("No Text component on the StepCount object: " + bpCount.name);
			return;
		}
		bpCountText.text = (nextStep + 1).ToString();

		bp.transform.SetParent(transform);
		bp.localPosition = new Vector3(bp.localPosition.x, bp.localPosition.y, 0);
		bp.localScale = Vector3.one;

		getHelpButtonRT.SetAsLastSibling();

		nextStep += 1;
		if (!HasNextStep())
		{
			ChangeToShowSolutionButton();
		}
	}

	private void ChangeToShowSolutionButton()
	{
		getHelpButtonText.text = "SHOW SOLUTION";
	}

	private bool HasNextStep()
	{
		if (nextStep + 1 > steps.Count)
			return false;
		else
			return true;
	}

}
