using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using System;
using UnityEngine.UI;

public class Wechat : MonoBehaviour
{
	void Start ()
	{
		ShareSDK.setCallbackObjectName ("Main Camera");
		ShareSDK.open ("6794e84148ae");

#if UNITY_IOS
		Hashtable wechatConf = new Hashtable ();
		wechatConf.Add ("app_key", "wxb1fcb260fce35b69");
		wechatConf.Add ("app_secret", "793a7c610a116d52b8649e7248ce3706");
		ShareSDK.setPlatformConfig (PlatformType.WeChatSession, wechatConf);
		ShareSDK.setPlatformConfig (PlatformType.WeChatTimeline, wechatConf);
		ShareSDK.setPlatformConfig (PlatformType.WeChatFav, wechatConf);
#endif
	}

	public void Share ()
	{
		Hashtable content = new Hashtable ();
		content ["content"] = "章鱼！章鱼！章鱼！永不言弃！！永不言弃！！永不言弃！！";
		content ["image"] = "http://i-2.yxdown.com/2014/12/18/KHgyNjAp/8cf68b4c-96bf-4d44-89ef-ce84c3b287b6.jpeg";
		content["title"] = "章鱼！永不言弃！！";
		content["description"] = "章鱼！永不言弃！！章鱼！永不言弃！！章鱼！永不言弃！！";
		content["url"] = "http://www.monstar-lab.com";
		content ["type"] = Convert.ToString ((int)ContentType.News);
		ShareResultEvent evt = new ShareResultEvent (ShareResultHandler);
		ShareSDK.shareContent (PlatformType.WeChatTimeline, content, evt);
	}

	void ShareResultHandler (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
	{
		if (state == ResponseState.Success) {
			print ("share result :");
			print (com.monstar.MiniJSON.jsonEncode (shareInfo));
		} else if (state == ResponseState.Fail) {
			print ("fail! error code = " + error ["error_code"] + "; error msg = " + error ["error_msg"]);
		}
	}
}
