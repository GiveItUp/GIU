using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public partial class GameCenterEventListener : MonoBehaviour
{
#if UNITY_IPHONE
	void Start()
	{
		// Listens to all the GameCenter events.  All event listeners MUST be removed before this object is disposed!
		// Player events
		GameCenterManager.loadPlayerDataFailedEvent += loadPlayerDataFailed;
		GameCenterManager.playerDataLoadedEvent += playerDataLoaded;
		GameCenterManager.playerAuthenticatedEvent += playerAuthenticated;
		GameCenterManager.playerFailedToAuthenticateEvent += playerFailedToAuthenticate;
		GameCenterManager.playerLoggedOutEvent += playerLoggedOut;
		GameCenterManager.profilePhotoLoadedEvent += profilePhotoLoaded;
		GameCenterManager.profilePhotoFailedEvent += profilePhotoFailed;

		// Leaderboards and scores
		GameCenterManager.loadCategoryTitlesFailedEvent += loadCategoryTitlesFailed;
		GameCenterManager.categoriesLoadedEvent += categoriesLoaded;
		GameCenterManager.reportScoreFailedEvent += reportScoreFailed;
		GameCenterManager.reportScoreFinishedEvent += reportScoreFinished;
		GameCenterManager.retrieveScoresFailedEvent += retrieveScoresFailed;
		GameCenterManager.scoresLoadedEvent += scoresLoaded;
		GameCenterManager.retrieveScoresForPlayerIdFailedEvent += retrieveScoresForPlayerIdFailed;
		GameCenterManager.scoresForPlayerIdLoadedEvent += scoresForPlayerIdLoaded;

		// Achievements
		GameCenterManager.reportAchievementFailedEvent += reportAchievementFailed;
		GameCenterManager.reportAchievementFinishedEvent += reportAchievementFinished;
		GameCenterManager.loadAchievementsFailedEvent += loadAchievementsFailed;
		GameCenterManager.achievementsLoadedEvent += achievementsLoaded;
		GameCenterManager.resetAchievementsFailedEvent += resetAchievementsFailed;
		GameCenterManager.resetAchievementsFinishedEvent += resetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailedEvent += retrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoadedEvent += achievementMetadataLoaded;

		// Challenges
		GameCenterManager.localPlayerDidSelectChallengeEvent += localPlayerDidSelectChallengeEvent;
		GameCenterManager.localPlayerDidCompleteChallengeEvent += localPlayerDidCompleteChallengeEvent;
		GameCenterManager.remotePlayerDidCompleteChallengeEvent += remotePlayerDidCompleteChallengeEvent;
		GameCenterManager.challengesLoadedEvent += challengesLoadedEvent;
		GameCenterManager.challengesFailedToLoadEvent += challengesFailedToLoadEvent;
		GameCenterManager.challengeIssuedSuccessfullyEvent += challengeIssuedSuccessfullyEvent;
		GameCenterManager.challengeNotIssuedEvent += challengeNotIssuedEvent;
	}


	void OnDisable()
	{
		// Remove all the event handlers
		// Player events
		GameCenterManager.loadPlayerDataFailedEvent -= loadPlayerDataFailed;
		GameCenterManager.playerDataLoadedEvent -= playerDataLoaded;
		GameCenterManager.playerAuthenticatedEvent -= playerAuthenticated;
		GameCenterManager.playerLoggedOutEvent -= playerLoggedOut;
		GameCenterManager.profilePhotoLoadedEvent -= profilePhotoLoaded;
		GameCenterManager.profilePhotoFailedEvent -= profilePhotoFailed;

		// Leaderboards and scores
		GameCenterManager.loadCategoryTitlesFailedEvent -= loadCategoryTitlesFailed;
		GameCenterManager.categoriesLoadedEvent -= categoriesLoaded;
		GameCenterManager.reportScoreFailedEvent -= reportScoreFailed;
		GameCenterManager.reportScoreFinishedEvent -= reportScoreFinished;
		GameCenterManager.retrieveScoresFailedEvent -= retrieveScoresFailed;
		GameCenterManager.scoresLoadedEvent -= scoresLoaded;
		GameCenterManager.retrieveScoresForPlayerIdFailedEvent -= retrieveScoresForPlayerIdFailed;
		GameCenterManager.scoresForPlayerIdLoadedEvent -= scoresForPlayerIdLoaded;

		// Achievements
		GameCenterManager.reportAchievementFailedEvent -= reportAchievementFailed;
		GameCenterManager.reportAchievementFinishedEvent -= reportAchievementFinished;
		GameCenterManager.loadAchievementsFailedEvent -= loadAchievementsFailed;
		GameCenterManager.achievementsLoadedEvent -= achievementsLoaded;
		GameCenterManager.resetAchievementsFailedEvent -= resetAchievementsFailed;
		GameCenterManager.resetAchievementsFinishedEvent -= resetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailedEvent -= retrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoadedEvent -= achievementMetadataLoaded;

		// Challenges
		GameCenterManager.localPlayerDidSelectChallengeEvent -= localPlayerDidSelectChallengeEvent;
		GameCenterManager.localPlayerDidCompleteChallengeEvent -= localPlayerDidCompleteChallengeEvent;
		GameCenterManager.remotePlayerDidCompleteChallengeEvent -= remotePlayerDidCompleteChallengeEvent;
		GameCenterManager.challengesLoadedEvent -= challengesLoadedEvent;
		GameCenterManager.challengesFailedToLoadEvent -= challengesFailedToLoadEvent;
		GameCenterManager.challengeIssuedSuccessfullyEvent -= challengeIssuedSuccessfullyEvent;
		GameCenterManager.challengeNotIssuedEvent -= challengeNotIssuedEvent;
	}



	#region Player Events

	void playerAuthenticated()
	{
		Debug.Log( "playerAuthenticatedEvent" );
	}


	void playerFailedToAuthenticate( string error )
	{
		Debug.Log( "playerFailedToAuthenticateEvent: " + error );
	}


	void playerLoggedOut()
	{
		Debug.Log( "playerLoggedOutEvent" );
	}


	void playerDataLoaded( List<GameCenterPlayer> players )
	{
		Debug.Log( "playerDataLoadedEvent" );
		foreach( GameCenterPlayer p in players )
			Debug.Log( p );
	}


	void loadPlayerDataFailed( string error )
	{
		Debug.Log( "loadPlayerDataFailedEvent: " + error );
	}


	void profilePhotoLoaded( string path )
	{
		Debug.Log( "profilePhotoLoadedEvent: " + path );
	}


	void profilePhotoFailed( string error )
	{
		Debug.Log( "profilePhotoFailedEvent: " + error );
	}

	#endregion;



	#region Leaderboard Events

	void categoriesLoaded( List<GameCenterLeaderboard> leaderboards )
	{
		Debug.Log( "categoriesLoadedEvent" );
		foreach( GameCenterLeaderboard l in leaderboards )
			Debug.Log( l );
	}


	void loadCategoryTitlesFailed( string error )
	{
		Debug.Log( "loadCategoryTitlesFailedEvent: " + error );
	}

	#endregion;


	#region Score Events

	void scoresLoaded( GameCenterRetrieveScoresResult retrieveScoresResult )
	{
		Debug.Log( "scoresLoadedEvent" );
		Prime31.Utils.logObject( retrieveScoresResult );
	}


	void retrieveScoresFailed( string error )
	{
		Debug.Log( "retrieveScoresFailedEvent: " + error );
	}


	void retrieveScoresForPlayerIdFailed( string error )
	{
		Debug.Log( "retrieveScoresForPlayerIdFailedEvent: " + error );
	}


	void scoresForPlayerIdLoaded( GameCenterRetrieveScoresResult retrieveScoresResult )
	{
		Debug.Log( "scoresForPlayerIdLoadedEvent" );
		Prime31.Utils.logObject( retrieveScoresResult );
	}


	void reportScoreFinished( string category )
	{
		Debug.Log( "reportScoreFinishedEvent for category: " + category );
	}


	void reportScoreFailed( string error )
	{
		Debug.Log( "reportScoreFailedEvent: " + error );
	}

	#endregion;


	#region Achievement Events

	void achievementMetadataLoaded( List<GameCenterAchievementMetadata> achievementMetadata )
	{
		Debug.Log( "achievementMetadatLoadedEvent" );
		foreach( GameCenterAchievementMetadata s in achievementMetadata )
			Debug.Log( s );
	}


	void retrieveAchievementMetadataFailed( string error )
	{
		Debug.Log( "retrieveAchievementMetadataFailedEvent: " + error );
	}


	void resetAchievementsFinished()
	{
		Debug.Log( "resetAchievmenetsFinishedEvent" );
	}


	void resetAchievementsFailed( string error )
	{
		Debug.Log( "resetAchievementsFailedEvent: " + error );
	}


	void achievementsLoaded( List<GameCenterAchievement> achievements )
	{
		Debug.Log( "achievementsLoadedEvent" );
		foreach( GameCenterAchievement s in achievements )
			Debug.Log( s );
	}


	void loadAchievementsFailed( string error )
	{
		Debug.Log( "loadAchievementsFailedEvent: " + error );
	}


	void reportAchievementFinished( string identifier )
	{
		Debug.Log( "reportAchievementFinishedEvent: " + identifier );
	}


	void reportAchievementFailed( string error )
	{
		Debug.Log( "reportAchievementFailedEvent: " + error );
	}

	#endregion;


	#region Challenges

	public void localPlayerDidSelectChallengeEvent( GameCenterChallenge challenge )
	{
		Debug.Log( "localPlayerDidSelectChallengeEvent : " + challenge );
	}


	public void localPlayerDidCompleteChallengeEvent( GameCenterChallenge challenge )
	{
		Debug.Log( "localPlayerDidCompleteChallengeEvent : " + challenge );
	}


	public void remotePlayerDidCompleteChallengeEvent( GameCenterChallenge challenge )
	{
		Debug.Log( "remotePlayerDidCompleteChallengeEvent : " + challenge );
	}


	void challengesLoadedEvent( List<GameCenterChallenge> list )
	{
		Debug.Log( "challengesLoadedEvent" );
		Prime31.Utils.logObject( list );
	}


	void challengesFailedToLoadEvent( string error )
	{
		Debug.Log( "challengesFailedToLoadEvent: " + error );
	}


	void challengeIssuedSuccessfullyEvent( List<object> playerIds )
	{
		Debug.Log( "challengeIssuedSuccessfullyEvent" );
		Prime31.Utils.logObject( playerIds );
	}


	void challengeNotIssuedEvent()
	{
		Debug.Log( "challengeNotIssuedEvent" );
	}

	#endregion

#endif
}
