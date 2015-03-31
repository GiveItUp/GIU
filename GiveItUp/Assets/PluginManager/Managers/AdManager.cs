using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AdManager 
{
    private float maxFrequency = 0f;
    private Dictionary<eAdNetwork, IADAdapter> adapters;

    public Action OnVideoStart;
    public Action OnVideoCancelled;
    public Action OnVideoFinished;
    public Action<int> OnVirtualCurrencyEarned;

    public AdManager(Dictionary<eAdNetwork, IADAdapter> adps)
    {
        adapters = adps;
    }

    public void SetMaxFrequency(float f)
    {
        maxFrequency = f;
    }

    public void CacheFullscreenAd(eAdNetwork adType)
    {
        if (adapters.ContainsKey(adType))
        {
            if (!adapters[adType].CacheFullscreenAd())
                Debug.LogWarning("Adapter " + adType.ToString() + " does not implement 'CacheFullscreenAd'!");
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");

    }

    public void CacheFullscreenAd()
    {
        foreach (var adapter in adapters.Values)
            if (adapter.CacheFullscreenAd())
                return;
        Debug.LogWarning("Not found any adapter which implement 'CacheFullscreenAd'!");
    }

    public void CacheFullscreenAd(eAdNetwork adType, string param)
    {
        if (adapters.ContainsKey(adType))
        {
            if (!adapters[adType].CacheFullscreenAd(param))
                Debug.LogWarning("Adapter " + adType.ToString() + " does not implement 'CacheFullscreenAd'!");
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
    }

    public void CacheFullscreenAd(string param)
    {
        foreach (var adapter in adapters.Values)
            if (adapter.CacheFullscreenAd(param))
                return;
        Debug.LogWarning("Not found any adapter which implement 'CacheFullscreenAd'!");
    }

    public void ShowFullscreenAd(eAdNetwork adType)
    {
		if (User.HasIAP_RemoveAds || User.IsPremium) return;

        if (adapters.ContainsKey(adType))
        {
            if (!adapters[adType].ShowFullscreenAd())
                Debug.LogWarning("Adapter " + adType.ToString() + " does not implement 'ShowFullscreenAd'!");
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
    }

    public void ShowFullscreenAd()
	{
		if (User.HasIAP_RemoveAds || User.IsPremium) return;

        foreach (var adapter in adapters.Values)
            if (adapter.ShowFullscreenAd())
                return;
        Debug.LogWarning("Not found any adapter which implement 'ShowFullscreenAd'!");
    }

    public void ShowFullscreenAd(eAdNetwork adType, string param)
	{
		if (User.HasIAP_RemoveAds || User.IsPremium) return;

        if (adapters.ContainsKey(adType))
        {
            if (!adapters[adType].ShowFullscreenAd(param))
                Debug.LogWarning("Adapter " + adType.ToString() + " does not implement 'ShowFullscreenAd'!");
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
    }

    public void ShowFullscreenAd(string param)
	{
		if (User.HasIAP_RemoveAds || User.IsPremium) return;

        foreach (var adapter in adapters.Values)
            if (adapter.ShowFullscreenAd(param))
                return;
        Debug.LogWarning("Not found any adapter which implement 'ShowFullscreenAd'!");
    }

    public void CacheMoreGames(eAdNetwork adType)
    {
        if (adapters.ContainsKey(adType))
        {
            if (!adapters[adType].CacheMoregames())
                Debug.LogWarning("Adapter " + adType.ToString() + " does not implement 'CacheMoreGames'!");
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
    }

    public void CacheMoreGames()
    {
        foreach (var adapter in adapters.Values)
            if (adapter.CacheMoregames())
                return;
        Debug.LogWarning("Not found any adapter which implement 'CacheMoreGames'!");
    }

    public void ShowMoreGames(eAdNetwork adType)
    {
        if (adapters.ContainsKey(adType))
        {
            if (!adapters[adType].ShowMoregames())
                Debug.LogWarning("Adapter " + adType.ToString() + " does not implement 'ShowMoreGames'!");
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
    }

    public void ShowMoreGames()
    {
        foreach (var adapter in adapters.Values)
            if (adapter.ShowMoregames())
                return;
        Debug.LogWarning("Not found any adapter which implement 'ShowMoreGames'!");
    }

    public bool IsVideoAdAvailable(eAdNetwork adType)
    {
        if (adapters.ContainsKey(adType))
        {
            return adapters[adType].IsVideoAdAvailable();
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
        return false;
    }

    public bool IsVideoAdAvailable()
    {
        foreach (var adapter in adapters.Values)
            if (adapter.IsVideoAdAvailable())
                return true;
        return false;
    }

    public bool IsOfferwallAvailable(eAdNetwork adType)
    {
        if (adapters.ContainsKey(adType))
        {
            return adapters[adType].IsOfferwallAvailable();
        }
        else
            Debug.LogError("Adapter " + adType.ToString() + " is not found!");
        return false;
    }

    public bool IsOfferwallAvailable()
    {
        foreach (var adapter in adapters.Values)
            if (adapter.IsOfferwallAvailable())
                return true;
        return false;
    }

    public void ShowVideoAd()
    {
        if (!IsVideoAdAvailable())
        {
            Debug.Log("Video ad is not available!");
            return;
        }
        foreach (var adapter in adapters.Values)
            if (adapter.IsVideoAdAvailable())
                adapter.ShowVideoAd();
    }

    public void ShowVideoAd(eAdNetwork adType)
    {
        if (!IsVideoAdAvailable(adType))
        {
            Debug.Log("Video ad is not available on adapter " + adType.ToString());
            return;
        }
        adapters[adType].ShowVideoAd();
    }

    public void ShowOfferwall()
    {
        if (!IsOfferwallAvailable())
        {
            Debug.Log("Offerwall is not available!");
            return;
        }
        foreach (var adapter in adapters.Values)
            if (adapter.IsOfferwallAvailable())
                adapter.ShowOfferwall();
    }

    public void ShowOfferwall(eAdNetwork adType)
    {
        if (!IsOfferwallAvailable(adType))
        {
            Debug.Log("Offerwall is not available on adapter " + adType.ToString());
            return;
        }
        adapters[adType].ShowOfferwall();
    }
}
