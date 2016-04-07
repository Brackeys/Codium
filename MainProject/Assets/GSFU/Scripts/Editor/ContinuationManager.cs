using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

/* kjems - http://answers.unity3d.com/questions/221651/yielding-with-www-in-editor.html

I made a ContinuationManager to handle the cases where I want to wait for a condition and then do something with an object.
	
The snippet below is an example of WWW using the ContinuationManager where the condition to trigger the continuation is www.isDone.
The lambda closure captures the www object so it can be used when the www is done. The code is non-blocking.
		
	var www = new WWW("someURL");
	ContinuationManager.Add( () => www.isDone, () =>
    {
		if (!string.IsNullOrEmpty(www.error))
			Debug.Log("WWW failed: " + www.error);

		Debug.Log("WWW result : " + www.text);
	}
	);

 */

internal static class ContinuationManager
{
	private class Job
	{
		public Job(Func<bool> completed, Action continueWith)
		{
			Completed = completed;
			ContinueWith = continueWith;
		}
		public Func<bool> Completed { get; private set; }
		public Action ContinueWith { get; private set; }
	}
	
	private static readonly List<Job> jobs = new List<Job>();
	
	public static void Add(Func<bool> completed, Action continueWith)
	{
		if (!jobs.Any()) EditorApplication.update += Update;
		jobs.Add(new Job(completed, continueWith));
	}
	
	private static void Update()
	{
		for (int i = 0; i >= 0; --i)
		{
			var jobIt = jobs[i];
			if (jobIt.Completed())
			{
				jobIt.ContinueWith();
				jobs.RemoveAt(i);
			}
		}
		if (!jobs.Any()) EditorApplication.update -= Update;
	}
}
