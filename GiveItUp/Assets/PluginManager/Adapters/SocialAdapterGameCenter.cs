#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SocialAdapterGameCenter : ISocialAdapter 
{
    public bool isLoggedIn = false;
    private SocialFriend me = null;
    private List<SocialFriend> friends = null;
    private List<Achievement> achievements = null;
    private List<Leaderboard> leaderboards = null;
    private bool initied = false;

    public override void Init()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable || initied)
        {
            PluginManager.OnConnectionOpened += Init;
            return;
        }

        PluginManager.OnConnectionOpened -= Init;
        
        Debug.Log("PINAFASZU");
        socialType = eSocialAdapter.GameCenter;
        GameCenterManager.playerAuthenticatedEvent += UserAuthenticated;
        GameCenterManager.playerFailedToAuthenticateEvent += OnLogInFailed;
        GameCenterManager.playerDataLoadedEvent += FriendsArrived;
        GameCenterManager.loadPlayerDataFailedEvent += GetFriendsFailed;
        //GameCenterManager.achievementsLoaded += AchievementsLoaded;
        //GameCenterManager.achievementMetadataLoaded += MetaDataLoaded;
        //GameCenterManager.loadAchievementsFailed += OnLoadAchievementsFailed;
        //GameCenterManager.retrieveAchievementMetadataFailed += OnLoadAchievementsFailed;
        //GameCenterManager.categoriesLoaded += LeaderboardsArrived;
        GameCenterManager.scoresLoadedEvent += ScoresArrived;
        //GameCenterManager.loadCategoryTitlesFailed += OnLoadLeaderboardsFailed;
        GameCenterManager.retrieveScoresFailed += OnRetrieveScoresFailed;
		GameCenterManager.scoresForPlayerIdLoadedEvent += ScoresForPlayerIdArrived;
		GameCenterManager.retrieveScoresForPlayerIdFailedEvent += ScoresForPlayerIdFailed;
        InitGC();
        initied = true;
    }

    private void InitGC()
    {
		Debug.Log("AUTHENTICATE START  " + GameCenterBinding.isPlayerAuthenticated());
    	GameCenterBinding.authenticateLocalPlayer();
    }

    public override void Login()
    {
        
    }

    public override void Logout()
    {
    }

    public override string GetID()
    {
        if (me == null)
            return null;
        return me.id;
    }

    public override List<SocialFriend> GetFriends()
    {
        return friends;
    }

    public override void LoadFriends()
    {
        if (GameCenterBinding.isPlayerAuthenticated())
            GameCenterBinding.loadPlayerData(new string[] { GameCenterBinding.playerIdentifier() }, true, false);
    }
    
	public override void ShowLeaderboards()
	{
		//if (GameCenterBinding.isPlayerAuthenticated())
			GameCenterBinding.showGameCenterViewController(GameCenterViewControllerState.Leaderboards);
	}

    public override void LoadAchievements()
    {
        if (GameCenterBinding.isPlayerAuthenticated())
            GameCenterBinding.getAchievements();
    }

    public override bool IsLoggedIn()
    {
        return GameCenterBinding.isPlayerAuthenticated();
    }

    public override bool IsFriendsLoaded()
    {
        return friends != null;
    }

    public override List<Achievement> GetAchievements()
    {
        return achievements;
    }

    public override void LoadLeaderboards()
    {
        if (GameCenterBinding.isPlayerAuthenticated())
         GameCenterBinding.loadLeaderboardTitles();
    }

    public override SocialFriend GetMe()
    {
        return me;
    }

    public override void ChallangeFriends(List<SocialFriend> friends, eLeaderboard leaderBoard, long score, string msg)
    {
        if (GameCenterBinding.isPlayerAuthenticated())
        {
            string[] ids = new string[friends.Count];
            for (int i = 0; i < friends.Count; i++)
                ids[i] = friends[i].id;

            GameCenterBinding.issueScoreChallenge(score, 0, PluginManager.IOS_Leaderboards[(int)leaderBoard], ids, msg);
        }
    }

    public override void SubmitScore(eLeaderboard leaderboard, long score)
    {
        if (GameCenterBinding.isPlayerAuthenticated())
            GameCenterBinding.reportScore(score, PluginManager.IOS_Leaderboards[(int)leaderboard]);
    }

    public override void ReportAchievement(eAchievement achievement, float progress)
    {
        if (GameCenterBinding.isPlayerAuthenticated())
        {
            GameCenterBinding.reportAchievement(PluginManager.IOS_Achievements[(int)achievement], progress);
            if (progress >= 100 && PlayerPrefs.GetInt(achievement.ToString(), 0) == 0)
            {
            	PlayerPrefs.SetInt(achievement.ToString(), 1);
            	GameCenterBinding.showCompletionBannerForAchievements();
            }
        }
    }

	private void OnApplicationPause( bool pause )
	{
		Debug.Log("ON APP PAUSE");
		if (pause)
		{
			isAuthenticatedThisSession = false;
			isFriendsGet = false;
		}
	}

	private bool isAuthenticatedThisSession = false;
    private void UserAuthenticated()
    {
    	if (isAuthenticatedThisSession)
    		return;
    		
    	isAuthenticatedThisSession = true;
    
        //me = new SocialFriend(GameCenterBinding.playerAlias(), GameCenterBinding.playerIdentifier(), null);
        //GameCenterBinding.retrieveFriends(true, false);
        
		Debug.Log("USER LOGGED IN  " + GameCenterBinding.playerAlias());
        
        GameCenterBinding.retrieveScoresForPlayerId( GameCenterBinding.playerIdentifier(), PluginManager.IOS_Leaderboards[(int)(eLeaderboard.DailyChallengeRank)] );
        
       // GameCenterBinding.retrieveScores( true, GameCenterLeaderboardTimeScope.AllTime, 1, 99 );
    }

    private void FriendsArrived(List<GameCenterPlayer> players)
    {
    	Debug.Log("FRIENDS ARRIVED 1");
    
        if (players == null)
            return;
        
        friends = new List<SocialFriend>();

        foreach (var gc_player in players)
        {
            if (gc_player.playerId == GameCenterBinding.playerIdentifier())
            {
                continue;
            }
            else
            {
                friends.Add(new SocialFriend(gc_player.alias, gc_player.playerId, gc_player.profilePhoto));
            }
        }
                
        GameCenterBinding.retrieveScores( true, GameCenterLeaderboardTimeScope.AllTime, 1, 99, PluginManager.IOS_Leaderboards[(int)(eLeaderboard.CompletedLevels)]);
        
    }
    
    private void GetFriendsFailed( string error )
    {
    	Debug.Log("GET FRIENDS FAILED");
    	Debug.Log( error );
    }

    private void AchievementsLoaded(List<GameCenterAchievement> gc_achievements)
    {
        if (gc_achievements == null)
            return;
        achievements = new List<Achievement>();

        for (int i = 0; i < System.Enum.GetValues(typeof(eAchievement)).Length; i++)
        {
            foreach (var gc_a in gc_achievements)
            {
                if (gc_a.identifier == PluginManager.IOS_Achievements[i])
                {
                    achievements.Add(new Achievement("", "", gc_a.identifier, gc_a.percentComplete));
                    break;
                }
            }
        }
        GameCenterBinding.retrieveAchievementMetadata();
    }

    private void MetaDataLoaded(List<GameCenterAchievementMetadata> metas)
    {
        if (metas == null)
            return;

        foreach (var meta in metas)
        {
            foreach (var a in achievements)
            {
                if (meta.identifier == a.id)
                {
                    a.title = meta.title;
                    a.description = meta.description;
                    break;
                }
            }
        }
        OnLoadedAchievements();
    }

    private void LeaderboardsArrived(List<GameCenterLeaderboard> boards)
    {
        if (boards == null)
            return;
        leaderboards = new List<Leaderboard>();

        for (int i = 0; i < System.Enum.GetValues(typeof(eLeaderboard)).Length; i++)
        {
            foreach (var gc_board in boards)
            {
                if (gc_board.leaderboardId == PluginManager.IOS_Leaderboards[i])
                {
                    leaderboards.Add(new Leaderboard(gc_board.leaderboardId, gc_board.title, null));
                    break;
                }
            }
        }
        //GameCenterBinding.retrieveScores(true, GameCenterLeaderboardTimeScope.AllTime, 1, 99);
    }

	
	private void ScoresArrived(GameCenterRetrieveScoresResult gc_res)
    {
    	Debug.Log("FRIEND SCORES ARRIVED");
    
        if (gc_res == null) return;
        
        PluginManager.Instance.gcFriendProgresses = new List<GCFriendProgress>();

		foreach (GameCenterScore gcs in gc_res.scores )
		{
			if (gcs.category == PluginManager.IOS_Leaderboards[(int)(eLeaderboard.CompletedLevels)])
			{
				foreach (SocialFriend sf in friends)
				{
					if (sf.id == gcs.playerId)
					{
						Debug.Log("ADD GC FRIEND DATA");
						GCFriendProgress gcfp;
						gcfp.pic = sf.photo;
						gcfp.progress = (int)(gcs.value);
						PluginManager.Instance.gcFriendProgresses.Add(gcfp);
					}
				}
			
				if (User.dailyChallengeLeaderboardScore < (int)(gcs.value))
				{
					User.dailyChallengeLeaderboardScore = (int)(gcs.value);
				}
			}
		}
		
		CGame.menuLayer.MainMenuGUI_RefreshGameCenterFriends();
    }
    
    private void OnRetrieveScoresFailed( string error )
    {
    	Debug.Log("RETRIEVE SCORES FAILED");
    	Debug.Log( error );
    }
    
    
    private bool isFriendsGet = false;
    private void ScoresForPlayerIdArrived( GameCenterRetrieveScoresResult gcrsr )
    {    	    
    	Debug.Log("SCORES FOR ID ARRIVED");
		
		foreach (GameCenterScore gcs in gcrsr.scores )
		{
			if (gcs.playerId == GameCenterBinding.playerIdentifier())
			{				
				if (gcs.category == PluginManager.IOS_Leaderboards[(int)(eLeaderboard.DailyChallengeRank)])
				{
					if (User.dailyChallengeLeaderboardScore < (int)(gcs.value))
					{
						User.dailyChallengeLeaderboardScore = (int)(gcs.value);
					}
				}
			}
		}
		
		if (!isFriendsGet)
		{
			isFriendsGet = true;
			GameCenterBinding.retrieveFriends( true, false );
		}
	}
    
    private void ScoresForPlayerIdFailed( string error )
    {
    	Debug.Log("Retrieve scores FAILED");
    	Debug.Log( error );
    }
}
#endif