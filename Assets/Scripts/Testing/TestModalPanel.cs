using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestModalPanel : MonoBehaviour {

	private ModalPanel modalPanel;

	private UnityAction myYesAction;
	private UnityAction myNoAction;
	private UnityAction myCancelAction;
	private UnityAction myOKAction;

	void Awake()
	{
		modalPanel = ModalPanel.ins;

		myYesAction = new UnityAction(TestYesMethod);
		myNoAction = new UnityAction(TestNoMethod);
		myCancelAction = new UnityAction(TestCancelMethod);
		myOKAction = new UnityAction(TestOKMethod);
	}

	public void TextYNC()
	{
		modalPanel.Choice("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
	}
	public void TextYN()
	{
		modalPanel.Choice("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction);
	}
	public void TextOK()
	{
		modalPanel.Notification("Would you like a poke in the eye?\nHow about with a sharp stick?", myOKAction);
	}
	public void TextCalmChoice()
	{
		modalPanel.CalmChoice("Would you like a poke in the eye?\nHow about with a sharp stick?", myOKAction, myCancelAction);
	}

	void TestYesMethod()
	{
		Debug.Log("YES!");
	}

	void TestNoMethod()
	{
		Debug.Log("NO!");
	}

	void TestCancelMethod()
	{
		Debug.Log("CANCEL!");
	}

	void TestOKMethod()
	{
		Debug.Log("OK!");
	}

}
