using UnityEngine;
using System.Collections;

public class AnalyticsAdapterEditor : IAnalyticsAdapter 
{
    private bool initied = false;
    
    public override void Init()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PluginManager.OnConnectionOpened += Init;
            return;
        }
        PluginManager.OnConnectionOpened -= Init;
        initied = true;
    }

    public override void PageView(ePageView p)
    {
        //Debug.Log("Pageview " + p.ToString());
    }

    public override void TrackEvent(eEvent e, bool timed = false)
    {
        //Debug.Log("EventLog " + e.ToString() + " timed: " + timed.ToString());
    }

    public override void TrackEvent(eEvent e, System.Collections.Generic.Dictionary<string, string> par, bool timed = false)
    {
        //Debug.Log("EventLog " + e.ToString() + " timed: " + timed.ToString());
    }
}
