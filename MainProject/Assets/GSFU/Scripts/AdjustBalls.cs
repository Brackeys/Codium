using UnityEngine;
using System.Collections;
using LitJson;

public struct OptionalMiddleStruct
{
	public string name;
	public Color color;
	public float drag;
}

public class AdjustBalls : MonoBehaviour
{

	public void DoSomethingWithTheData(JsonData[] ssObjects)
	{
		OptionalMiddleStruct container = new OptionalMiddleStruct();
		
		for (int i = 0; i < ssObjects.Length; i++) 
		{	
			if (ssObjects[i].Keys.Contains("name"))
				container.name = ssObjects[i]["name"].ToString();

			if (ssObjects[i].Keys.Contains("color"))
				container.color = GetColor(ssObjects[i]["color"].ToString());

			if (ssObjects[i].Keys.Contains("drag"))
				container.drag = float.Parse(ssObjects[i]["drag"].ToString());

			UpdateObjectValues(container);
		}	
	}

	void UpdateObjectValues(OptionalMiddleStruct container)
	{
		GameObject ball = GameObject.Find(container.name);
		
		ball.GetComponent<Renderer>().sharedMaterial.color = container.color;
		ball.GetComponent<Rigidbody>().drag = container.drag;
	}	
	
	Color GetColor(string color)
	{
		Color c;

		switch (color)
		{
		case "black":
			c = Color.black;
			break;
		case "blue":
			c = Color.blue;
			break;
		case "clear":
			c = Color.clear;
			break;
		case "cyan":
			c = Color.cyan;
			break;
		case "gray":
			c = Color.gray;
			break;
		case "green":
			c = Color.green;
			break;
		case "grey":
			c = Color.grey;
			break;
		case "magenta":
			c = Color.magenta;
			break;
		case "red":
			c = Color.red;
			break;
		case "white":
			c = Color.white;
			break;
		case "yellow":
			c = Color.yellow;
			break;
		default:
			c = Color.grey;
			break;
		}

		return c;
	}

	public void ResetBalls()
	{
		OptionalMiddleStruct container = new OptionalMiddleStruct();

		container.color = Color.white;
		container.drag = 0f;

		string nameBase = "Ball";
		for (int i = 1; i < 4; i++)
		{
			container.name = nameBase + i.ToString();
			UpdateObjectValues(container);
		}
	}
}

