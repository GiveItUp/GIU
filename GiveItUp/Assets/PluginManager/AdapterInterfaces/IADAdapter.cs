using UnityEngine;
using System.Collections;

public abstract class IADAdapter
{
    protected eAdNetwork adNetwork;
    public IADAdapter()
    {
        Init();
    }

    abstract public void Init();

    virtual public bool ShowFullscreenAd()
    {
        return false;
    }

    virtual public bool ShowFullscreenAd(string param)
    {
        return false;
    }

    virtual public bool CacheFullscreenAd()
    {
        return false;
    }

    virtual public bool CacheFullscreenAd(string param)
    {
        return false;
    }

    virtual public bool ShowMoregames()
    {
        return false;
    }

    virtual public bool CacheMoregames()
    {
        return false;
    }

    virtual public void ShowVideoAd()
    {
    }

    virtual public void CacheVideoAd()
    {
    }

    virtual public void ShowOfferwall()
    {
    }

    virtual public bool IsOfferwallAvailable()
    {
        return false;
    }

    virtual public bool IsVideoAdAvailable()
    {
        return false;
    }

    protected void OnVideoAdStart()
    {
        if (PluginManager.ads.OnVideoStart != null)
            PluginManager.ads.OnVideoStart();
    }

    protected void OnVideoAdCancelled()
    {
        if (PluginManager.ads.OnVideoCancelled != null)
            PluginManager.ads.OnVideoCancelled();
    }

    protected void OnVideoAdFinished()
    {
        if (PluginManager.ads.OnVideoFinished != null)
            PluginManager.ads.OnVideoFinished();
    }

    protected void OnVirtualCurrencyEarned(int amount)
    {
        if (PluginManager.ads.OnVirtualCurrencyEarned != null)
            PluginManager.ads.OnVirtualCurrencyEarned(amount);
    }
}
