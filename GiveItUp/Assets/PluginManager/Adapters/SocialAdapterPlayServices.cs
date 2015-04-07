//#if UNITY_ANDROID
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class SocialAdapterPlayServices : ISocialAdapter 
//{
//    private SocialFriend me;
//    private List<SocialFriend> friends;
//    private List<Achievement> achievements;
//    private List<Leaderboard> leaderboards;
//
//    
//    private int picDownloaded = 0;
//
//    public override void Init()
//    {
//        socialType = eSocialAdapter.GooglePlayServices;
//        featureMatrix = new FeatureMatrix(false, true, true, true, false);
//        GPGManager.authenticationSucceededEvent += OnAuthSuccess;
//        GPGManager.authenticationFailedEvent += OnLogInFailed;
//        GPGManager.userSignedOutEvent += OnLoggedOut;
//        GPGManager.loadScoresSucceededEvent += OnScoresLoaded;
//        GPGManager.loadScoresFailedEvent += (string s, string s2) => { OnLoadLeaderboardsFailed(s); };
//        if (PlayerPrefs.GetInt("FirstLaunchGPS", 0) == 0)
//        {
//            PlayerPrefs.SetInt("FirstLaunchGPS", 1);
//            PlayGameServices.authenticate();
//        }
//        else
//            PlayGameServices.attemptSilentAuthentication();
//    }
//
//    public override void Login()
//    {
//        PlayGameServices.authenticate();
//    }
//
//    public override void ShowLeaderboards()
//    {
//        if (PlayGameServices.isSignedIn())
//            PlayGameServices.showLeaderboards();
//        else
//            PlayGameServices.authenticate();
//    }
//
//    public override void ShowAchievements()
//    {
//        PlayGameServices.showAchievements();
//    }
//
//    public override void Logout()
//    {
//        PlayGameServices.signOut();
//    }
//
//    public override bool IsFriendsLoaded()
//    {
//        return friends != null;
//    }
//
//    public override List<SocialFriend> GetFriends()
//    {
//        return friends;
//    }
//
//    public override string GetID()
//    {
//        if (me != null)
//            return me.id;
//        return null;
//    }
//
//    public override bool IsLoggedIn()
//    {
//        return me != null;
//    }
//
//    public override List<Achievement> GetAchievements()
//    {
//        return achievements;
//    }
//
//    public override List<Leaderboard> GetLeaderboards()
//    {
//        return leaderboards;
//    }
//
//    public override SocialFriend GetMe()
//    {
//        return me;
//    }
//
//    public override List<Score> GetScores(eLeaderboard leaderBoard)
//    {
//        if (leaderboards == null)
//            return null;
//        if (leaderboards.Count <= (int)leaderBoard)
//            return null;
//        return leaderboards[(int)leaderBoard].scores;
//    }
//
//    public override void LoadAchievements()
//    {
//        OnGPGMetadataLoaded(PlayGameServices.getAllAchievementMetadata());
//    }
//
//    public override void LoadFriends()
//    {
//        //Mivez ezzel együtt töltődik be csak...
//        LoadLeaderboards();
//    }
//
//    public override void LoadLeaderboards()
//    {
//        OnGPGLeaderboards(PlayGameServices.getAllLeaderboardMetadata());
//    }
//
//    public override void SubmitScore(eLeaderboard leaderboard, long score)
//    {
//        if (PlayGameServices.isSignedIn())
//            PlayGameServices.submitScore(PluginManager.ANDROID_Leaderboards[(int)leaderboard], score);
//    }
//
//    public override void ReportAchievement(eAchievement achievement, float progress)
//    {
//        return;
//        if (PlayGameServices.isSignedIn())
//        {
//      
//            if (achievements[(int)achievement].isIncremental)
//            {
//                int steps = (int)(progress * achievements[(int)achievement].maxSteps - achievements[(int)achievement].progress * achievements[(int)achievement].maxSteps);
//                PlayGameServices.incrementAchievement(PluginManager.ANDROID_Achievements[(int)achievement], steps);
//            }
//            else
//                PlayGameServices.unlockAchievement(PluginManager.ANDROID_Achievements[(int)achievement]);
//        }
//    }
//
//    public override void Share(Dictionary<string, string> pars, Texture2D pic = null)
//    {
//        PlayGameServices.showShareDialog(pars["message"]);    
//    }
//
//    private void OnAuthSuccess(string res)
//    {
//        me = new SocialFriend(PlayGameServices.getLocalPlayerInfo().name, PlayGameServices.getLocalPlayerInfo().playerId, null);
//        PluginManager.Instance.StartCoroutine(DownloadProfilePicture(PlayGameServices.getLocalPlayerInfo()));
//        LoadAchievements();
//    }
//
//    private void OnGPGMetadataLoaded(List<GPGAchievementMetadata> datas)
//    {
//        if (datas == null)
//        {
//            OnLoadAchievementsFailed("Achievement data is null!");
//            return;
//        }
//        achievements = new List<Achievement>();
//        foreach (var ach in datas)
//        {
//            Achievement a = new Achievement(ach.name, ach.achievementDescription, ach.achievementId, (float)ach.progress);
//            if (ach.type == 1)
//            {
//                a.isIncremental = true;
//                a.maxSteps = ach.numberOfSteps;
//            }
//            achievements.Add(a);
//        }
//        OnLoadedAchievements();
//    }
//
//    private void OnGPGLeaderboards(List<GPGLeaderboardMetadata> datas)
//    {
//        if (datas == null)
//        {
//            OnLoadLeaderboardsFailed("Leaderboard data is null!");
//            return;
//        }
//        leaderboards = new List<Leaderboard>();
//        foreach (var lead in datas)
//        {
//            Leaderboard l = new Leaderboard(lead.leaderboardId, lead.title, null);
//            leaderboards.Add(l);
//        }
//        if (leaderboards.Count > 0)
//            PlayGameServices.loadScoresForLeaderboard(leaderboards[0].id, GPGLeaderboardTimeScope.AllTime, true, false);
//    }
//
//    private void OnScoresLoaded(List<GPGScore> scores)
//    {
//        if (scores == null)
//        {
//            OnLoadLeaderboardsFailed("Score data is null!");
//            return;
//        }
//
//        List<Score> scs = new List<Score>();
//        eLeaderboard current = (eLeaderboard)0;
//        for (int i = 0; i < leaderboards.Count; i++)
//        {
//            if (leaderboards[i].id == scores[0].leaderboardId)
//            {
//                current = (eLeaderboard)i;
//                break;
//            }
//        }
//
//        if (friends == null)
//            friends = new List<SocialFriend>();
//
//        foreach (var score in scores)
//        {
//            SocialFriend friend = null;
//            foreach (var fr in friends)
//                if (fr.id == score.playerId)
//                    friend = fr;
//            if (friend == null)
//            {
//                friend = new SocialFriend(score.displayName, score.playerId, null);
//                friend.url = score.avatarUrl;
//            }
//            Score s = new Score(leaderboards[(int)current], friend, (long)score.value, (long)score.rank);
//        }
//        if (PluginManager.ANDROID_Leaderboards.Length > ((int)current + 1))
//            PlayGameServices.loadScoresForLeaderboard(PluginManager.ANDROID_Leaderboards[(int)current + 1], GPGLeaderboardTimeScope.AllTime, true, false);
//        else
//        {
//            foreach (var fr in friends)
//            {
//                GPGPlayerInfo playerInfo = new GPGPlayerInfo();
//                playerInfo.playerId = fr.id;
//                playerInfo.avatarUrl = fr.url;
//                PluginManager.Instance.StartCoroutine(DownloadProfilePicture(playerInfo));
//            }
//        }
//    }
//
//    private IEnumerator DownloadProfilePicture(GPGPlayerInfo player)
//    {
//        if (player == null || player.avatarUrl == null)
//        {
//            picDownloaded++;
//            yield break;
//        }
//        WWW www = new WWW(player.avatarUrl);
//        if (www.error != null && www.error != "")
//        {
//            picDownloaded++;
//            yield break;
//        }
//
//        if (player.playerId == me.id)
//        {
//            me.photo = www.texture;
//            OnLoggedIn();
//        }
//        else
//        {
//            if (friends != null)
//                foreach (var friend in friends)
//                    if (friend.id == player.playerId)
//                    {
//                        picDownloaded++;
//                        friend.photo = www.texture;
//                        if (picDownloaded == friends.Count)
//                            OnLoadedFriends();
//                        yield break;
//                    }
//        }
//    }
//}
//#endif