using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SocialManager
{

    public Dictionary<eSocialAdapter, ISocialAdapter> SocialAdapters = new Dictionary<eSocialAdapter,ISocialAdapter>();
    
    //Handlers
    public Action<eSocialAdapter> OnLoggedIn;
    public Action<eSocialAdapter, string> OnLogInFailed; 
    public Action<eSocialAdapter> OnLoggedOut;
    public Action<eSocialAdapter, string> OnLogOutFailed;
    public Action<eSocialAdapter> OnLoadedFriends;
    public Action<eSocialAdapter, string> OnLoadFriendsFailed;
    public Action<eSocialAdapter> OnLoadedAchievements;
    public Action<eSocialAdapter, string> OnLoadAchievementsFailed;
    public Action<eSocialAdapter> OnLoadedLeaderboards;
    public Action<eSocialAdapter, string> OnLoadLeaderboardsFailed;
    public Action<eSocialAdapter> OnShared;
    public Action<eSocialAdapter, string> OnShareFailed;
    public Action<eSocialAdapter, string> OnNotification;

    private eSocialAdapter defaultAdapter = eSocialAdapter.Test;


    public SocialManager(Dictionary<eSocialAdapter, ISocialAdapter> adapters)
    {
        SocialAdapters = adapters;
        foreach (var adapter in SocialAdapters.Values)
            adapter.Init();
    }

    public void SetDefaultAdapter(eSocialAdapter adapter)
    {
        defaultAdapter = adapter;
    }

    public SocialFriend GetMe(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].GetMe();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return null;
    }

    public SocialFriend GetMe()
    {
        if (SocialAdapters.ContainsKey(defaultAdapter))
            return SocialAdapters[defaultAdapter].GetMe();
        return null;
    }

    public List<SocialFriend> GetFriends(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].GetFriends();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return null;
    }

    public List<SocialFriend> GetFriends()
    {
        if (SocialAdapters.ContainsKey(defaultAdapter))
            return SocialAdapters[defaultAdapter].GetFriends();
        return null;
    }

    public void Login(eSocialAdapter adapter)
    {
        defaultAdapter = adapter;
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].Login();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void Login()
    {
        Login(defaultAdapter);
    }

    public void Logout(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].Logout();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void Logout()
    {
        Logout(defaultAdapter);
    }

    public void LoadFriends(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].LoadFriends();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void LoadFriends()
    {
        LoadFriends(defaultAdapter);
    }

    public void ShowLeaderboards(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].ShowLeaderboards();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void ShowLeaderboards()
    {
        ShowLeaderboards(defaultAdapter);
    }

    public void ShowAchievements(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].ShowAchievements();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void ShowAchievements()
    {
        ShowAchievements(defaultAdapter);
    }

    public string GetID(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].GetID();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return null;
    }

    public string GetID()
    {
        return GetID(defaultAdapter);
    }

    public void Share(eSocialAdapter adapter, Dictionary<string, string> pars, Texture2D pic = null)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].Share(pars, pic);
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void Share(Dictionary<string, string> pars, Texture2D pic = null)
    {
        Share(defaultAdapter, pars, pic);
    }

    public void SendNotification(eSocialAdapter adapter, List<SocialFriend> to, Dictionary<string, string> pars)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].SendNotification(to, pars);
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void SendNotification(List<SocialFriend> to, Dictionary<string, string> pars)
    {
        SendNotification(defaultAdapter, to, pars);
    }

    public bool IsLoggedIn(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].IsLoggedIn();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return false;
    }

    public bool IsLoggedIn()
    {
        return IsLoggedIn(defaultAdapter);
    }

    public bool IsFriendsLoaded(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].IsFriendsLoaded();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return false;
    }

    public bool IsFriendsLoaded()
    {
        return IsFriendsLoaded(defaultAdapter);
    }

    public void LoadLeaderboards(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].LoadLeaderboards();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void LoadLeaderboards()
    {
        LoadLeaderboards(defaultAdapter);
    }

    public void LoadAchievements(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].LoadAchievements();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void LoadAchievements()
    {
        LoadAchievements(defaultAdapter);
    }

    public void SubmitScore(eSocialAdapter adapter, eLeaderboard leaderboard, long score)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].SubmitScore(leaderboard, score);
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void SubmitScore(eLeaderboard leaderboard, long score)
    {
        SubmitScore(defaultAdapter, leaderboard, score);
    }

    public void ReportAchievement(eSocialAdapter adapter, eAchievement achievement, float progress)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].ReportAchievement(achievement, progress);
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void ReportAchievement(eAchievement achievement, float progress)
    {
        ReportAchievement(defaultAdapter, achievement, progress);
    }

    public void ChallangeFriends(eSocialAdapter adapter, List<SocialFriend> friends, eLeaderboard leaderBoard, long score, string msg)
    {
        if (SocialAdapters.ContainsKey(adapter))
            SocialAdapters[adapter].ChallangeFriends(friends, leaderBoard, score, msg);
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
    }

    public void ChallangeFriends(List<SocialFriend> friends, eLeaderboard leaderBoard, long score, string msg)
    {
        ChallangeFriends(defaultAdapter, friends, leaderBoard, score, msg);
    }

    public List<Achievement> GetAchievements(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].GetAchievements();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return null;
    }

    public List<Achievement> GetAchievements()
    {
        return GetAchievements(defaultAdapter);
    }

    public List<Leaderboard> GetLeaderboards(eSocialAdapter adapter)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].GetLeaderboards();
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return null;
    }

    public List<Leaderboard> GetLeaderboards()
    {
        return GetLeaderboards(defaultAdapter);
    }

    public List<Score> GetScores(eSocialAdapter adapter, eLeaderboard leaderboard)
    {
        if (SocialAdapters.ContainsKey(adapter))
            return SocialAdapters[adapter].GetScores(leaderboard);
        else
            Debug.LogError("Adapter " + adapter.ToString() + " not found!");
        return null;
    }

    public List<Score> GetScores(eLeaderboard leaderboard)
    {
        return GetScores(defaultAdapter, leaderboard);
    }

}
