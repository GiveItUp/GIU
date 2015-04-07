using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Ki lehet egészíteni újakkal is
public enum eSocialAdapter
{
    Test,
    Facebook,
    Twitter,
    LINE,
    WeChat,
    GameCenter,
    GooglePlayServices,
}

//Ki lehet egészíteni újakkal is
public enum eAdNetwork
{
    Test,
    Playhaven,
    Chartboost,
    Vungle,
    AdColony,
    Tapjoy,
    Metaps,
    AdMob,
}

public enum eAnalytics
{
    Test,
    GoogleAnalytics,
    Flurry,
    Playnomics,
}

public enum eLeaderboard
{
    Attempts,
    DailyChallengeRank,
    CompletedLevels
}

public enum eAchievement
{
    jump_200,
    jump_500,
    jump_1000,
    jump_5000,
    jump_10000,
    die_50,
    die_100,
    die_200,
    die_500,
    die_1000,
    jump_spike_10,
    jump_spike_20,
    jump_spike_50,
    jump_spike_100,
    jump_spike_500,
    jump_up_100,
    jump_up_200,
    jump_up_500,
    jump_up_1000,
	jump_up_5000,
	play_game_50,
	play_game_100,
	play_game_200,
    play_game_500,
    play_game_1000,
	stage1,
	stage2,
	stage3,
	stage4,
	stage5,
	stage6,
	stage7,
	stage8,
	stage9,
	stage10,
	stage11,
	stage12,
	stage13,
	stage14,
	stage15,
	stage16,
	stage17,
    stage18,
}

public enum eIAP
{
    UnlockAll = 0,
    NoAds,
	UnlockGame = 2,
}

//Helyettesítsük valóssal
public enum eEvent
{
    playOnLevel,
}

//Helyettesítsük valóssal
public enum ePageView
{
    test_pageview1,
    
}


public struct GCFriendProgress
{
	public Texture2D pic;
	public int progress;
}


public class PluginManager : MonoBehaviour
{
    public static Dictionary<eIAP, string> IAP_Ids = new Dictionary<eIAP, string> { };
    public static Dictionary<string, string> Ad_Ids = new Dictionary<string, string> { };

    public static Action OnConnectionOpened;
    public static Action OnConnectionClosed;

    public static SocialManager social;
    public static IAPManager iap;
    public static AdManager ads;
    public static AnalyticsManager analytics;

    public static PluginManager Instance = null;
    public static Action Init; 

#if UNITY_IPHONE
	public static string AdMob_Intersitial_AdUnit = "N/A";
	public static string[] IOS_IAPs = new string[] {  "com.east2west.impossiball.unlockall","com.east2west.impossiball.unlockgame"};
	public static string[] IOS_Achievements = new string[] { "com.east2west.impossiball.jump200",            //OK
		"com.east2west.impossiball.jump500",
		"com.east2west.impossiball.jump1000",
                                                             "com.east2west.impossiball.jump5000",
                                                             "com.east2west.impossiball.jump10000",
                                                             "com.east2west.impossiball.die50",              //OK
                                                             "com.east2west.impossiball.die100",
                                                             "com.east2west.impossiball.die200",
                                                             "com.east2west.impossiball.die500",
                                                             "com.east2west.impossiball.die1000",
                                                             "com.east2west.impossiball.overspikes10",
                                                             "com.east2west.impossiball.overspikes20",
                                                             "com.east2west.impossiball.overspikes50",
                                                             "com.east2west.impossiball.overspikes100",
                                                             "com.east2west.impossiball.overspikes500",
                                                             "com.east2west.impossiball.jumpup100",           //OK
                                                             "com.east2west.impossiball.jumpup200",
                                                             "com.east2west.impossiball.jumpup500",
                                                             "com.east2west.impossiball.jumpup1000",
															 "com.east2west.impossiball.jumpup5000",
															 "com.east2west.impossiball.play50",            //OK
															 "com.east2west.impossiball.play100",
															 "com.east2west.impossiball.play200",
															 "com.east2west.impossiball.play500",
															 "com.east2west.impossiball.play1000",
															 "com.east2west.impossiball.stage1",
															 "com.east2west.impossiball.stage2",
															 "com.east2west.impossiball.stage3",
															 "com.east2west.impossiball.stage4",
															 "com.east2west.impossiball.stage5",
															 "com.east2west.impossiball.stage6",
															 "com.east2west.impossiball.stage7",
															 "com.east2west.impossiball.stage8",
															 "com.east2west.impossiball.stage9",
															 "com.east2west.impossiball.stage10",
															 "com.east2west.impossiball.stage11",
															 "com.east2west.impossiball.stage12",
															 "com.east2west.impossiball.stage13",
															 "com.east2west.impossiball.stage14",
															 "com.east2west.impossiball.stage15",
															 "com.east2west.impossiball.stage16",
															 "com.east2west.impossiball.stage17",
							     							 "com.east2west.impossiball.stage18", };

	public static string[] IOS_Leaderboards = new string[] { "com.east2west.impossiball.attemps", "com.east2west.impossiball.dailychallenge", "com.east2west.impossiball.completedlevels" };

