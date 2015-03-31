using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum eMergeType
{
    Replace,
    HigherIsBetter,
    LowerIsBetter,
}

public class SaveManager 
{
    public Action OnCloudDataLoaded;
    public Action OnCloudDataFailed;

    private ISaveAdapter adapter;

    public SaveManager(bool loadLocal = true, bool loadCloud = true, Dictionary<string, eMergeType> config = null)
    {
        if (loadLocal)
            LoadLocal(config != null, config);
        if (loadCloud)
            LoadCloud(config != null, config);
    }

    public void SetInt(string path, int value, bool saveToLocal = false, bool saveToCloud = false)
    {
        if (adapter != null)
            adapter.SetInt(path, value, saveToLocal, saveToCloud);
    }

    public void SetFloat(string path, float value, bool saveToLocal = false, bool saveToCloud = false)
    {
        if (adapter != null)
            adapter.SetFloat(path, value, saveToLocal, saveToCloud);
    }

    public void SetString(string path, string value, bool saveToLocal = false, bool saveToCloud = false)
    {
        if (adapter != null)
            adapter.SetString(path, value, saveToLocal, saveToCloud);
    }

    public int GetInt(string path)
    {
        if (adapter != null)
            return adapter.GetInt(path);
        return 0;
    }

    public float GetFloat(string path)
    {
        if (adapter != null)
            return adapter.GetFloat(path);
        return 0f;
    }

    public string GetString(string path)
    {
        if (adapter != null)
            return adapter.GetString(path);
        return "";
    }

    public void LoadLocal(bool mergeCurrent = false, Dictionary<string, eMergeType> config = null)
    {
        if (adapter != null)
            adapter.LoadLocal(mergeCurrent, config);
    }

    public void LoadCloud(bool mergeCurrent = false, Dictionary<string, eMergeType> config = null)
    {
        if (adapter != null)
            adapter.LoadCloud(mergeCurrent, config);
    }

    public void SaveLocal()
    {
        if (adapter != null)
            adapter.SaveLocal();
    }

    public void SaveCloud()
    {
        if (adapter != null)
            adapter.SaveCloud();
    }


}
