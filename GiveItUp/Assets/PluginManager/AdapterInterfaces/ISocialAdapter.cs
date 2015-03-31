using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocialFriend
{
    public SocialFriend() { }
    public SocialFriend(string name, string id, Texture2D photo)
    {
        this.name = name;
        this.id = id;
        this.photo = photo;
    }
    public string name { get; set;}
    public string id {get; set;}
    public Texture2D photo {get; set;}
    public string url { get; set; }
}

public class Achievement
{
    public Achievement(string title, string description, string id, float progress)
    {
        this.title = title;
        this.description = description;
        this.id = id;
        this.progress = progress;
        isIncremental = false;
        maxSteps = 1;
    }

    public Achievement(string title, string description, string id, float progress, int maxSteps)
    {
        this.title = title;
        this.description = description;
        this.id = id;
        this.progress = progress;
        isIncremental = false;
        this.maxSteps = maxSteps;
    }


    public string title {get; set;}
    public string description {get; set;}
    public string id {get; set;}
    public float progress {get; set;}
    public bool isIncremental { get; set; }
    public int maxSteps { get; set; }
}

public class Score
{
    public Score(Leaderboard leaderBoard, SocialFriend friend, long score, long rank)
    {
        this.leaderBoard = leaderBoard;
        this.friend = friend;
        this.score = score;
        this.rank = rank;
    }
    public Leaderboard leaderBoard { get; set; }
    public SocialFriend friend {get; set; }
    public long score {get; set;}
    public long rank {get; set;}
}

public class Leaderboard
{
    public Leaderboard(string id, string title, List<Score> scores)
    {
        this.id = id;
        this.title = title;
        this.scores = scores;
    }
    public string id {get; set;}
    public string title {get; set;}
    public List<Score> scores {get; set;}
}

public abstract class ISocialAdapter
{
    protected Dictionary<eLeaderboard, string> leaderboard_Ids;
    protected Dictionary<eAchievement, string> achievement_Ids;
    protected Dictionary<string, string> login_Ids;

    public class FeatureMatrix
    {
        public FeatureMatrix (bool cf, bool a, bool l, bool s, bool n)
        {
            CHALLANGE_FRIENDS = cf;
            ACHIEVEMENTS = a;
            LEADERBOARDS = l;
            SHARE = s;
        }

        public bool CHALLANGE_FRIENDS = false;
        public bool ACHIEVEMENTS = false;
        public bool LEADERBOARDS = false;
        public bool SHARE = false;
        public bool NOTIFICATION = false;
    }

    protected eSocialAdapter socialType = eSocialAdapter.Test;
    protected FeatureMatrix featureMatrix; 

    virtual public void Init()
    {
        socialType = eSocialAdapter.Test;
        featureMatrix = new FeatureMatrix(false, false, false, false, false);
    }

    abstract public SocialFriend GetMe();

    public void ApplyLeaderboardIds(Dictionary<eLeaderboard, string> lIds)
    {
        leaderboard_Ids = lIds;
    }

    public void ApplyAchievementIds(Dictionary<eAchievement, string> aIds)
    {
        achievement_Ids = aIds;
    }

    public void ApplyLoginIds(Dictionary<string, string> lIds)
    {
        login_Ids = lIds;
    }

    public FeatureMatrix GetFeatureMatrix()
    {
        return featureMatrix;
    }

    //Social
    abstract public void Login();
    abstract public void Logout();
    abstract public void LoadFriends();
    abstract public string GetID();

    virtual public void ShowLeaderboards()
    {
        if (!featureMatrix.LEADERBOARDS)
            OnLoadLeaderboardsFailed("Feature 'LEADERBOARDS' is not available!");
    }

    virtual public void ShowAchievements()
    {
        if (!featureMatrix.LEADERBOARDS)
            OnLoadLeaderboardsFailed("Feature 'LEADERBOARDS' is not available!");
    }
    
    virtual public void Share(Dictionary<string, string> pars, Texture2D pic = null)
    {
        if (!featureMatrix.SHARE)
            OnShareFailed("Feature 'SHARE' is not available!");
    }

    abstract public List<SocialFriend> GetFriends();

    virtual public void SendNotification(List<SocialFriend> to, Dictionary<string, string> pars)
    {
        if (!featureMatrix.NOTIFICATION)
            Debug.LogWarning("Feature 'NOTIFICATION' is not available!");
    }

