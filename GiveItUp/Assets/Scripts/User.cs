using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class User 
{
	#if PREMIUM_APP
	public const bool PREMIUM = true;
	#else
	public const bool PREMIUM = false;
	#endif

	public static int ActualStage = 0;

	public static string Version = "";
	
	public static bool HasIAP_RemoveAds = false;
	public static bool HasIAP_UnlockAll = false;

	public static bool IsSoundEnabled = true;
	public static bool IsVoiceEnabled = true;

	public static bool IsPremium = PREMIUM;
	
	public static List<int> levelScores = new List<int>();
	public static List<float> levelRecords = new List<float>();
	public static List<int> levelTries = new List<int>();
	public static int actualRandomLevelTries = 0;
	public static float actualRandomLevelRecord = 0;
	public static int actualRandomLevelScore = 100;
	public static DateTime lastRandomLevelDate;
	
	public static int dailyChallengeLeaderboardScore = 0;
	
	public static bool _isClearance = false;
	public static bool _isToChangePackage = false;
	static User()
	{
		Load();
	}

	#region Methods
	
	public static DateTime GetLastRandomLevelDate( )
	{
		return lastRandomLevelDate;
	}
	
	public static void SetLastRandomLevelDate( DateTime date)
	{
		if (LevelEditor.testmode)
			return;
		///if (levelScores.Count == level)
		//	levelScores.Add(score);
		
		lastRandomLevelDate = date;
		Save();
	}	
	
	public static int GetLevelScore(int level)
	{
		if (level == -1 )
		{
			return actualRandomLevelScore;
		}
	
		return levelScores[level];
	}
	
	public static void SetLevelScore(int level, int score)
	{
		if (LevelEditor.testmode)
			return;
		///if (levelScores.Count == level)
		//	levelScores.Add(score);
		
		if (level == -1)
		{
			actualRandomLevelScore = Mathf.Max(actualRandomLevelScore, score);
			Save ();
			return;
		}

		levelScores[level] = Mathf.Max(levelScores[level], score);
		Save();
	}

	public static float GetLevelRecord(int level)
	{
		if ( level == -1)
		{
			return actualRandomLevelRecord;
		}
	
		return levelRecords[level];
	}
	
	public static void SetLevelRecord(int level, float score)
	{
		if (LevelEditor.testmode)
			return;
		///if (levelScores.Count == level)
		//	levelScores.Add(score);
		
		if (level == -1)
		{
			actualRandomLevelRecord = Mathf.Max( actualRandomLevelRecord, score);
			Save ();
			return;
		}
		
		levelRecords[level] = Mathf.Max(levelRecords[level], score);
		Save();
	}

	public static int GetLevelTries(int level)
	{
		if (level == -1)
		{
			return actualRandomLevelTries;
		}
	
		return levelTries[level];
	}
	
	public static void AddLevelTries(int level)
	{
		if (LevelEditor.testmode)
			return;
			
		if (level == -1)
		{
			actualRandomLevelTries++;
			Save();
			return;
		}
		
		levelTries [level]++;
		Save();
	}
	
	public static void CompleteStage(int _stageIndex)
	{
		if (_stageIndex >= Storage.Instance._worlds.Count || _stageIndex >= CGame.LEVEL_COUNT || (GetLevelScore(_stageIndex) >= 100 && _stageIndex < ActualStage) || LevelEditor.testmode)
			return;
		
		if (_stageIndex >= CGame.LEVEL_COUNT - 1) {
			_isClearance = true;
			SetLevelScore (_stageIndex, 100);
		} else {
			SetLevelScore (_stageIndex, 100);
			_isClearance = false;
			ActualStage++;
			LastPlayedStage = ActualStage;
		}

		Save();
	}
	#endregion

	#region General Methods

    private static Hashtable aData = new Hashtable();

    public static int PlayCount
    {
        get { return aData.Contains("PlayCount") ? (int)aData["PlayCount"] : 0; }
        set { aData["PlayCount"] = value; }
    }

    public static int JumpCount
    {
        get { return aData.Contains("JumpCount") ? (int)aData["JumpCount"] : 0; }
        set { aData["JumpCount"] = value; }
    }

    public static int JumpUpCount
    {
        get { return aData.Contains("JumpUpCount") ? (int)aData["JumpUpCount"] : 0; }
		set { aData["JumpUpCount"] = value; }
    }

    public static int DieCount
    {
        get { return aData.Contains("DieCount") ? (int)aData["DieCount"] : 0; }
        set { aData["DieCount"] = value; }
	}

	public static int JumpOverSpikeCount
	{
        get { return aData.Contains("JumpOverSpikeCount") ? (int)aData["JumpOverSpikeCount"] : 0; }
        set { aData["JumpOverSpikeCount"] = value; }
	}

    public static void LoadAData()
    {
        aData["PlayCount"] = PlayerPrefs.GetInt("PlayCount", 0);
        aData["JumpCount"] = PlayerPrefs.GetInt("JumpCount", 0);
        aData["JumpUpCount"] = PlayerPrefs.GetInt("JumpUpCount", 0);
        aData["DieCount"] = PlayerPrefs.GetInt("DieCount", 0);
        aData["JumpOverSpikeCount"] = PlayerPrefs.GetInt("JumpOverSpikeCount", 0);
    }

    public static void SaveAData()
    {
        foreach (var data in aData.Keys)
            PlayerPrefs.SetInt((string)data, (int)aData[data]);
    }

	public static int LastPlayedStage
	{		
		get { return PlayerPrefs.GetInt("LastPlayedStage", 0); }
		set { PlayerPrefs.SetInt("LastPlayedStage", value); }
	}
	
	public static bool NeedRateNotififaction
	{
		get { return PlayerPrefs.GetInt("NeedRateNotififaction", 0) == 1; }
		set { PlayerPrefs.SetInt("NeedRateNotififaction", value ? 1 : 0); }
	}

	public static void Reset()
	{
		PlayerPrefs.DeleteAll ();
		ActualStage = 0;
		Version = "";
		levelScores = new List<int>();
		levelRecords = new List<float>();
		levelTries = new List<int>();
		actualRandomLevelTries = 0;
		actualRandomLevelRecord = 0;
		actualRandomLevelScore = 100;
		HasIAP_RemoveAds = false;
		HasIAP_UnlockAll = false;
		IsSoundEnabled = true;
		IsVoiceEnabled = true;
		IsPremium = PREMIUM;

		LastPlayedStage = 0;
		
		Save ();
		Load ();

		CGame.Instance.RefreshMute ();
		CGame.menuLayer.ShowMainMenuGUI ();
	}
	
	public static void Load()
	{
		//Debug.Log("userload");
		string str_UserData = PlayerPrefs.GetString("saveData", "nodata");
//		Debug.Log("str_UserData = " + str_UserData);

		Hashtable ht_userData = null;
		if (str_UserData == "nodata")
		{
			ht_userData = new Hashtable();
		}
		else
		{
			ht_userData = MiniJSON_Old.JsonDecode(str_UserData) as Hashtable;
		}

		LoadFromHashtable (ht_userData);
        LoadAData();
	}
	
	public static void Save()
	{
		string str = MiniJSON_Old.JsonEncode(User.ToHashtable());
		//Debug.Log("str = " + str);
		PlayerPrefs.SetString("saveData", str);
	}
	
	private static void LoadFromHashtable(Hashtable ht)
	{
		ActualStage = ht["AS"] != null ? int.Parse(ht["AS"].ToString()) : 0;

		Version = ht["VS"] != null ? ht["VS"].ToString() : "";
		
		levelScores = new List<int>();
		for(int i = 0; i < Storage.Instance._worlds.Count && i < CGame.LEVEL_COUNT; i++)
			levelScores.Add(0);

		string str_scores = ht["HS2"] != null ? ht["HS2"].ToString() : null;
		if(str_scores != null)
		{
	        ArrayList al_scores = (ArrayList)MiniJSON_Old.JsonDecode(str_scores);
			for(int i = 0; i < al_scores.Count && i < CGame.LEVEL_COUNT; i++)
	            levelScores[i] = int.Parse((al_scores[i].ToString()));
		}
		else
		{
			ActualStage = 0;
			LastPlayedStage = 0;
		}
		
		levelTries = new List<int>();
		for(int i = 0; i < Storage.Instance._worlds.Count && i < CGame.LEVEL_COUNT; i++)
			levelTries.Add(0);
		
		string str_tries = ht["HT2"] != null ? ht["HT2"].ToString() : null;
		if(str_tries != null)
		{
			ArrayList al_tries = (ArrayList)MiniJSON_Old.JsonDecode(str_tries);
			for(int i = 0; i < al_tries.Count && i < CGame.LEVEL_COUNT; i++)
				levelTries[i] = int.Parse((al_tries[i].ToString()));
		}

		levelRecords = new List<float>();
		for(int i = 0; i < Storage.Instance._worlds.Count && i < CGame.LEVEL_COUNT; i++)
			levelRecords.Add(0);
			
		actualRandomLevelTries = ht["ARLT"] != null ? int.Parse(ht["ARLT"].ToString()) : 0;
		
		string str_records = ht["HR2"] != null ? ht["HR2"].ToString() : null;
		if(str_records != null)
		{
			ArrayList al_records = (ArrayList)MiniJSON_Old.JsonDecode(str_records);
			for(int i = 0; i < al_records.Count && i < CGame.LEVEL_COUNT; i++)
				levelRecords[i] = float.Parse((al_records[i].ToString()));
		}
		
		actualRandomLevelRecord = ht["ARLR"] != null ? float.Parse(ht["ARLR"].ToString()) : 0;
		
		actualRandomLevelScore = ht["ARLS"] != null ? int.Parse(ht["ARLS"].ToString()) : 100;
		
		lastRandomLevelDate = ht["LRLD"] != null ? DateTime.Parse(ht["LRLD"].ToString()) : new DateTime(1950, 1, 1);


		HasIAP_RemoveAds = ht["HIR"] != null ? int.Parse(ht["HIR"].ToString()) == 1 : false;
		HasIAP_UnlockAll = ht["HIU"] != null ? int.Parse(ht["HIU"].ToString()) == 1 : false;

		IsSoundEnabled = ht["ISE"] != null ? int.Parse(ht["ISE"].ToString()) == 1 : true;
		IsVoiceEnabled = ht["IVE"] != null ? int.Parse(ht["IVE"].ToString()) == 1 : true;

		IsPremium = ht["IP"] != null ? PREMIUM || int.Parse(ht["IP"].ToString()) == 1 : PREMIUM;
	}
	
	private static Hashtable ToHashtable()
	{
		Hashtable ret = new Hashtable ();
		
		ret ["AS"] = ActualStage.ToString();
		ret ["VS"] = Version;
		
		ArrayList al_scores = new ArrayList();
		foreach (var score in levelScores)
			al_scores.Add(score.ToString());
			
		ret ["ARLT"] = actualRandomLevelTries.ToString();
		ret ["ARLR"] = actualRandomLevelRecord.ToString();
		ret ["ARLS"] = actualRandomLevelScore.ToString();
		
		ret ["LRLD"] = lastRandomLevelDate.ToString();
		
		ArrayList al_tries = new ArrayList();
		foreach (var tries in levelTries)
			al_tries.Add(tries.ToString());

		ArrayList al_records = new ArrayList();
		foreach (var records in levelRecords)
			al_records.Add(records.ToString());
		
		ret ["HS2"] = MiniJSON_Old.JsonEncode(al_scores);
		ret ["HT2"] = MiniJSON_Old.JsonEncode(al_tries);
		ret ["HR2"] = MiniJSON_Old.JsonEncode(al_records);
		ret ["HIR"] = HasIAP_RemoveAds ? "1" : "0";
		ret ["HIU"] = HasIAP_UnlockAll ? "1" : "0";
		ret ["ISE"] = IsSoundEnabled ? "1" : "0";
		ret ["IVE"] = IsVoiceEnabled ? "1" : "0";

		ret ["IP"] = IsPremium ? "1" : "0";

		return ret;
	}
	#endregion
}