	public static string IOS_FlurryApiKey = "-";
    public static string IOS_UpsightsToken = "-";
    public static string IOS_UpsightsSecret = "-";
    public static string[] IOS_UpsightsAdvertPlacement = { "" };
    public static string IOS_UpsightsMoregamesPlacement = "more_games";
#endif

#if UNITY_ANDROID && GOOGLEPLAY
    public static string ChartboostAppID = "";
    public static string ChartboostSignature = "";
    public static string ANDROID_UpsightsToken = "-";
    public static string ANDROID_UpsightsSecret = "-";
    public static string[] ANDROID_UpsightsAdvertPlacement = { "" };
    public static string ANDROID_UpsightsMoregamesPlacement = "more_games";
    public static string AdMob_Intersitial_AdUnit = "-";
	public static string[] ANDROID_IAPs = new string[] { "com.invictus.impossiball.unlockall", "com.invictus.impossiball.noads"};
    public static string[] ANDROID_Achievements = new string[] { "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-",
                                                                 "-"};
    public static string[] ANDROID_Leaderboards = new string[] { "-", "-", "-" };
    public static string ANDROID_FlurryApiKey = "-";
    public static string ANDROID_PublicKey = "-";
#endif

    public static string[] KONG_Leaderboards = new string[] { "", "" };
    
    
	public List<GCFriendProgress> gcFriendProgresses;
	

    void Awake()
    {
        //DontDestroyOnLoad(this);
        //DontDestroyOnLoad(this.gameObject);
        Instance = this;

	//	#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE
        social = new SocialManager(new Dictionary<eSocialAdapter, ISocialAdapter> { {eSocialAdapter.Test, new SocialAdapterEditor()} });
        iap = new IAPManager(new IAPAdapterEditor());
        ads = new AdManager(new Dictionary<eAdNetwork, IADAdapter> { { eAdNetwork.Test, new AdAdapterEditor() } });
        analytics = new AnalyticsManager(new Dictionary<eAnalytics, IAnalyticsAdapter> { {eAnalytics.Test, new AnalyticsAdapterEditor() } });
        
//		#elif UNITY_IPHONE
//        social = new SocialManager(new Dictionary<eSocialAdapter, ISocialAdapter> { {eSocialAdapter.GameCenter, new SocialAdapterGameCenter()} });
//		iap = new IAPManager(new IAPAdapterIOS());
//		//ads = new AdManager(new Dictionary<eAdNetwork, IADAdapter> { { eAdNetwork.Playhaven, new AdAdapterUpsights() } });
//       	//analytics = new AnalyticsManager(new Dictionary<eAnalytics, IAnalyticsAdapter> {  {eAnalytics.Flurry, new AnalyticsAdapterFlurry() } });
//		social.SetDefaultAdapter(eSocialAdapter.GameCenter);
//		//FacebookAndroid.init (true);
//
//		#elif UNITY_ANDROID && GOOGLEPLAY
//        social = new SocialManager(new Dictionary<eSocialAdapter, ISocialAdapter> { { eSocialAdapter.GooglePlayServices, new SocialAdapterPlayServices() } });
//        iap = new IAPManager(new IAPAdapterGP());
//        ads = new AdManager(new Dictionary<eAdNetwork, IADAdapter> { { eAdNetwork.AdMob, new AdAdapterAdMob() }});
//        analytics = new AnalyticsManager(new Dictionary<eAnalytics, IAnalyticsAdapter> {  {eAnalytics.Flurry, new AnalyticsAdapterFlurry() } });
//        social.SetDefaultAdapter(eSocialAdapter.GooglePlayServices);
//		//FacebookAndroid.init (true);
//
	//	#endif
	    StopAllCoroutines ();
		lastReachability = Application.internetReachability;
		OnConnection ();
        StartCoroutine(CheckConnection());
        
        #if UNITY_ANDROID
		StartCoroutine (ImmersiveMode ());
		#endif

        #if UNITY_ANDROID
//        FacebookAndroid.init(true);
        #endif
        #if UNITY_IPHONE
       // FacebookBinding.init();
        #endif

       // Upsight.init(ANDROID_UpsightsToken, ANDROID_UpsightsSecret);
    }

    void Start()
    {
        if (Init != null)
            Init();
    }

	public bool ForceCheckConnection = false;
	private NetworkReachability lastReachability = NetworkReachability.NotReachable;

#if UNITY_ANDROID
	IEnumerator ImmersiveMode()
	{
		EtceteraAndroid.enableImmersiveMode (true);
		if (CGame.menuLayer != null)
			CGame.menuLayer.MainMenuGUI_RefreshScreenSize ();
		yield return new WaitForSeconds (2f);
		StartCoroutine (ImmersiveMode ());
	}
#endif

    IEnumerator CheckConnection()
    {
        yield return new WaitForSeconds(1.5f);

		if(lastReachability != Application.internetReachability)
		{
			//ForceCheckConnection = false;
			lastReachability = Application.internetReachability;
			OnConnection ();
		}
        StartCoroutine(CheckConnection());
    }

	private void OnConnection()
	{
		if (lastReachability != NetworkReachability.NotReachable)
		{
			if (OnConnectionOpened != null)
			{
				//Debug.Log ("OnConnectionOpened");
				OnConnectionOpened();
			}
		}
		else
		{
			if (OnConnectionClosed != null)
				OnConnectionClosed();
		}
	}
	
	private void OnDestroy()
	{
		StopAllCoroutines ();
	}
}
