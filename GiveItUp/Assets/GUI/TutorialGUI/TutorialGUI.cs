using UnityEngine;
using System.Collections;

public class TutorialGUI : GUIPopup {
	
	public SpriteText lbl_title;
	public Transform bubble;

	private string _title;  

	#region Init
	public void Init(string title, Vector2 pos)
	{
		_title = title;

		InitLabels ();
		InitButtons ();

		PositionThis ();

		Vector3 p = bubble.position;
		p.x = pos.x;
		p.y = pos.y;
		bubble.position = pos;

	}
	#endregion

	#region Methods
	
	private void InitLabels()
	{
		lbl_title.Text = TextManager.Get (_title);//;_title.ToUpper();
	}
	
	private void InitButtons()
	{

	}

	private void PositionThis()
	{
		float w = GfxSettings.Instance ().GetScreenWidth () / 2f;
		float h = GfxSettings.Instance ().GetScreenHeight () / 2f;
	}

	#endregion

	#region Handlers
	
	
	private void OnClose()
	{
		CGame.popupLayer.CloseOptionsGUI ();
	}
	
	#endregion
}
