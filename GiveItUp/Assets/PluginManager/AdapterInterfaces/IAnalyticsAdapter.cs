using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAnalyticsAdapter 
{
    protected bool errorTracking = false;

    public IAnalyticsAdapter()
    {
        Init();
    }

    virtual public void Init()
    {
    }

    virtual public void SetErrorTracking(bool enable)
    {
        errorTracking = enable;
    }

    virtual public void TrackEvent(eEvent e, bool timed = false)
    {
    }

    virtual public void TrackEvent(eEvent e, Dictionary<string, string> par, bool timed = false)
    {
    }

    virtual public void PageView(ePageView p)
    {
    }
}
