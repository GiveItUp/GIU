using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultsSuccessGUI : GUIPopup {
	
	public UIButton btn_restart;
	public UIButton btn_menu;
	public UIButton btn_next;
	public UIButton btn_share;
	public UIButton btn_rate;
	
	public PackedSprite ps_rate_notification;

	public SpriteText lbl_not_bad;
	public SpriteText lbl_score;
	public SpriteText lbl_score_outline;
	public SpriteText lbl_bestscore;
	
	public ParticleSystem firework_prefab;
	
	private Results _results;

	private Stage _nextStage;

	private AudioSource audio_results = null;

	#region Init
	public void Init(Results result)
	{
		_inputEnabled = false;
		_results = result;
		_nextStage = Storage.Instance.GetValidStage (_results.StageIndex + 1);
		
		InitButtons ();
		InitLabels ();

		ps_rate_notification.gameObject.SetActive (User.NeedRateNotififaction);

		StartCoroutine (PlayShowAnim ());
		
		StartCoroutine (PlayFirework());

//        if (result.StageIndex == 8)
//            PluginManager.social.ReportAchievement(eAchievement.stage9, 100f);

		
	}
	#endregion
	
	#region Unity Methods
	protected override void Update()
	{
		base.Update ();

		if (Input.GetKeyDown(KeyCode.Return))
		{
			StartCoroutine(OnNext());
		}
	}
	#endregion
	
	#region Methods
	
	private IEnumerator PlayFirework()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
			
			ParticleSystem ps = Instantiate(firework_prefab) as ParticleSystem;
			ps.transform.position = new Vector3(Random.Range(-500, 500), Random.Range(-350, 350), -800);
			Destroy(ps.gameObject, 3);

			SoundManager.PlayRandomParticleSound();
		}
	}
	
	private IEnumerator PlayShowAnim()
	{
		//ComponentAnimation_Prepare (btn_share.transform);
		ComponentAnimation_Prepare (btn_menu.transform);
		ComponentAnimation_Prepare (btn_next.transform);
		ComponentAnimation_Prepare (lbl_not_bad.transform);
		ComponentAnimation_Prepare (lbl_score.transform);
		ComponentAnimation_Prepare (lbl_score_outline.transform);
		ComponentAnimation_Prepare (lbl_bestscore.transform);
		ComponentAnimation_Prepare (btn_restart.transform);
		//ComponentAnimation_Prepare (btn_rate.transform);
		
		yield return StartCoroutine(ComponentAnimation_Show (lbl_not_bad.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (lbl_score.transform, 0f));
		yield return StartCoroutine(ComponentAnimation_Show (lbl_score_outline.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (lbl_bestscore.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Show (btn_share.transform, 0.1f));
		if(_nextStage != null)
			yield return StartCoroutine(ComponentAnimation_Show (btn_next.transform, 0.1f));
		else
			yield return StartCoroutine(ComponentAnimation_Show (btn_restart.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Show (btn_rate.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Show (btn_menu.transform, 0.1f));

		InitBackButton (() => { StartCoroutine(OnMenu()); } );
		
		_inputEnabled = true;

		if (PlayerPrefs.GetInt ("TVMode", 0) != 1) {
			btn_restart.GetComponent<Animation>().Play ("ButtonAnim");
			btn_next.GetComponent<Animation>().Play ("ButtonAnim");
		}

	//	audio_results = SoundManager.Instance.Play (SoundManager.eSoundClip.Music_Results, 0);
	}

	private IEnumerator PlayHideAnim()
	{
		yield return StartCoroutine(ComponentAnimation_Hide (btn_menu.transform, 0.1f));
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_rate.transform, 0.1f));

		if(_nextStage != null)
			yield return StartCoroutine(ComponentAnimation_Hide (btn_next.transform, 0.1f));
		else
			yield return StartCoroutine(ComponentAnimation_Hide (btn_restart.transform, 0.1f));
		
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_share.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_bestscore.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_score_outline.transform, 0f, false));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_score.transform, 0.1f));
		yield return StartCoroutine(ComponentAnimation_Hide (lbl_not_bad.transform, 0.1f));

		yield return new WaitForSeconds (0.3f);
	}

	private void InitLabels()
	{
		lbl_score.Text = "" + _results.Score + "%";
		lbl_score_outline.Text = "" + _results.Score + "%";
		lbl_bestscore.Text = TextManager.Get("Best: ") + _results.BestScore + "%";
		lbl_not_bad.Text = TextManager.Get(Results.GetResultString (_results.Score));
	}
	
	private void InitButtons()
	{
		btn_share.SetInputDelegate (OnBtn_Share_Input);
		btn_menu.SetInputDelegate (OnBtn_Menu_Input);
		btn_next.SetInputDelegate (OnBtn_Next_Input);
		btn_restart.SetInputDelegate (OnBtn_Next_Input);
		btn_rate.SetInputDelegate (OnBtn_Rate_Input);

		//btn_next.gameObject.SetActive (_nextStage != null);
	}

	private void DestroyMusic()
	{
		if(audio_results != null)
		{
			GameObject.Destroy(audio_results.gameObject);
			audio_results = null;
		}
	}
	#endregion
	
	#region Handlers
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
				//{ "picture", "" },
				{ "caption", TextManager.Get(string.Format("I just completed stage {0}",(_results.StageIndex + 1).ToString() ))}
			};
//            FacebookAndroid.showDialog("stream.publish", parameters);
#endif
#if UNITY_IPHONE
            var parameters = new Dictionary<string, string>
			{
				{ "link", "https://itunes.apple.com/app/give-it-up!/id932389062" },
				{ "name", "Download Give It Up! For Free!" },
				//{ "picture", "" },
				{ "caption", "I just completed stage " + (_results.StageIndex + 1).ToString() }
			};
          //  FacebookBinding.showDialog("stream.publish", parameters);

#endif
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
			
			DestroyMusic();
			_inputEnabled = false;
			SoundManager.PlayButtonBackSound();
			
			yield return StartCoroutine(PlayHideAnim());

			CGame.Instance.DestroyGamelogic ();
			CGame.popupLayer.CloseResultsSuccessGUI ();
			CGame.menuLayer.ShowMainMenuGUI ();
		}
	}
	
	void OnBtn_Next_Input(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) return;
			
			StartCoroutine(OnNext());
			
			break;
		}
	}
	
	private IEnumerator OnNext()
	{
		if(_inputEnabled)
		{
			DestroyMusic();
			_inputEnabled = false;
			SoundManager.PlayButtonTapSound();
			
			yield return StartCoroutine(PlayHideAnim());
			
			CGame.popupLayer.CloseResultsSuccessGUI();
			
			if (_results.StageIndex == -1 )
			{
				CGame.Instance.InitGamelogicRandom();
				yield break;
			}
			
			if(_nextStage != null)
				CGame.Instance.InitGamelogic (_results.StageIndex + 1, _nextStage);
			else
				CGame.Instance.InitGamelogic (_results.StageIndex, _results.Stage);
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
	#endregion
}
