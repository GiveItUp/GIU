using UnityEngine;
using System.Collections;

public class HUD : GUILayer {

	public UIButton btn_pause;
	public UIProgressBar pb_progress;
	public SpriteText lbl_stage;
	public SpriteText lbl_tries;

	private bool _started = false;

	#region Init
	public void Init()
	{
		InitLabels ();
		InitButtons ();

		PositionThis ();

		pb_progress.gameObject.SetActive (false);

		StartCoroutine (PlayShowAnim());
	}
	#endregion
	
	#region Methods

	private IEnumerator PlayShowAnim()
	{
		ComponentAnimation_Prepare (lbl_stage.transform);

		yield return new WaitForSeconds (0.2f);

		if(!_started)
			yield return StartCoroutine(ComponentAnimation_Show (lbl_stage.transform, 0.1f));

		lbl_tries.gameObject.SetActive (true);
		if (CGame.gamelogic.state == Gamelogic.eState.menu)
			lbl_tries.GetComponent<Animation>().Play ("ShowTriesAnim");

		SoundManager.Instance.Play(SoundManager.eSoundClip.GUI_PopupShowComponent, 1);
	}

	public void PlayTriesAnimHide()
	{
		_started = true;
		lbl_tries.GetComponent<Animation>().Stop ("ShowTriesAnim");
		lbl_tries.GetComponent<Animation>().Play ("HideTriesAnim");
		StartCoroutine(ComponentAnimation_Hide (lbl_stage.transform, 0.0f));
	}

	public void Reset()
	{
		SetProgress (0);
	}

	private void InitLabels()
	{
		int stageIndex = CGame.gamelogic.GetStageIndex();
	
		if (stageIndex == -1)
		{
			lbl_stage.Text = TextManager.Get("Daily challenge");
		}
		else
		{
			lbl_stage.Text = TextManager.Get("Stage ") + (stageIndex+ 1);
		}
		
		lbl_tries.Text = string.Format(TextManager.Get("and your {0}. try..."),User.GetLevelTries(stageIndex));
		lbl_tries.gameObject.SetActive (false);
	}

	private void InitButtons()
	{
		btn_pause.SetInputDelegate (OnBtn_Pause_Input);
		btn_pause.gameObject.SetActive (false);
	}

	public void SetProgress(float progress)
	{
		/*
		if(_lastProgress != progress)
		{
			//lbl_progress.Text = "" + Mathf.RoundToInt(progress * 100f);
			pb_progress.Value = progress;
		}*/
	}

	private void PositionThis()
	{
		float w = GfxSettings.Instance ().GetScreenWidth () / 2.0f;
		float h = GfxSettings.Instance ().GetScreenHeight () / 2.0f;

		btn_pause.transform.localPosition = new Vector3 (w - 100, h - 100, -1);
	}
	#endregion
	
	#region Handlers
	void OnBtn_Pause_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			OnPause();
			
			break;
		}
	}
	
	private void OnPause()
	{
		SoundManager.PlayButtonTapSound();
	}
	#endregion
}
