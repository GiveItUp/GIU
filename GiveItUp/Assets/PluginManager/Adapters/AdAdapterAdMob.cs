///* This script requires GoogleAdMob plugin. https://github.com/googleads/googleads-mobile-plugins */
//
//#if UNITY_ANDROID
//using UnityEngine;
//using System.Collections;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
//
//
//public class AdAdapterAdMob : IADAdapter 
//{
//    private BannerView bannerView;
//    private InterstitialAd interstitial;
//
//    public override void Init()
//    {
//        if (interstitial != null) return;
//        interstitial = new InterstitialAd(PluginManager.AdMob_Intersitial_AdUnit);
//        interstitial.AdOpened += (object sender, System.EventArgs args) => { interstitial = null; };
//        interstitial.LoadAd(createAdRequest());
//        return;
//    }
//
//    public override bool CacheFullscreenAd()
//    {
//        if (interstitial != null) return false;
//        interstitial = new InterstitialAd(PluginManager.AdMob_Intersitial_AdUnit);
//        interstitial.AdOpened += (object sender, System.EventArgs args) => { interstitial = null; };
//        interstitial.LoadAd(createAdRequest());
//        return true;
//    }
//
//    public override bool CacheFullscreenAd(string param)
//    {
//        return CacheFullscreenAd();
//    }
//
//    public override bool ShowFullscreenAd()
//    {
//        if (!interstitial.IsLoaded())
//            return false;
//        interstitial.Show();
//        return true;
//    }
//
//    public override bool ShowFullscreenAd(string param)
//    {
//        return ShowFullscreenAd();
//    }
//
//    private AdRequest createAdRequest()
//    {
//        return new AdRequest.Builder()
//                .AddTestDevice(AdRequest.TestDeviceSimulator)
//                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
//                .AddKeyword("game")
//                .SetGender(Gender.Male)
//                .SetBirthday(new System.DateTime(1985, 1, 1))
//                .TagForChildDirectedTreatment(false)
//                .AddExtra("color_bg", "9B30FF")
//                .Build();
//    }
//}
//
//#endif