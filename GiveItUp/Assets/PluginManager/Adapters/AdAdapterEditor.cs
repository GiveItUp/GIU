using UnityEngine;
using System.Collections;

public class AdAdapterEditor : IADAdapter 
{
    private bool initied = false;

    public override void Init()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PluginManager.OnConnectionOpened += Init;
        }
        PluginManager.OnConnectionOpened -= Init;

        adNetwork = eAdNetwork.Test;
        initied = true;
    }

    public override bool IsVideoAdAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;
        return true;
    }

    public override void ShowVideoAd()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;
        PluginManager.Instance.StartCoroutine(IE_TEST_ShowVideoAd());   
    }

    private IEnumerator IE_TEST_ShowVideoAd()
    {
        OnVideoAdStart();
        Debug.Log("VideoAd is started!");
        yield return new WaitForSeconds(4f);
        OnVideoAdFinished();
        Debug.Log("VideoAd is ended!");
    }

    public override bool IsOfferwallAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;
        return true;
    }

    public override void ShowOfferwall()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;
        OnVirtualCurrencyEarned(10);
    }

    public override bool CacheFullscreenAd()
    {
        Debug.Log("Fullscreen Ad is cached!");
        return true;
    }

    public override bool CacheMoregames()
    {
        Debug.Log("MoreGames is cached!");
        return true;
    }

    public override bool ShowFullscreenAd()
    {
        Debug.Log("Fullscreen Ad is shown!");
        return true;
    }

    public override bool ShowMoregames()
    {
        Debug.Log("MoreGames is shown!");
        return true;
    }
}
