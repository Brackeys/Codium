using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using MSCS;

/// ScriptEditor.cs (Attach this to any game object) /// Useage Notes: /// For this to run, you MUST: /// 1. Go to File-->Build Settings... /// 2. Click "Player Settings..." /// 3. "Other Settings" /// 4. "Optimization"-->"API Compatability Level" /// 5. Change from ".NET 2.0 Subset" --> ".NET 2.0" ///
public class ScriptEditor : MonoBehaviour {
	
	void Update()
	{
		// Dont run our script until it has been COMPILED successfully.
		if(myScript_Type == null || myScript_Instance == null) return;      
		
		// Run the script's Update() function
		myScript_Type.InvokeMember("Update",BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, myScript_Instance, null);
		
		// Read the scripts "readme" variable.
		PropertyInfo readmePropertyInfo = myScript_Type.GetProperty("readme");        // Source: http://www.csharp-examples.net/reflection-examples/
		readme = (string)readmePropertyInfo.GetValue(myScript_Instance, null);
	}
	
	private string scriptText = 
		@"using System;
 using System.Collections.Generic;
 
 public class MyScript
 {
     public string readme {get;set;}  // Our unity program will read this variable.
     private int counter = 0;    
 
     public MyScript() // Constructor
     {
         readme = """";
     }
 
     public void Update()  // Our unity game will run this function.
     {
         counter++;        
         readme = ""Hi! Script is Running! "" + counter + ""\n"";
     }
 
 
 }
 ";
	
	private string readme = "";            // Displayed on the GUI
	private string compilerErrorMessages = "";        // Displayed on the GUI
	
	void OnGUI() 
	{
		// ********** Display the script in Unity
		scriptText = GUI.TextArea(new Rect(10, 10, 700, 500), scriptText);
		
		// ********** Compile your script
		if (GUI.Button(new Rect(720, 10, 300, 30), "Compile and Run"))
		{                            
			Compile ();      // Compile the script. Write errors to "compilerErrorMessages", if any.
		}
		
		// ********** Display any compiler errors
		GUI.TextArea(new Rect(10, 510, 700, 50), compilerErrorMessages);  // The console for displaying errors.
		
		// ********** Display the Script's Output.
		GUI.TextArea (new Rect(720, 50, 300, 30), readme);     
	}
	
	private Assembly generatedAssembly;                    // Compiled code is called an "Assembly"
	private Type myScript_Type = null;                    // These two variables are used run the compiled code.
	private object myScript_Instance = null;            // These two variables are used run the compiled code.
	
	private void Compile()
	{
		try
		{
			compilerErrorMessages = "";  // clear any previous messages
			
			// ********** Create an instance of the C# compiler   
			CSharpCodeCompiler codeProvider = new CSharpCodeCompiler();
			
			// ********** add compiler parameters
			CompilerParameters compilerParams = new CompilerParameters();
			compilerParams.CompilerOptions = "/target:library /optimize /warn:0"; 
			compilerParams.GenerateExecutable = false;
			compilerParams.GenerateInMemory = true;
			compilerParams.IncludeDebugInformation = false;
			compilerParams.ReferencedAssemblies.Add("System.dll");
			compilerParams.ReferencedAssemblies.Add("System.Core.dll");
			
			// ********** actually compile the code  ??????? THIS LINE WORKS IN UNITY EDITOR --- BUT NOT IN BUILD ??????????
			CompilerResults results = codeProvider.CompileAssemblyFromSource(compilerParams,scriptText);
			
			// ********** Do we have any compiler errors
			if (results.Errors.Count > 0)
			{
				foreach (CompilerError error in results.Errors)
					compilerErrorMessages = compilerErrorMessages + error.ErrorText + '\n';
			}
			else
			{
				// ********** get a hold of the actual assembly that was generated
				generatedAssembly = results.CompiledAssembly;
				
				if(generatedAssembly != null)
				{
					// get type of class Calculator from just loaded assembly
					myScript_Type = generatedAssembly.GetType("MyScript");
					
					// create instance of class MyScript
					myScript_Instance = Activator.CreateInstance(myScript_Type);
					
					// Say success!
					compilerErrorMessages = "Success!";
				}
			}
		}
		catch(Exception o)
		{
			compilerErrorMessages = ""+o.Message +"\n"+ o.Source +"\n"+ o.StackTrace +"\n";
		}
		
	}
	
}