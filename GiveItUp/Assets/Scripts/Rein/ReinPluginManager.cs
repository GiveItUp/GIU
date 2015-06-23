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
			#if UNITY_ANDROID		
			int channleId = 0;//0没有后缀的包名，1.baidu 2.anzhi 3.baofeng 4.lenovo 5.leshi 6.iqiyi
			switch(channleId){
			case 0:
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL0");
				}
//				User._isToChangePackage = false;
				break;
			case 1://
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.baidu.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL1");
				}
//				User._isToChangePackage = true;
				break;
			case 2://
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.anzhi.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL2");
				}
//				User._isToChangePackage = true;
				break;
			case 3://
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.baofeng.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL3");
				}
//				User._isToChangePackage = true;
				break;
			case 4://
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.lenovo.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL4");
				}
//				User._isToChangePackage = true;
				break;
			case 5://
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.leshi.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL5");
				}
//				User._isToChangePackage = true;
				break;
			case 6://
				using (var pluginClass = new AndroidJavaClass( "com.east2west.octopus.iqiyi.MainActivity" )) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
					//_plugin.Call("Login");
					Debug.Log ("PLUGIN INITIAL5");
				}
//				User._isToChangePackage = true;
				break;
			}
			#endif

	}

	public static void ShowAds ()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			#if UNITY_ANDROID
				if((UmengInitializer._showAdChance != 0)){
					if(!User.HasIAP_RemoveAds){
						int rand = Random.Range(0, 100);
						Debug.Log("rand:" + rand + "-Chance" + UmengInitializer._showAdChance);
						if(rand < UmengInitializer._showAdChance){
							Debug.Log("rand < UmengInitializer._showAdChance");
							_plugin.Call ("ShowAds");
						}
					}
				}
			}
			#endif
	}

	public static void Purchase (string pid)
	{
		Debug.Log ("purchase item=" + pid);
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			#if UNITY_ANDROID
			_plugin.Call ("Buy", pid);
			#endif
		}
	}

	public static void GetBaiduChannel()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			#if UNITY_ANDROID
			Debug.Log ("GetBaiduChannel");
			_plugin.Call ("GetBaiduChannel");
			#endif
		}
	}
	public static void ExitGame ()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			#if UNITY_ANDROID
			Debug.Log ("ExitGame");
			_plugin.Call ("ExitGame");
			#endif
		}
	}
	
	public static void Quit ()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			#if UNITY_ANDROID
			Debug.Log ("Quit");
			_plugin.Call ("Quit");
			#endif
		}
	}
	public static void ShowLeaderboards(string s){		
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			#if UNITY_ANDROID
			Debug.Log ("ShowLeaderboards" + s);
			_plugin.Call ("ShowLeaderboards", s);
			#endif
		}
	}
//	//积分广告
//	//显示积分广告
//	public static void ShowOffersAds(){
//		if (Application.platform != RuntimePlatform.WindowsEditor) {
//			if(UmengInitializer._showPointsAdChance != 0){
//				#if UNITY_ANDROID
//				_plugin.Call ("ShowOffersAds");
//				#endif
//			}
//		}
//	}
//	//查询积分
//	public static void QueryPoints(){
//		if (Application.platform != RuntimePlatform.WindowsEditor) {
//			if(UmengInitializer._showPointsAdChance != 0){
//				#if UNITY_ANDROID
//				_plugin.Call ("QueryPoints");
//				#endif
//			}
//		}
//	}
//	//扣除积分
//	public static void SpendPoints(){
//		if (Application.platform != RuntimePlatform.WindowsEditor) {
//			if(UmengInitializer._showPointsAdChance != 0){
//				#if UNITY_ANDROID
//				_plugin.Call ("SpendPoints", User.offersPoints);
//				#endif
//			}
//		}
//	}
//	//增加积分
//	public static void AwardPoints(int amount){
//		if (Application.platform != RuntimePlatform.WindowsEditor) {			
//			if(UmengInitializer._showPointsAdChance != 0){
//				#if UNITY_ANDROID
//				_plugin.Call ("AwardPoints", amount);
//				#endif
//			}
//		}
//	}
//	//视频广告
//	//请求视频
//	public static void RequestVideoAd(){
//		if (Application.platform != RuntimePlatform.WindowsEditor) {
//			if(UmengInitializer._showPointsAdChance != 0){
//				#if UNITY_ANDROID
//				_plugin.Call ("RequestVideoAd");
//				#endif
//			}
//		}
//	}
//	//显示视频广告
//	public static void ShowVideoAds(){
//		if (Application.platform != RuntimePlatform.WindowsEditor) {
//			if(UmengInitializer._showPointsAdChance != 0){
//				#if UNITY_ANDROID
//				_plugin.Call ("showVideoAds");
//				#endif
//			}
//		}
//	}
}
