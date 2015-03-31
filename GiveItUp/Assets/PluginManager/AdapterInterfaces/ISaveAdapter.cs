using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ISaveAdapter 
{
    protected Hashtable saveData;

    abstract public void SetInt(string path, int value, bool saveToLocal = false, bool saveToCloud = false);
    abstract public void SetFloat(string path, float value, bool saveToLocal = false, bool saveToCloud = false);
    abstract public void SetString(string path, string value, bool saveToLocal = false, bool saveToCloud = false);
    abstract public int GetInt(string path);
    abstract public float GetFloat(string path);
    abstract public string GetString(string path);
    abstract public void LoadLocal(bool mergeCurrent = false, Dictionary<string, eMergeType> config = null);
    abstract public void LoadCloud(bool mergeCurrent = false, Dictionary<string, eMergeType> config = null);
    abstract public void SaveLocal();
    abstract public void SaveCloud();
}
