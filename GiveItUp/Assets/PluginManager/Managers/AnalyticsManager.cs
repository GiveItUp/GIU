using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnalyticsManager 
{
    private Dictionary<eAnalytics, IAnalyticsAdapter> adapters;

    public AnalyticsManager(Dictionary<eAnalytics, IAnalyticsAdapter> adps)
    {
        adapters = adps;
    }

    public void TrackEvent(eAnalytics anType, eEvent e, bool timed = false)
    {
        if (adapters.ContainsKey(anType))
            adapters[anType].TrackEvent(e, timed);
        else
            Debug.LogError("Adapter " + anType.ToString() + " is not found!");
    }

    public void TrackEvent(eEvent e, bool timed = false)
    {
        foreach (var adapter in adapters.Values)
            adapter.TrackEvent(e, timed);
    }

    public void TrackEvent(eAnalytics anType, eEvent e, Dictionary<string, string> param, bool timed = false)
    {
        if (adapters.ContainsKey(anType))
            adapters[anType].TrackEvent(e, param, timed);
        else
            Debug.LogError("Adapter " + anType.ToString() + " is not found!");
    }

    public void TrackEvent(eEvent e, Dictionary<string, string> param, bool timed = false)
    {
        foreach (var adapter in adapters.Values)
            adapter.TrackEvent(e, param, timed);
    }

    public void PageView(eAnalytics anType, ePageView p)
    {
        if (adapters.ContainsKey(anType))
            adapters[anType].PageView(p);
        else
            Debug.LogError("Adapter " + anType.ToString() + " is not found!");
    }

    public void PageView(ePageView p)
    {
        foreach (var adapter in adapters.Values)
            adapter.PageView(p);
    }

    public void SetErrorTracking(eAnalytics anType, bool enabled)
    {
        if (adapters.ContainsKey(anType))
            adapters[anType].SetErrorTracking(enabled);
        else
            Debug.LogError("Adapter " + anType.ToString() + " is not found!");
    }

    public void SetErrorTracking(bool enabled)
    {
        foreach (var adapter in adapters.Values)
            adapter.SetErrorTracking(enabled);
    }
}
