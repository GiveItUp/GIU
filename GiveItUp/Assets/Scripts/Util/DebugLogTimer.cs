using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugLogTimer {

    private List<float> times = new List<float>();

    public DebugLogTimer()
    {
        times = new List<float>();
    }

    public void Add()
    {
        times.Add(Time.realtimeSinceStartup);
    }

    public void Log()
    {
        for (int i = 0; i < times.Count - 1; i++)
        {
            Debug.Log("time[" + i + "] = " + (times[i + 1] - times[i]));
        }
    }
}
