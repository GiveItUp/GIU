using UnityEngine;
using System.Collections;

public class GfxSettings {
	private static GfxSettings instance;
	
	public static GfxSettings Instance()
	{
		if(instance == null)
			instance = new GfxSettings();
		return instance;
	}
	
	public GfxSettings() {}
	
	public bool IsRetinaDisplay()
	{
		return false;	
//		#if UNITY_IPHONE
//		if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad3Gen)
//			return true;
//		#endif
//		return false;
	}


    public int GetScreenWidth()
    {
        int w = (int)(768f / (float)Screen.height * (float)Screen.width);
        return w;
    }

    public int GetScreenHeight()
    {
        int h = 768;
        return h;
    }

    public int GetScreenRealWidth()
    {
        if (IsRetinaDisplay())
        {
            return (int)(Screen.width / 2.0f);
        }
        return Screen.width;
    }
    public int GetScreenRealHeight()
    {
        if (IsRetinaDisplay())
        {
            return (int)(Screen.height / 2.0f);
        }
        return Screen.height;
    }
}
