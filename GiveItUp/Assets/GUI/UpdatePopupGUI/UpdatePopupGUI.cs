﻿using UnityEngine;
using System.Collections;

public class UpdatePopupGUI : GUIPopup {

	public SpriteText lbl_text;
	
	public UIButton btn_ok;
	public PackedSprite ps_bgr;

	#region Init
	public void Init()
	{
		InitLabels ();
		InitButtons ();
		
		BlackGUIBehind ();
		
		SoundManager.Instance.Play (SoundManager.eSoundClip.GUI_Button_popup, 1);
		
		StartCoroutine (PlayShowAnim());
	}
	#endregion
	
	#region Methods
	
	private IEnumerator PlayShowAnim()
	{
		ComponentAnimation_Prepare (ps_bgr.transform);
		
		yield return StartCoroutine(ComponentAnimation_Show (ps_bgr.transform, 0.1f));
		
		InitBackButton (() => { StartCoroutine(OnOk()); } );
	}
	
	private IEnumerator PlayHideAnim()
	{
		yield return StartCoroutine(ComponentAnimation_Hide (ps_bgr.transform, 0.1f));
		
		yield return new WaitForSeconds (0.3f);
	}
	
	private void InitLabels()
	{
		string text = TextManager.Get("Update notes:")+"\n\n";

		text += "";
		text += TextManager.Get("** 9 new levels of varied difficulty")+"\n\n";
		text += TextManager.Get("** Daily challenge: complete daily challenge levels the higher percent to increase your scores on the Leaderboard")+"\n\n";
		text += TextManager.Get("** New crazy music");

		lbl_text.Text = text;
	}
	
	private void InitButtons()
	{
		btn_ok.SetInputDelegate (OnBtn_Ok_Input);
	}
	#endregion
	
	#region Handlers
	
	void OnBtn_Ok_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			StartCoroutine(OnOk());
			
			break;
		}
	}
	
	private IEnumerator OnOk()
	{
		if(_inputEnabled)
		{
			_inputEnabled = false;
			SoundManager.PlayButtonTapSound();
			
			yield return StartCoroutine(PlayHideAnim());
			
			CGame.popupLayer.CloseUpdatePopupGUI();
		}
	}
	#endregion

}
