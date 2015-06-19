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

	public void Share (int score, int stageIndex, int tryCount)
	{
		ShareSDK.close();
		string str = "";
		if(score != 100){
			str = "第" + tryCount + "次尝试仍然还在第" + (stageIndex + 1) + "关被虐，必须砸手机！";
		}else{
			str = "第" + tryCount + "次尝试总算搞定了第" + (stageIndex + 1) + "关，不服来战！";
		}

		Hashtable content = new Hashtable ();
		content ["content"] = str;
		content ["image"] = (PlayerPrefs.GetString("ballName") == "Go_PinkBall")?"http://547849.user-website5.com/roger/0.jpg":"http://547849.user-website5.com/roger/1.jpg";
		content["title"] = "章鱼！永不言弃！！";
		content["description"] = "章鱼！永不言弃！！";
		content["url"] = "http://www.monstar-lab.com";
		content ["type"] = Convert.ToString ((int)contentType);
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
	ContentType contentType = ContentType.App;
	void OnGUI()
	{
		if(GUI.Button(new Rect(0,0,100,100),"App"))
		{
			contentType = ContentType.App;
		}
		if(GUI.Button(new Rect(0,100,100,100),"News"))
		{
			contentType = ContentType.News;
		}
		if(GUI.Button(new Rect(0,200,100,100),"Image"))
		{
			contentType = ContentType.Image;
		}
	}
}
