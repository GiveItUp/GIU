using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultsGUI : GUIPopup {
	
	public UIButton btn_restart;
	public UIButton btn_menu;
	public UIButton btn_share;
	public UIButton btn_rate;

	public PackedSprite ps_rate_notification;
	
	public SpriteText lbl_title;
	public SpriteText lbl_score;
	public SpriteText lbl_score_outline;
	public SpriteText lbl_bestscore;
	
	public SpriteText lbl_newbest;

	private Results _results;

	#region Init
	public void Init(Results result)
	{
		_inputEnabled = false;
		_results = result;

		InitButtons ();
		InitLabels ();

		ps_rate_notification.gameObject.SetActive (User.NeedRateNotififaction);

		StartCoroutine (PlayShowAnim ());
	}
	#endregion

	#region Unity Methods
	protected override void Update()
	{
		base.Update ();

		if (Input.GetKeyDown(KeyCode.Return))
		{
			StartCoroutine(OnRestart());
		}
	}
	#endregion
	
	#region Methods

	private IEnumerator PlayShowAnim()
	{
		ComponentAnimation_Prepare (btn_restart.transform);
		ComponentAnimation_Prepare (btn_menu.transform);
		ComponentAnimation_Prepare (lbl_score.transform);
		ComponentAnimation_Prepare (lbl_score_outline.transform);
		ComponentAnimation_Prepare (lbl_bestscore.transform);
		ComponentAnimation_Prepare (lbl_title.transform);
		//ComponentAnimation_Prepare (btn_share.transform);
		ComponentAnimation_Prepare (lbl_newbest.transform);
		//ComponentAnimation_Prepare (btn_rate.transform);
		
		yield return StartCoroutine(ComponentAnimation_Show (lbl_title.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (lbl_score.transform, 0f));
		yield return StartCoroutine(ComponentAnimation_Show (lbl_score_outline.transform, 0.1f));
		if(_results.IsNewBest)
			yield return StartCoroutine(ComponentAnimation_Show (lbl_newbest.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (lbl_bestscore.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Show (btn_share.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (btn_restart.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Show (btn_rate.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (btn_menu.transform, 0.1f));
		
		InitBackButton (() => { StartCoroutine(OnMenu()); } );

		_inputEnabled = true;

		if(_results.IsNewBest)
			lbl_newbest.GetComponent<Animation>().Play("NewAnim");

		if (PlayerPrefs.GetInt ("TVMode", 0) != 1) {
			btn_restart.GetComponent<Animation>().Play ("ButtonAnim");
		}
	}

	private IEnumerator PlayHideAnim()
	{
		yield return StartCoroutine(ComponentAnimation_Hide (btn_menu.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_rate.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Hide (btn_restart.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_share.transform, 0.15f));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_bestscore.transform, 0.1f));
		if(_results.IsNewBest)
			yield return StartCoroutine(ComponentAnimation_Hide (lbl_newbest.transform, 0f));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_score_outline.transform, 0f, false));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_score.transform, 0.12f));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_title.transform, 0.1f));

		yield return new WaitForSeconds (0.3f);
	}

	private void InitLabels()
	{
		lbl_score.Text = "" + _results.Score + "%";
		lbl_score_outline.Text = "" + _results.Score + "%";
		lbl_bestscore.Text = TextManager.Get("Best: ") + _results.BestScore + "%";
		lbl_title.Text = TextManager.Get(Results.GetResultString (_results.Score));
	}

	private void InitButtons()
	{
		btn_restart.SetInputDelegate (OnBtn_Restart_Input);
		btn_menu.SetInputDelegate (OnBtn_Menu_Input);
		btn_rate.SetInputDelegate (OnBtn_Rate_Input);
		btn_share.SetInputDelegate (OnBtn_Share_Input);
	}
	#endregion
	
	#region Handlers
	void OnBtn_Restart_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			StartCoroutine(OnRestart());
			
			break;
		}
	}
	
	private IEnumerator OnRestart()
	{
		if(_inputEnabled)
		{
			_inputEnabled = false;
			SoundManager.PlayButtonTapSound();

			yield return StartCoroutine(PlayHideAnim());

			CGame.popupLayer.CloseResultsGUI();
			
			if ( _results.StageIndex == -1 )
			{
				CGame.Instance.InitGamelogicRandom();
			}
			else
			{
				CGame.Instance.InitGamelogic (_results.StageIndex, _results.Stage);
			}
		}
	}

	void OnBtn_Menu_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			StartCoroutine(OnMenu());
			
			break;
		}
	}
	
	private IEnumerator OnMenu()
	{
		if(_inputEnabled)
		{
			_inputEnabled = false;
			SoundManager.PlayButtonBackSound();

			yield return StartCoroutine(PlayHideAnim());

			CGame.Instance.DestroyGamelogic ();
			CGame.popupLayer.CloseResultsGUI ();
			CGame.menuLayer.ShowMainMenuGUI ();
		}
	}
	
	void OnBtn_Rate_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			OnRate();
			
			break;
		}
	}
	
	private void OnRate()
	{
		if(_inputEnabled)
		{
			SoundManager.PlayButtonTapSound();
			
			User.NeedRateNotififaction = false;
			
			CGame.Instance.AskForReviewNow();
		}
	}
	
	void OnBtn_Share_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			OnShare();
			
			break;
		}
	}
	
	private void OnShare()
	{
		if(_inputEnabled)
		{
			SoundManager.PlayButtonTapSound();
			
			#if !UNITY_EDITOR
			if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				CGame.popupLayer.ShowInfoPopupGUI (TextManager.Get("Please check your internet connection!"));
				return;
			}
			#endif

#if UNITY_ANDROID
            var parameters = new Dictionary<string, string>
			{
				{ "link", "https://play.google.com/store/apps/details?id=com.invictus.impossiball" },
				{ "name", TextManager.Get("Download Give It Up! For Free!") },

				{ "caption", TextManager.Get(string.Format( "I just reached {0}% on stage {1}",_results.Score,(_results.StageIndex + 1).ToString()) )}
			};
            FacebookAndroid.showDialog("stream.publish", parameters);
#endif
#if UNITY_IPHONE
            var parameters = new Dictionary<string, string>
			{
				{ "link", "https://itunes.apple.com/app/give-it-up!/id932389062" },
				{ "name", "Download Give It Up! For Free!" },
				//{ "picture", "" },
				{ "caption", "I just reached " + _results.Score + "% on stage " + (_results.StageIndex + 1).ToString() }
			};
           // FacebookBinding.showDialog("stream.publish", parameters);

#endif
        }
	}
	#endregion
}
