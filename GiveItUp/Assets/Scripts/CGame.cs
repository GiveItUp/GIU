using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CGame : MonoBehaviour 
{
	public const string VERSION = "v1.0.3";
	public const int LEVEL_COUNT = 21;

	public static CGame Instance;
	public static PopupLayer popupLayer;
	public static MenuLayer menuLayer;	
	public static Gamelogic gamelogic;
	public static CameraLogic cameraLogic;

	public PopupLayer p_PopupLayer;
	public MenuLayer p_MenuLayer;
	public Gamelogic p_Gamelogic;

	#region Unity Methods
	void Awake()
	{	
		Application.targetFrameRate = 55;
		Instance = this;


	}

	void Start()
	{
		RefreshMute ();
		
		InitLayers ();
		
		cameraLogic = Camera.main.GetComponent<CameraLogic> ();
		
		User.dailyChallengeLeaderboardScore = PlayerPrefs.GetInt("DCLS", 0);

		CGame.menuLayer.ShowMainMenuGUI ();

		if(User.Version != CGame.VERSION)
		{
			popupLayer.ShowUpdatePopupGUI();
			User.Version = CGame.VERSION;
			User.Save();
		}

//		PluginManager.iap.OnProductsSet -= OnProductsSet;
//		PluginManager.iap.OnProductsSet += OnProductsSet;
//		
//		PluginManager.iap.OnPurchaseSuccess -= OnPurchaseSuccess;
//		PluginManager.iap.OnPurchaseSuccess += OnPurchaseSuccess;
//		
//		PluginManager.iap.OnPurchaseFailed -= OnPurchaseFailed;
//		PluginManager.iap.OnPurchaseFailed += OnPurchaseFailed;
//		
//		PluginManager.iap.OnPurchaseCancelled -= OnPurchaseCancelled;
//		PluginManager.iap.OnPurchaseCancelled += OnPurchaseCancelled;

#if UNITY_ANDROID
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent -= OnAskForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent += OnAskForReviewRemindMeLaterEvent;

		EtceteraAndroidManager.askForReviewDontAskAgainEvent -= OnAskForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent += OnAskForReviewDontAskAgainEvent;

		EtceteraAndroidManager.askForReviewWillOpenMarketEvent -= OnAskForReviewWillOpenMarketEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent += OnAskForReviewWillOpenMarketEvent;

#elif UNITY_IPHONE
		/*
		EtceteraBinding.askForReviewRemindMeLaterEvent -= OnAskForReviewRemindMeLaterEvent;
		EtceteraBinding.askForReviewRemindMeLaterEvent += OnAskForReviewRemindMeLaterEvent;
		
		EtceteraBinding.askForReviewDontAskAgainEvent -= OnAskForReviewDontAskAgainEvent;
		EtceteraBinding.askForReviewDontAskAgainEvent += OnAskForReviewDontAskAgainEvent;
		
		EtceteraBinding.askForReviewWillOpenMarketEvent -= OnAskForReviewWillOpenMarketEvent;
		EtceteraBinding.askForReviewWillOpenMarketEvent += OnAskForReviewWillOpenMarketEvent;*/
#endif

	}
	
	
	void OnApplicationPause( bool pause )
	{
#if UNITY_IPHONE
		if (!pause)
		{
			// Cancel notis
		
//			for (int i = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications.Length - 1; i >= 0; i--)
//			{
//				if (UnityEngine.iOS.NotificationServices.scheduledLocalNotifications[i].userInfo != null &&
//					UnityEngine.iOS.NotificationServices.scheduledLocalNotifications[i].userInfo.Contains("giveitup"))
//					{
//						UnityEngine.iOS.NotificationServices.CancelLocalNotification(UnityEngine.iOS.NotificationServices.scheduledLocalNotifications[i]);
//					}
//			}
//			
//			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
		}
		else
		{
			// Make new noti
		
//			UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification();
//			notif.fireDate = DateTime.Now.Date.AddHours(33); // masnap 9-kor jon noti
//			notif.userInfo.Add("giveitup", "giveitup");
//			notif.alertBody = "New daily challenge is available";
//			notif.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
//			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
		}
#endif
	}

	
	#endregion
	
	#region Methods
	private void InitLayers()
	{
		popupLayer = GameObject.Instantiate (p_PopupLayer) as PopupLayer;
		popupLayer.transform.localPosition = new Vector3 (0,0,-1);
		popupLayer.Init ();
		
		menuLayer = GameObject.Instantiate (p_MenuLayer) as MenuLayer;
		menuLayer.transform.localPosition = new Vector3 (0,0,-1);
	}

	public void InitGamelogic(int stageIndex, Stage stage)
	{
		DestroyGamelogic ();

		gamelogic = GameObject.Instantiate (p_Gamelogic) as Gamelogic;
		gamelogic.Init (stageIndex, stage);
	}

	public void InitGamelogicRandom()
	{
		DestroyGamelogic ();
		
		gamelogic = GameObject.Instantiate (p_Gamelogic) as Gamelogic;
		
		gamelogic.InitRandom ();
	}
	

	public void DestroyGamelogic()
	{
		if(gamelogic != null)
		{
			GameObject.Destroy(gamelogic.gameObject);
			gamelogic = null;
		}
	}

	public void RefreshMute()
	{
		AudioListener.volume = User.IsSoundEnabled ? 1 : 0;
	}
	
	public void AskForReview()
	{
		#if UNITY_ANDROID
		EtceteraAndroid.askForReview(1, 0, 1, "Please rate the game", "Yeah, we know our game is a bit annoying, but please rate it if you like it! Thanks for your feedback / support!");
		#elif UNITY_IPHONE
		EtceteraBinding.askForReview(1, 1, "Please rate the game", "Yeah, we know our game is a bit annoying, but please rate it if you like it! Thanks for your feedback / support!", "932389062");
		#endif
	}
	
	public void AskForReviewNow()
	{
		#if UNITY_ANDROID
		EtceteraAndroid.askForReviewNow("Please rate the game", "Yeah, we know our game is a bit annoying, but please rate it if you like it! Thanks for your feedback / support!");
		#elif UNITY_IPHONE
		EtceteraBinding.askForReview("Please rate the game", "Yeah, we know our game is a bit annoying, but please rate it if you like it! Thanks for your feedback / support!", "932389062");
		#endif
	}

	private AudioSource audio_menumusic = null;
	public void PlayMenuMusic()
	{
		if(audio_menumusic == null)
		{
			audio_menumusic = SoundManager.Instance.Play(SoundManager.eSoundClip.Music_Menu, 0);
		}
	}

	public void DestroyMenuMusic()
	{
		if(audio_menumusic != null)
		{
			GameObject.Destroy(audio_menumusic.gameObject);
		}
		audio_menumusic = null;
	}
	
	
	public List<Texture2D> GetGameCenterFriendTexturesOnStage(int stageIndex)
	{
		List<Texture2D> ret = new List<Texture2D> ();
		//FIXME
		
		
//		if (PluginManager.Instance == null || PluginManager.Instance.gcFriendProgresses == null)
//			return ret;
//		
//		foreach (GCFriendProgress gcfp in PluginManager.Instance.gcFriendProgresses)
//		{
//			if ( gcfp.progress == stageIndex)
//			{
//				ret.Add(gcfp.pic);
//			}
//		}
		
		Util.Shuffle<Texture2D>(ret);
		
		return ret;
	}

	#endregion

	#region IAP
	private void OnProductsSet()
	{
		menuLayer.MainMenuGUI_Refresh ();
	}
	
	private void OnPurchaseSuccess()
	{
		bool needPopup = true;
//		switch(iap)
//		{
//		case eIAP.NoAds:
//			if (User.HasIAP_RemoveAds)
//				needPopup = false;
//			User.HasIAP_RemoveAds = true;
//			User.Save();
//			break;
//		case eIAP.UnlockAll: 
			bool needrefresh = true;
			if (User.HasIAP_UnlockAll)
			{
				needPopup = false;
				needrefresh = false;
			}
			User.HasIAP_UnlockAll = true;
			User.Save();
			
			//PluginManager.social.SubmitScore(eLeaderboard.CompletedLevels, 18);
			
			if(needrefresh)
			{
				if(menuLayer.HasMainMenu())
					menuLayer.ShowMainMenuGUI();
			}

//			break;
//		}
		if (needPopup)
		{
			popupLayer.ClosePurchaseLoadingGUI();
			menuLayer.MainMenuGUI_Refresh ();
			popupLayer.ShowInfoPopupGUI(TextManager.Get("Purchase success!"));
		}
	}
	
	private void OnPurchaseFailed()
	{
		popupLayer.ClosePurchaseLoadingGUI ();
		menuLayer.MainMenuGUI_Refresh ();
		popupLayer.ShowInfoPopupGUI (TextManager.Get("Purchase failed!"));
	}
	
	private void OnPurchaseCancelled()
	{
		popupLayer.ClosePurchaseLoadingGUI ();
		menuLayer.MainMenuGUI_Refresh ();
		popupLayer.ShowInfoPopupGUI (TextManager.Get("Purchase cancelled!"));
	}
	
	private void OnAskForReviewRemindMeLaterEvent()
	{
		User.NeedRateNotififaction = true;
	}

	private void OnAskForReviewDontAskAgainEvent()
	{
		User.NeedRateNotififaction = false;
	}

	private void OnAskForReviewWillOpenMarketEvent()
	{
		User.NeedRateNotififaction = false;
	}

	#endregion

}
