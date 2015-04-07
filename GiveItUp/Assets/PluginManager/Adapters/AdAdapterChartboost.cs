///*Requires Prime 31 Chartboost Unity SDK*/
//#if UNITY_ANDROID
//using UnityEngine;
//using System.Collections;
//
//using CBManager = ChartboostAndroidManager;
//using CB = ChartboostAndroid;
//
//public class AdAdapterChartboost : IADAdapter 
//{
//    private bool initied = false;
//	private bool isVisible = false;
//
//    public override void Init()
//    {
//		PluginManager.OnConnectionOpened -= Init;
//        if (Application.internetReachability == NetworkReachability.NotReachable)
//        {
//            PluginManager.OnConnectionOpened += Init;
//			return;
//        }
//
//		CBManager.didFinishInterstitialEvent += (string arg1, string arg2) => { OnVideoAdFinished(); isVisible = false; } ;
//		CBManager.didFinishRewardedVideoEvent += (string loc, string arg2) =>  { CB.cacheRewardedVideo(loc); OnVideoAdFinished(); isVisible = false; };
//        CBManager.didFinishMoreAppsEvent += (string arg1, string arg2) => { isVisible = false; };
//        CBManager.didCompleteRewardedVideoEvent += (int obj) => { OnVirtualCurrencyEarned(500); OnVideoAdFinished(); isVisible = false; };
//		adNetwork = eAdNetwork.Chartboost;
//
//        CB.init (PluginManager.ChartboostAppID, PluginManager.ChartboostSignature, true);
//		CB.setImpressionsUseActivities (true);
//		initied = true;	
//
//    }
//
//    public override bool IsVideoAdAvailable()
//    {
//		if (!initied)
//			return false;
//
//		if (!CB.hasCachedRewardedVideo("RewardedVideo")) 
//		{
//			CB.cacheRewardedVideo("RewardedVideo");
//			return false;
//		}
//		return CB.hasCachedRewardedVideo("RewardedVideo");
//    }
//
//    public override void ShowVideoAd()
//    {
//		if (!initied)
//			return;
//
//		isVisible = true;
//		OnVideoAdStart();
//		CB.setImpressionsUseActivities (true);
//		CB.showRewardedVideo ("RewardedVideo");
//    }
//
//    public override bool IsOfferwallAvailable()
//    {
//        return false;
//    }
//
//    public override void ShowOfferwall()
//    {
//        return;
//    }
//
//    public override bool CacheFullscreenAd()
//    {
//		if (!initied)
//			return false;
//
//        if (CB.hasCachedMoreApps())
//            return true;
//		CB.cacheInterstitial("MoreGames");
//        return false;
//    }
//
//    public override bool CacheFullscreenAd(string param)
//    {
//		if (!initied)
//			return false;
//
//		CB.cacheInterstitial (param);
//        return true;
//    }
//
//    public override bool CacheMoregames()
//    {
//		if (!initied)
//			return false;
//
//		if (!CB.hasCachedMoreApps("Default"))
//			CB.cacheMoreApps("Default");
//        return true;
//    }
//
//    public override bool ShowFullscreenAd()
//    {
//		if (!initied)
//			return false;
//
//        if (CB.hasCachedInterstitial("Default"))
//        {
//            CB.setImpressionsUseActivities(true);
//            OnVideoAdStart();
//            CB.showInterstitial("Default");
//            isVisible = true;
//        }
//        else
//        {
//
//            CB.cacheInterstitial("Default");
//            return false;
//        }
//        return true;
//    }
//
//    public override bool ShowFullscreenAd(string param)
//    {
//		if (!initied)
//			return false;
//
//		if (CB.hasCachedInterstitial(param)) 
//		{
//			OnVideoAdStart();
//			CB.setImpressionsUseActivities (true);
//			CB.showInterstitial (param);
//            isVisible = true;
//		}
//		else 
//		{
//			CB.cacheInterstitial (param);
//			return false;
//		}
//        return true;
//    }
//
//    public override bool ShowMoregames()
//    {
//		if (!initied)
//			return false;
//
//		CB.setImpressionsUseActivities (true);
//		CB.showMoreApps ("MoreGames");
//        isVisible = true;
//        return true;
//    }
//}
//#endif