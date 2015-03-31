using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public class UmengInitializer : MonoBehaviour
{
	public Channels channel;
	//public GameAD ad;
	Dictionary<East2WestGames,string> umengIdDict = new Dictionary<East2WestGames, string> ();
	
	void Start ()
	{
		umengIdDict.Add (East2WestGames.GiveItUp, "5503ffc6fd98c58388000b77");
		GA.StartWithAppKeyAndChannelId (umengIdDict [East2WestGames.GiveItUp], channel.ToString());
//		ad.channel = channel;
//		if (PlayerPrefs.GetInt ("TVMode", 0) == 0) {
//			GA.UpdateOnlineConfig ();
//			Debug.Log (GA.GetConfigParamForKey ("GameAD"));
//			PlayerPrefs.SetString ("GameAD", GA.GetConfigParamForKey ("GameAD"));
//			PlayerPrefs.SetString ("NormalAD", GA.GetConfigParamForKey ("NormalAD"));
//			PlayerPrefs.SetString ("GameADParam", GA.GetConfigParamForKey ("GameADParam"));
//			PlayerPrefs.SetString ("GameADCustomParam", GA.GetConfigParamForKey ("GameADCustomParam"));
//		}

//		if(GA.GetConfigParamForKey ("GameAD")=="1"){
//			ad.gameObject.SetActive(true);
//		}
	}

}