    abstract public bool IsLoggedIn();
    abstract public bool IsFriendsLoaded();

    //Gaming
    virtual public void LoadLeaderboards()
    {
        if (!featureMatrix.LEADERBOARDS)
            OnLoadLeaderboardsFailed("Feature 'LEADERBOARDS' is not available!");
    }

    virtual public void LoadAchievements()
    {
        if (!featureMatrix.ACHIEVEMENTS)
            OnLoadAchievementsFailed("Feature 'ACHIEVEMENTS' is not available!");
    }

    virtual public void SubmitScore(eLeaderboard leaderboard, long score)
    {
        if (!featureMatrix.LEADERBOARDS)
            Debug.LogWarning("Feature 'LEADERBOARDS' is not available!");
    }

    virtual public void ReportAchievement(eAchievement achievement, float progress)
    {
        if (!featureMatrix.ACHIEVEMENTS)
            Debug.LogWarning("Feature 'ACHIEVEMENTS' is not available!");
    }

    virtual public void ChallangeFriends(List<SocialFriend> friends, eLeaderboard leaderBoard, long score, string msg)
    {
        if (!featureMatrix.CHALLANGE_FRIENDS)
            Debug.LogWarning("Feature 'CHALLANGE_FRIENDS' is not available!");
    }

    virtual public List<Achievement> GetAchievements()
    {
        if (!featureMatrix.ACHIEVEMENTS)
            Debug.LogWarning("Feature 'ACHIEVEMENTS' is not available!");
        return null;
    }

    virtual public List<Leaderboard> GetLeaderboards()
    {
        if (!featureMatrix.LEADERBOARDS)
            Debug.LogWarning("Feature 'LEADERBOARDS' is not available!");
        return null;
    }

    virtual public List<Score> GetScores(eLeaderboard leaderBoard)
    {
        if (!featureMatrix.LEADERBOARDS)
            Debug.LogWarning("Feature 'LEADERBOARDS' is not available!");
        return null;
    }

    //Handlers
    protected void OnLoggedIn()
    {
        if (PluginManager.social.OnLoggedIn != null)
            PluginManager.social.OnLoggedIn(socialType);
    }

    protected void OnLogInFailed(string error)
    {
        if (PluginManager.social.OnLogInFailed != null)
            PluginManager.social.OnLogInFailed(socialType, error);
    }

    protected void OnLoggedOut()
    {
        if (PluginManager.social.OnLoggedOut != null)
            PluginManager.social.OnLoggedOut(socialType);
    }

    protected void OnLogOutFailed(string error)
    {
        if (PluginManager.social.OnLogOutFailed != null)
            PluginManager.social.OnLogOutFailed(socialType, error);
    }

    protected void OnLoadedFriends()
    {
        if (PluginManager.social.OnLoadedFriends != null)
            PluginManager.social.OnLoadedFriends(socialType);
    }

    protected void OnLoadFriendsFailed(string error)
    {
        if (PluginManager.social.OnLoadFriendsFailed != null)
            PluginManager.social.OnLoadFriendsFailed(socialType, error);
    }

    protected void OnLoadedAchievements()
    {
        if (PluginManager.social.OnLoadedAchievements != null)
            PluginManager.social.OnLoadedAchievements(socialType);
    }

    protected void OnLoadAchievementsFailed(string error)
    {
        if (PluginManager.social.OnLoadAchievementsFailed != null)
            PluginManager.social.OnLoadAchievementsFailed(socialType, error);
    }

    protected void OnLoadedLeaderboards()
    {
        if (PluginManager.social.OnLoadedLeaderboards != null)
            PluginManager.social.OnLoadedLeaderboards(socialType);
    }

    protected void OnLoadLeaderboardsFailed(string error)
    {
        if (PluginManager.social.OnLoadLeaderboardsFailed != null)
            PluginManager.social.OnLoadLeaderboardsFailed(socialType, error);
    }

    protected void OnShared()
    {
        if (PluginManager.social.OnShared != null)
            PluginManager.social.OnShared(socialType);
    }

    protected void OnShareFailed(string error)
    {
        if (PluginManager.social.OnShareFailed != null)
            PluginManager.social.OnShareFailed(socialType, error);
    }

    protected void OnNotificationCompleted(string result)
    {
        if (PluginManager.social.OnNotification != null)
            PluginManager.social.OnNotification(socialType, result);
    }
}
