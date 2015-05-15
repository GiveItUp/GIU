//#define USE_PROGRESS_BASED_CHALLENGE


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public class Gamelogic : MonoBehaviour
{
	[System.Serializable]
	public class SoundList
	{
		public List<AudioSource> sounds;
	}


    public enum eState
    {
        menu,
        ingame,
        results_success,
        results_fail
    }

	public Transform recordMarker;

	public HUD p_HUD;
	public IngameBgr p_IngameBgr;

	public World world;
	public Ball ball;
	
	[HideInInspector]
	public AudioSource music;
	public AudioSource music1;
	public AudioSource music2;
	public AudioSource music3;
	private int challengeMusic;
	
	public int currentUpSoundPack = 0;
	public int currentScratchSoundPack = 0;
	public int currentScratchSound = 0;
	public List<SoundList> upSounds;
	public List<SoundList> scratchSounds;

	[System.NonSerialized]
	public HUD hud;

	[System.NonSerialized]
	public IngameBgr ingameBgr;

	[System.NonSerialized]
    public eState state;
    float progress;
    
    public delegate void OnJumpedDelegate();
    public OnJumpedDelegate OnJumped;

	private int _stageIndex = 0;
	private Stage _stage;

	public int GetStageIndex() { return _stageIndex; }
	#region Methods

	private void Awake()
	{
		GameObject go = Resources.Load("Prefabs/Monstar/"+CGame.ballName) as GameObject;
		go = GameObject.Instantiate(go) as GameObject;
		go.transform.parent = this.transform;
		ball = go.GetComponent<Ball>();
	}
	
	public void InitRandom()
	{
		Stage stage = new Stage();
		
		challengeMusic = PlayerPrefs.GetInt("ChallengeMusic", Random.Range(0,2));
		
		#if USE_PROGRESS_BASED_CHALLENGE
		float progress = User.actualRandomLevelScore;
		if (progress >= 100)
		#else
		if ( User.lastRandomLevelDate.CompareTo(System.DateTime.Now.Date) != 0 )
		#endif
		{
			stage.GenerateFromChunks( 200, 300);
			stage.Save( "STG_RND" );
			
			User.actualRandomLevelScore = 0;
			User.actualRandomLevelRecord = 0;
			User.actualRandomLevelTries = 0;
			User.lastRandomLevelDate = System.DateTime.Now.Date;
			
			if (PlayerPrefs.GetInt("ChallengeMusic", -1) == -1)
			{
				challengeMusic = 1;
			}
			else
			{
				challengeMusic = Random.Range(0,2);
			}
			PlayerPrefs.SetInt("ChallengeMusic", challengeMusic);
		}
		else
		{
			stage.Load( "STG_RND" );
		}
		
		
		recordMarker.gameObject.SetActive (false);
		_stageIndex = -1;
		_stage = stage;
		world.Init(_stage);
		
		User.AddLevelTries (_stageIndex);
		
		InitHUD ();
		InitIngameBgr ();
		ChangeState (eState.menu);

		CGame.cameraLogic.ball = ball.transform;
		
		if (!LevelEditor.testmode && (int)User.GetLevelRecord (_stageIndex) > 2)
		{
			Column c;
			if (User.GetLevelScore(_stageIndex) == 100)
			{
				c = world.GetNextColumn (0, (int)User.GetLevelRecord (_stageIndex) + 1);
			}
			else
			{
				c = world.GetNextColumn (0, (int)User.GetLevelRecord (_stageIndex));
			}
			if (c == null)
			{
				Destroy(recordMarker.gameObject);
			}
			else
			{
				recordMarker.gameObject.SetActive (true);
				recordMarker.transform.parent = c.sprite.transform;
				recordMarker.transform.localPosition = new Vector3 (0, -0.18f, -1);
				
				if(recordMarker.transform.position.x <= 2)
					Destroy(recordMarker.gameObject);
			}
		}
		else
		{
			Destroy(recordMarker.gameObject);
		}
		
		
	}
	
	public void Init(int stageIndex, Stage stage)
	{
		CGame.cameraLogic.ball = ball.transform;
		recordMarker.gameObject.SetActive (false);
		_stageIndex = stageIndex;
		_stage = stage;
        world.Init(_stage);
		User.LastPlayedStage = _stageIndex;
		User.AddLevelTries (_stageIndex);
		InitHUD ();
		InitIngameBgr ();
		ChangeState (eState.menu);


		if (!LevelEditor.testmode && (int)User.GetLevelRecord (_stageIndex) > 2)
		{
			Column c;
			if (User.GetLevelScore(_stageIndex) == 100)
			{
				c = world.GetNextColumn (0, (int)User.GetLevelRecord (_stageIndex) + 1);
			}
			else
			{
				c = world.GetNextColumn (0, (int)User.GetLevelRecord (_stageIndex));
			}
			if (c == null)
			{
				Destroy(recordMarker.gameObject);
			}
			else
			{
				recordMarker.gameObject.SetActive (true);
                if (c != null && c.sprite != null)
				    recordMarker.transform.parent = c.sprite.transform;
				recordMarker.transform.localPosition = new Vector3 (0, -0.18f, -1);

				if(recordMarker.transform.position.x <= 2)
					Destroy(recordMarker.gameObject);
			}
		}
		else
		{
			Destroy(recordMarker.gameObject);
		}
	}
	
	private void InitHUD()
	{
		hud = GameObject.Instantiate (p_HUD) as HUD;
		hud.transform.parent = this.transform;
		hud.transform.localPosition = new Vector3 (0,0,-1);
		hud.Init ();
	}
	
	private void InitIngameBgr()
	{
		ingameBgr = GameObject.Instantiate (p_IngameBgr) as IngameBgr;
		ingameBgr.transform.parent = Camera.main.transform;
		ingameBgr.transform.localPosition = new Vector3 (0,0,-1);
		//ingameBgr.Init ();
	}
	#endregion

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(JoyStickConfig.a))
        {
			if (state == eState.menu)
            {
                ChangeState(eState.ingame);
            }
        }

		if(state != eState.results_fail && state != eState.results_success)
        	progress = CGame.gamelogic.world.GetProgress(ball.transform.position.x);

		if(hud != null)
		{
			hud.SetProgress(progress);
		}
		if(ingameBgr != null)
		{
			ingameBgr.SetProgress(progress);
		}
    }

	private void OnDestroy()
	{
		if(ingameBgr != null)
		{
			GameObject.Destroy(ingameBgr.gameObject);
			ingameBgr = null;
		}
	}
	
	#if TEST_BUILD
	private void OnGUI()
	{
		if(state == eState.ingame)
			if(GUI.Button(new Rect(100,100,100,100), "Complete"))
			{
				ChangeState(eState.results_success);
			}
	}
	#endif

	public IEnumerator ZoomFinish(Transform ballParent)
	{
		Column c = world.GetCurrentColumn (ball.transform.position.x);

		ballParent.transform.parent = c.transform;

		Vector3 newScale = c.transform.localScale * 2;
		Vector3 newPos = c.transform.position + new Vector3(-2, 1, 0);
		Vector3 oldScale = c.transform.localScale;
		Vector3 oldPos = c.transform.position;

		float t = 0;

		while (t <= 1)
		{
			c.transform.localScale = Vector3.Lerp( oldScale, newScale, t);
			c.transform.localPosition = Vector3.Lerp( oldPos, newPos, t);

			t += Time.deltaTime;
			yield return 0;
		}

		c.transform.localScale = newScale;
		c.transform.localPosition = newPos;
	}

    public void ChangeState(eState st)
    {
        state = st;

        switch (state)
		{
			case eState.menu:
				{
					ingameBgr.SetBgr(false);
					ingameBgr.SetProgress(0);
					hud.gameObject.SetActive(true);
					hud.SetProgress(0);
					ingameBgr.SetBgr(false);
					ball.SetActive(false);
					ball.Init();
					CGame.gamelogic.world.ClearColumnColor();
					music.Stop();
				} break;
			case eState.ingame:
				{
					hud.PlayTriesAnimHide();
					ball.SetActive(true);
					
					if ( music != null && music.isPlaying)
					{
						music.Stop();
					}
					if(_stageIndex > 17)
					{
						music = music3;
					}
					else if ( (_stageIndex == -1 && challengeMusic == 1) || _stageIndex > 8 )
					{
						music = music2;
					}
					else
					{
						music = music1;
					}
					
					//ütem: c=1/3 sec
					//elfogadható csúszás: r=+-c/4=0.15sec 
					//      		              
					//PC: -0.04..+0.11  median:+0.035
					//HTC: -0.23..-0.08  +0.1...0.25  m1:-0.155 m2=0.18 (rengeteg hasonló delayű készülékünk van)
					//NX5 -0.14..0.01  m=-0.074 (kevés hasonló delayű készülékünk van)
					//választott anti-delay: -0.12 (majdnem a HTC medianja, kicsit a NX5 tartományába betolva)
					float antiDelay=0.0f;
#if UNITY_ANDROID && !UNITY_EDITOR
					antiDelay=-0.12f;
#endif
					music.PlayScheduled(AudioSettings.dspTime + 0.333f + antiDelay);
					ball.prevMusicTimeDouble=AudioSettings.dspTime;
					GA.StartLevel((GetStageIndex()+1).ToString());
				} break;
            case eState.results_fail:
				{
					ReportAchievements();
				
					CGame.popupLayer.CloseTutorialGUI();
					ingameBgr.SetBgr(true);
					ingameBgr.PlayFadeAnim(false);
					hud.gameObject.SetActive(false);
                    ball.SetActive(false);
                    music.Stop();

					SoundManager.Instance.Play(SoundManager.eSoundClip.Game_End_Splat_01, 1);

					Results r = new Results();
					r.Stage = _stage;
					r.StageIndex = _stageIndex;
					r.Score = Mathf.RoundToInt(progress * 100f);
					int userBest = User.GetLevelScore(_stageIndex);
					r.BestScore = userBest;
					if(r.Score > r.BestScore)
					{
						if (_stageIndex == -1)
						{
							User.dailyChallengeLeaderboardScore += r.Score - userBest;
							PluginManager.social.SubmitScore(eSocialAdapter.GameCenter, eLeaderboard.DailyChallengeRank, User.dailyChallengeLeaderboardScore);
						}
						
						PlayerPrefs.SetInt("DCLS", User.dailyChallengeLeaderboardScore);
						
						r.IsNewBest = true;
						r.BestScore = r.Score;
					}
					User.SetLevelScore(_stageIndex, r.Score);
					User.SetLevelRecord(_stageIndex, ball.transform.position.x);
                    world.FallPlatforms(false);
                    Util.CallWithDelay(() => { CGame.popupLayer.ShowResultsGUI(r); }, 0.5f);

					//PluginManager.analytics.TrackEvent(eEvent.playOnLevel, new Dictionary<string, string> { { "percent", ((int)(r.Score / 10) * 10).ToString() } });

//                    if (User.PlayCount % 5 == 1)
//                    {
//                        if (Random.value > 0.5f)
//                            PluginManager.ads.ShowFullscreenAd(eAdNetwork.Chartboost);
//                        else
//                            PluginManager.ads.ShowFullscreenAd(eAdNetwork.AdMob);
//                    }
//
//                    else
//                    {
//                        if (r.IsNewBest || r.Score >= 50)
//                        {
//							CGame.Instance.AskForReview();
//                        }
//                    }
                    User.SaveAData();

					GA.FailLevel((GetStageIndex()+1).ToString());
                } break;
            case eState.results_success:
				{
					ReportAchievements();
			
					hud.gameObject.SetActive(false);
					ball.SetActive(false);
					music.Stop();
					
					SoundManager.Instance.Play (SoundManager.eSoundClip.End_Success, 1);
                    ingameBgr.PlayFadeAnim(true);

					if (recordMarker != null)
						Destroy(recordMarker.gameObject);

					PlayerPrefs.SetInt("HasPlayedTutorial", 1);
				
					progress = 1;
					Results r = new Results();
					r.Stage = _stage;
					r.StageIndex = _stageIndex;
					r.Score = 100;
					
					int userBest = User.GetLevelScore(_stageIndex);

					r.BestScore = userBest;
					if(r.Score > r.BestScore)
					{
						if (_stageIndex == -1)
						{
							User.dailyChallengeLeaderboardScore += r.Score - userBest;
							PluginManager.social.SubmitScore(eSocialAdapter.GameCenter, eLeaderboard.DailyChallengeRank, User.dailyChallengeLeaderboardScore);
						}
						
						PlayerPrefs.SetInt("DCLS", User.dailyChallengeLeaderboardScore);
					
						r.IsNewBest = true;
						r.BestScore = r.Score;
					}
					User.SetLevelScore(_stageIndex, r.Score);
					User.SetLevelRecord(_stageIndex, ball.transform.position.x);
					
					if (_stageIndex >= 0)
					{
						PluginManager.social.ReportAchievement( (eAchievement)((int)(eAchievement.stage1) + _stageIndex), 100);
						PluginManager.social.SubmitScore(eLeaderboard.CompletedLevels, _stageIndex + 1);
					}
					
                    world.FallPlatforms(true);

					GameObject go = new GameObject ("BallParent");
					go.transform.position = CGame.gamelogic.ball.transform.position;
					go.transform.parent = CGame.gamelogic.transform;
					CGame.gamelogic.ball.transform.parent = go.transform;
					CGame.gamelogic.ball.transform.localPosition = Vector3.zero;
					CGame.gamelogic.ball.GetComponent<Animation>().clip = CGame.gamelogic.ball.GetComponent<Animation>() ["ball_poing"].clip;
					CGame.gamelogic.ball.GetComponent<Animation>().Play ();

					StartCoroutine(ZoomFinish(go.transform));


                    //PluginManager.analytics.TrackEvent(eEvent.playOnLevel, new Dictionary<string, string> { { "percent", ((int)(r.Score / 10) * 10).ToString() } });
                    Util.CallWithDelay(() => { CGame.popupLayer.ShowResultsSuccessGUI(r); }, 0.5f);

					if (_stageIndex != -1)
					{
						if (PlayerPrefs.GetInt ("TVMode", 0) != 1 && _stageIndex<1) {
							User.CompleteStage(_stageIndex);
						}else{
							if (!User.HasIAP_UnlockAll) {
//								#if UNITY_ANDROID
//								EtceteraAndroidManager.alertButtonClickedEvent += UnlockAll;
//								EtceteraAndroid.showAlert ("解锁全部关卡", TextManager.Get ("Unlock all info"), "好的", "取消");
//								#endif
								PluginManager.iap.PurchaseProduct(eIAP.UnlockAll);
							}
							User.CompleteStage(_stageIndex);
						}
					}
//                    if (User.PlayCount % 5 == 1)
//                    {
//                        if (Random.value > 0.5f)
//                            PluginManager.ads.ShowFullscreenAd(eAdNetwork.Chartboost);
//                        else
//                            PluginManager.ads.ShowFullscreenAd(eAdNetwork.AdMob);
//                    }
//                    else
//					{
//						CGame.Instance.AskForReview();
//                    }
                    User.SaveAData();
					GA.FinishLevel((GetStageIndex()+1).ToString());
                } break;
        }
    }
    
	void UnlockAll (string s)
	{
		if (s == "好的") {
			if (PlayerPrefs.GetInt ("DebugMode", 0) != 1) {
				CGame.popupLayer.ShowPurchaseLoadingGUI ();
				ReinPluginManager.Purchase ("UnlockAll");
			} else {
				CGame.Instance.gameObject.SendMessage ("OnPurchaseSuccess");
			}
		}
	}

    void ReportAchievements()
    {
		PluginManager.social.ReportAchievement(eAchievement.jump_up_100, (float)User.JumpUpCount / 100f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_up_200, (float)User.JumpUpCount / 200f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_up_500, (float)User.JumpUpCount / 500f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_up_1000, (float)User.JumpUpCount / 1000f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_up_5000, (float)User.JumpUpCount / 5000f * 100f);
		
		PluginManager.social.ReportAchievement(eAchievement.jump_500, (float)User.JumpCount / 500f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_200, (float)User.JumpCount / 200f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_1000, (float)User.JumpCount / 1000f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_5000, (float)User.JumpCount / 5000f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_10000, (float)User.JumpCount / 10000f * 100f);
		
		PluginManager.social.ReportAchievement(eAchievement.jump_spike_10, (float)User.JumpOverSpikeCount / 10f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_spike_20, (float)User.JumpOverSpikeCount / 20f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_spike_50, (float)User.JumpOverSpikeCount / 50f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_spike_100, (float)User.JumpOverSpikeCount / 100f * 100f);
		PluginManager.social.ReportAchievement(eAchievement.jump_spike_500, (float)User.JumpOverSpikeCount / 500f * 100f);
	}
}
