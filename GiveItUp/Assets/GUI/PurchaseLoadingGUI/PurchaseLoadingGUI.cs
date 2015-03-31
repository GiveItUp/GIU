using UnityEngine;
using System.Collections;

public class PurchaseLoadingGUI : GUIPopup {

	public SpriteText lbl_text;

	#region Init
	public void Init()
	{
		InitLabels ();
		InitButtons ();

		BlackGUIBehind ();
	}
	#endregion
	
	#region Methods
	private void InitLabels()
	{
		lbl_text.Text = TextManager.Get("Waiting for purchase...");
	}

	private void InitButtons()
	{
	}
	#endregion
	
	#region Handlers
	#endregion
}
