using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using System;
using UnityEngine.UI;

public class Wechat : MonoBehaviour
{
	void Start ()
	{
		ShareSDK.setCallbackObjectName (this.name);
		ShareSDK.open ("api20");
	}

	public void Share ()
	{
		Hashtable content = new Hashtable ();
		content ["content"] = "章鱼！永不言弃！！";
		content ["image"] = "http://i-2.yxdown.com/2014/12/18/KHgyNjAp/8cf68b4c-96bf-4d44-89ef-ce84c3b287b6.jpeg";
		ShareResultEvent evt = new ShareResultEvent (ShareResultHandler);
		ShareSDK.shareContent (PlatformType.WeChatTimeline, content, evt);
	}

	void ShareResultHandler (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
	{
		if (state == ResponseState.Success) {
			print ("share result :");
			print (MiniJSON.jsonEncode (shareInfo));
		} else if (state == ResponseState.Fail) {
			print ("fail! error code = " + error ["error_code"] + "; error msg = " + error ["error_msg"]);
		}
	}
}
