using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocialAdapterEditor : ISocialAdapter
{
    private bool loggedin = false;
    private bool initied = false;
    private List<SocialFriend> friends = null;
    private List<Achievement> achievements = null;
    private List<Leaderboard> leaderboards = null;
    private SocialFriend me = null;


    public override void Init()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable || initied)
        {
            PluginManager.OnConnectionOpened += Init;
            return;
        }
        PluginManager.OnConnectionOpened -= Init;
        base.Init();
        featureMatrix.ACHIEVEMENTS = true;
        featureMatrix.LEADERBOARDS = true;
        featureMatrix.NOTIFICATION = true;
        featureMatrix.SHARE = true;
        initied = true;
        if (PlayerPrefs.GetInt("SocialLoggedIn", 0) == 1)
            Login();
    }

    public override void Login()
    {
        if (!initied)
        {
            Debug.LogError("Not initied!");
            return;
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            loggedin = false;
            OnLogInFailed("No Connection");
            return;
        }
        loggedin = true;
        me = new SocialFriend("player_me", SystemInfo.deviceUniqueIdentifier, null);
        PluginManager.Instance.StartCoroutine(IE_OnLoggedIn());
        PlayerPrefs.SetInt("SocialLoggedIn", 1);
 	}

    IEnumerator IE_OnLoggedIn()
    {
        yield return new WaitForSeconds(1f);
        OnLoggedIn();
    }

    public override SocialFriend GetMe()
    {
        if (!loggedin)
        {
            Debug.Log("Not logged in!");
            return null;
        }
        return me;
    }

    public override void Logout()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnLogOutFailed("No Connection!");
            return;
        }
        loggedin = false;
        OnLoggedOut();
        PlayerPrefs.SetInt("SocialLoggedIn", 0);
    }

    public override string GetID()
    {
        if (loggedin)
            return SystemInfo.deviceUniqueIdentifier;
        Debug.LogError("Not logged in!");
        return null;
    }

    public override void LoadFriends()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnLoadFriendsFailed("No Connection!");
            return;
        }
        
        if (!loggedin)
        {
            Debug.LogError("Not logged in!");
            return;
        }

        friends = new List<SocialFriend>();
        friends.Add(new SocialFriend("Jani", "453245234523", null));
        friends.Add(new SocialFriend("Zoli", "345345334673", null));
        friends.Add(new SocialFriend("Juli", "234345396ghg", null));
        friends.Add(new SocialFriend("Anna", "453453535643", null));
        OnLoadedFriends();
    }

    public override List<SocialFriend> GetFriends()
    {
        return friends;
    }

    public override void LoadAchievements()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnLoadAchievementsFailed("No Connection!");
            return;
        }

        if (!loggedin)
        {
            Debug.LogError("Not logged in!");
            return;
        }

        base.LoadAchievements();
        achievements = new List<Achievement>();

        for (int i = 0; i < System.Enum.GetValues(typeof(eAchievement)).Length; i++)
            achievements.Add(new Achievement("ach" + i.ToString(), "desc" + i.ToString(), "zgdfgd" + i.ToString(), Random.Range(0f, 100f)));
        
        OnLoadedAchievements();
    }

    public override List<Achievement> GetAchievements()
    {
        return achievements;
    }

    public override void LoadLeaderboards()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnLoadLeaderboardsFailed("No Connection!");
            return;
        }

        if (!loggedin)
        {
            Debug.LogError("Not logged in!");
            return;
        }

        base.LoadLeaderboards();
        leaderboards = new List<Leaderboard>();

        for (int i = 0; i < System.Enum.GetValues(typeof(eLeaderboard)).Length; i++)
        {
            leaderboards.Add(new Leaderboard("sdfsafasdfas" + i.ToString(), "leaderboard" + i.ToString(), null));
            leaderboards[i].scores = new List<Score> { new Score(leaderboards[i], friends[0], 10000, 1), new Score(leaderboards[i], friends[1], 1000, 2) };
        }
       
        OnLoadedLeaderboards();
    }

    public override List<Leaderboard> GetLeaderboards()
    {
        return leaderboards;
    }

    public override bool IsLoggedIn()
    {
        return loggedin;
    }

    public override bool IsFriendsLoaded()
    {
        return friends != null;
    }

    public override List<Score> GetScores(eLeaderboard leaderBoard)
    {
        if (leaderboards == null)
        {
            Debug.LogError("Leaderboards are not loaded!");
            return null;
        }

        return leaderboards[(int)leaderBoard].scores;
    }

    public override void Share(Dictionary<string, string> pars, Texture2D pic = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnShareFailed("No Connection!");
            return;
        }

        if (!loggedin)
        {
            Debug.LogError("Not logged in!");
            return;
        }
        OnShared();
    }
}
