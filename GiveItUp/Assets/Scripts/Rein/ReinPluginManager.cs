using UnityEngine;
using System.Collections;
using Umeng;

public class ReinPluginManager : MonoBehaviour
{
	bool isTestVersion = false;
	#if UNITY_ANDROID
	public static AndroidJavaObject _plugin;
	#endif
	
	void Awake ()
	{
		if (PlayerPrefs.GetInt ("DebugMode", 0) != 1) {
			#if UNITY_ANDROID
			using (var pluginClass = new AndroidJavaClass( "com.mondotv.marcuslevel.MainActivity" )) {
				_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
				//_plugin.Call("Login");
				Debug.Log ("PLUGIN INITIAL");
			}
			#endif
		}

		//shareSDK = this.GetComponent<cn.sharesdk.unity3d.ShareSDK>();
	}

//	void Start ()
//	{
//		Debug.Log ("InitShareSDK");
//		cn.sharesdk.unity3d.ShareSDK.setCallbackObjectName ("Main Camera");
//		cn.sharesdk.unity3d.ShareSDK.open ("api20");//41b6e293d624
//		
//		//Sina Weibo
//		Hashtable sinaWeiboConf = new Hashtable ();
//		sinaWeiboConf.Add ("app_key", "568898243");
//		sinaWeiboConf.Add ("app_secret", "38a4f8204cc784f81f9f0daaf31e02e3");
//		sinaWeiboConf.Add ("redirect_uri", "http://www.sharesdk.cn");
//		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.SinaWeibo, sinaWeiboConf);
//		
//		//WeChat
//		Hashtable wcConf = new Hashtable ();
//		wcConf.Add ("app_id", "wx03593366d89d4e53");
//		//wcConf.Add ("app_secret", "5dd8b29d9c019cf0400f8b2e301080b4");
//		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.WeChatSession, wcConf);
//		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.WeChatTimeline, wcConf);
//		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.WeChatFav, wcConf);
//	}

	public static void ShowAds ()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			if (PlayerPrefs.GetInt ("DebugMode", 0) != 1) {
				#if UNITY_ANDROID
				_plugin.Call ("ShowAds");
				#endif
			}
		}
		Debug.Log ("ShowAds");
	}

	public static void Purchase (string pid)
	{
		Debug.Log ("purchase item=" + pid);
		#if UNITY_ANDROID
				_plugin.Call ("Buy", pid);
		#endif
	}
}
