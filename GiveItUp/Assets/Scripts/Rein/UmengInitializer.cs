using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public class UmengInitializer : MonoBehaviour
{
	public Channels channel;

	public static int _showAdChance = 0;//广告数字为0关闭非0为百分比
	public static int _showPointsAdChance = 1;//积分广告数字为比例x：1
	public static bool _isShowIap = false;//内购类型true硬计费false软计费
	public static bool _isShowGameAd = false;//内推广告

	public static bool _is360 = false;

//	private string strChanceAd = {"360AD","MiAD","4399AD"};
	Dictionary<East2WestGames,string> umengIdDict = new Dictionary<East2WestGames, string> ();

	void Start ()
	{
		umengIdDict.Add (East2WestGames.GiveItUp, "54eae9dbfd98c5a8770006db");
		GA.StartWithAppKeyAndChannelId (umengIdDict [East2WestGames.GiveItUp], channel.ToString());
//		ad.channel = channel;
		if (PlayerPrefs.GetInt ("TVMode", 0) == 0) {
			GA.UpdateOnlineConfig ();
			/***********************************************插屏广告部分*************************************************/
			string strChance360 = GA.GetConfigParamForKey ("360AD");
			string strChanceXiaomi = GA.GetConfigParamForKey ("MiAD");
			string strChance4399 = GA.GetConfigParamForKey ("4399AD");
			string strChanceHuawei = GA.GetConfigParamForKey ("HuaweiAD");
			string strChanceOppo = GA.GetConfigParamForKey ("OppoAD");
			string strChanceYouku = GA.GetConfigParamForKey ("YoukuAD");
			string strChanceJinli = GA.GetConfigParamForKey ("JinliAD");
			string strChanceAnzhi = GA.GetConfigParamForKey ("AnzhiAD");
			string strChance = GA.GetConfigParamForKey ("AD");
			if(channel.ToString() == "Q360" && strChance360 != "" && strChance360 != "0"){	//360
				_showAdChance = int.Parse(strChance360);
			}
			else if(channel.ToString() == "MI" && strChanceXiaomi != "" && strChanceXiaomi != "0"){	//小米
				_showAdChance = int.Parse(strChanceXiaomi);
			}
			else if(channel.ToString() == "M4399" && strChance4399 != "" && strChance4399 != "0"){	//4399
				_showAdChance = int.Parse(strChance4399);
			}
			else if(channel.ToString() == "Huawei" && strChanceHuawei != "" && strChanceHuawei != "0"){	//华为
				_showAdChance = int.Parse(strChanceHuawei);
			}
			else if(channel.ToString() == "Oppo" && strChanceOppo != "" && strChanceOppo != "0"){	//oppo
				_showAdChance = int.Parse(strChanceOppo);
			}
			else if(channel.ToString() == "Youku" && strChanceYouku != "" && strChanceYouku != "0"){	//优酷
				_showAdChance = int.Parse(strChanceYouku);
			}
			else if(channel.ToString() == "Jinli" && strChanceJinli != "" && strChanceJinli != "0"){	//金立
				_showAdChance = int.Parse(strChanceJinli);
			}
			else if(channel.ToString() == "Anzhi" && strChanceAnzhi != "" && strChanceAnzhi != "0"){	//安智
				_showAdChance = int.Parse(strChanceAnzhi);
			}
			else if(strChance != "" && strChance != "0"){//其他有广告渠道
				_showAdChance = int.Parse(strChance);
			}
			/***********************************************积分广告部分*************************************************/
			string strPointsChanceXiaomi = GA.GetConfigParamForKey ("MiADPoints");
			string strPointsChance4399 = GA.GetConfigParamForKey ("4399ADPoints");
			string strPointsChanceHuawei = GA.GetConfigParamForKey ("HuaweiADPoints");
			string strPointsChanceOppo = GA.GetConfigParamForKey ("OppoADPoints");
			string strPointsChanceYouku = GA.GetConfigParamForKey ("YoukuADPoints");
			string strPointsChanceJinli = GA.GetConfigParamForKey ("JinliADPoints");
			string strPointsChanceAnzhi = GA.GetConfigParamForKey ("AnzhiADPoints");
			string strPointsChance = GA.GetConfigParamForKey ("ADPoints");
			_is360 = false;
			if(channel.ToString() == "Q360"){	//360
				Debug.Log("360");
				_is360 = true;
				_showPointsAdChance = 0;
			}
			else if(channel.ToString() == "MI" && strPointsChanceXiaomi != "" && strPointsChanceXiaomi != "0"){	//小米
				Debug.Log("mi");
				_showPointsAdChance = int.Parse(strPointsChanceXiaomi);
			}
			else if(channel.ToString() == "M4399" && strPointsChance4399 != "" && strPointsChance4399 != "0"){	//4399
				Debug.Log("4399");
				_showPointsAdChance = int.Parse(strPointsChance4399);
			}
			else if(channel.ToString() == "Huawei" && strPointsChanceHuawei != "" && strPointsChanceHuawei != "0"){	//华为
				Debug.Log("huawei");
				_showPointsAdChance = int.Parse(strPointsChanceHuawei);
			}
			else if(channel.ToString() == "Oppo" && strPointsChanceOppo != "" && strPointsChanceOppo != "0"){	//oppo
				Debug.Log("oppo");
				_showPointsAdChance = int.Parse(strPointsChanceOppo);
			}
			else if(channel.ToString() == "Youku" && strPointsChanceYouku != "" && strPointsChanceYouku != "0"){	//优酷
				Debug.Log("youku");
				_showPointsAdChance = int.Parse(strPointsChanceYouku);
			}
			else if(channel.ToString() == "Jinli" && strPointsChanceJinli != "" && strPointsChanceJinli != "0"){	//金立
				Debug.Log("jinli");
				_showPointsAdChance = int.Parse(strPointsChanceJinli);
			}
			else if(channel.ToString() == "Anzhi" && strPointsChanceAnzhi != "" && strPointsChanceAnzhi != "0"){	//安智
				Debug.Log("anzhi");
				_showPointsAdChance = int.Parse(strPointsChanceAnzhi);
			}
			else if(strPointsChance != "" && strPointsChance != "0"){//其他有广告渠道
				Debug.Log("other");
				_showPointsAdChance = int.Parse(strPointsChance);
			}
			/***********************************************视频广告部分*************************************************/			
			/*************************************************内购部分**************************************************/
			if((GA.GetConfigParamForKey ("360IAP") == "1" && channel.ToString() == "Q360") ||	//360
			   (GA.GetConfigParamForKey ("MiIAP") == "1" && channel.ToString() == "MI") ||		//小米
			   (GA.GetConfigParamForKey ("4399IAP") == "1" && channel.ToString() == "M4399") ||	//4399
			   (GA.GetConfigParamForKey ("HuaweiIAP") == "1" && channel.ToString() == "Huawei") ||//华为
			   (GA.GetConfigParamForKey ("OppoIAP") == "1" && channel.ToString() == "Oppo") ||	//oppo
			   (GA.GetConfigParamForKey ("YoukuIAP") == "1" && channel.ToString() == "Youku") ||//优酷
			   (GA.GetConfigParamForKey ("JinliIAP") == "1" && channel.ToString() == "Jinli") ||//金立
			   (GA.GetConfigParamForKey ("AnzhiIAP") == "1" && channel.ToString() == "Anzhi") ||//安智
				GA.GetConfigParamForKey ("IAP") == "1"){
				_isShowIap = true;
			}	
			/*************************************************内推部分**************************************************/			
			if(GA.GetConfigParamForKey ("GameAD") == "1"){
				_isShowGameAd = true;
			}
		}
	}

//	public static int GetCoinRewardValue(){
//		PlayerPrefs.SetString ("CoinReward", GA.GetConfigParamForKey ("CoinReward"));
//		string v = PlayerPrefs.GetString ("CoinReward");
//		if (v != "") {
//						int value = int.Parse (v);
//						if (value != 0) {
//								coinReward = value;	
//						}
//				}
//		return coinReward;
//	}

}
