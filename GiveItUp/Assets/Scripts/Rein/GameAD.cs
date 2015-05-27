using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum East2WestGames
{
	Minigore2,
	ACS,
	DD,
	GrannySmith,
	Sprinkle,
	AntHill,
	GemLegend,
	TotalRecoil,
	ToTheEnd,
	RollerRally,
	MarcusLevel,
	GiveItUp,
	Count
}

public enum Channels
{
	Q360,
	MI,
	M4399,
	MM,
	WDJ,
	M91,
	DuoKu,
	MyApp,
	UC,
	Uni,
	Tele,
	Oppo,
	Gionee,
	Dcn,
	Vivo,
	Anzhi,
	Zte,
	Sogou,
	MHA,
	Huawei,
	Kugou,
	XiaoLajiao,
	IOS,
	Count
}

public class GameAD : MonoBehaviour
{
	public Channels channel;
	public static Channels currentChannel;
	public Texture[] gameAds;
	//public string[] gameSites;
	public Btn linkBtn;
	public Btn closeBtn;
	East2WestGames game;
	string site = "http://east2west.cn/#游戏作品";
	Dictionary<East2WestGames,Dictionary<Channels,string>> gameDicts = new Dictionary<East2WestGames, Dictionary<Channels,string>> ();
	//Dictionary<Channels,string> gameSiteDicts=new Dictionary<Channels, string>();
	int index=0;
	string eventKey;
	void Start ()
	{
		channel = currentChannel;
		gameDicts.Add (East2WestGames.Minigore2, new Dictionary<Channels, string> ());
		gameDicts [East2WestGames.Minigore2].Add (Channels.Q360, "http://zhushou.360.cn/detail/index/soft_id/1556893");
		gameDicts [East2WestGames.Minigore2].Add (Channels.MM, "http://mm.10086.cn/android/info/300002838540");
		gameDicts [East2WestGames.Minigore2].Add (Channels.MI, "http://app.mi.com/detail/55757");
		gameDicts [East2WestGames.Minigore2].Add (Channels.M4399, "http://a.4399.cn/game-id-34432.html");
		gameDicts [East2WestGames.Minigore2].Add (Channels.WDJ, "http://www.wandoujia.com/apps/net.mountainsheep.minigore2");
		gameDicts [East2WestGames.Minigore2].Add (Channels.Oppo, "http://store.oppomobile.com/product/0000/599/429_1.html");
		gameDicts [East2WestGames.Minigore2].Add (Channels.Gionee, "http://game.gionee.com/Front/Game/detail/?from=gn&id=3614");
		gameDicts [East2WestGames.Minigore2].Add (Channels.MyApp, "");
	

		gameDicts.Add (East2WestGames.ACS, new Dictionary<Channels, string> ());
		gameDicts [East2WestGames.ACS].Add (Channels.Q360, "http://zhushou.360.cn/detail/index/soft_id/1582757");
		gameDicts [East2WestGames.ACS].Add (Channels.MM, "");
		gameDicts [East2WestGames.ACS].Add (Channels.MI, "");
		gameDicts [East2WestGames.ACS].Add (Channels.M4399, "http://a.4399.cn/game-id-36609.html");
		gameDicts [East2WestGames.ACS].Add (Channels.WDJ, "http://www.wandoujia.com/apps/com.noodlecake.anothercasesolved");
		gameDicts [East2WestGames.ACS].Add (Channels.Oppo, "http://store.oppomobile.com/product/0000/545/983_1.html");
		gameDicts [East2WestGames.ACS].Add (Channels.Gionee, "http://game.gionee.com/Front/Search/detail/?from=baidu&id=6868291");
		gameDicts [East2WestGames.ACS].Add (Channels.MyApp, "http://android.myapp.com/myapp/detail.htm?apkName=com.noodlecake.anothercasesolved");

		gameDicts.Add (East2WestGames.DD, new Dictionary<Channels, string> ());
		gameDicts [East2WestGames.DD].Add (Channels.Q360, "http://zhushou.360.cn/detail/index/soft_id/1902905");
		gameDicts [East2WestGames.DD].Add (Channels.MM, "");
		gameDicts [East2WestGames.DD].Add (Channels.MI, "");
		gameDicts [East2WestGames.DD].Add (Channels.M4399, "http://a.4399.cn/game-id-41307.html");
		gameDicts [East2WestGames.DD].Add (Channels.WDJ, "http://www.wandoujia.com/apps/com.noodlecake.deviousdungeon");
		gameDicts [East2WestGames.DD].Add (Channels.Oppo, "http://store.nearme.com.cn/product/0000/552/645_1.html");
		gameDicts [East2WestGames.DD].Add (Channels.Gionee, "");
		gameDicts [East2WestGames.DD].Add (Channels.MyApp, "");
	

		switch (PlayerPrefs.GetString ("GameADParam", "Random")) {
		case "Random":
			game=(East2WestGames)(UnityEngine.Random.Range(0,gameAds.Length));
			break;
		case "Sequential":
			game=(East2WestGames)Mathf.Clamp(index,0,gameAds.Length);
			break;
		case "Custom":
			game=(East2WestGames)Mathf.Clamp(int.Parse(PlayerPrefs.GetString("GameADCustomParam","0")),0,gameAds.Length);
			break;
		default:
			game=East2WestGames.Minigore2;
			break;
		}

		eventKey = "Visit"+game.ToString ();

		GetComponent<Renderer>().material.mainTexture = gameAds [(int)game];

		site = gameDicts[game][channel];
		linkBtn.Init (OnLinkBtn);
		closeBtn.Init (OnCloseBtn);
	}

	void OnLinkBtn ()
	{
		Application.OpenURL (site);
//		GA.Event (eventKey);
		index++;
		if (index >= gameAds.Length) {
			index=0;
		}
	}

	void OnCloseBtn ()
	{
		gameObject.SetActive (false);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.JoystickButton1)) {
			OnCloseBtn();
		}
	}

}
