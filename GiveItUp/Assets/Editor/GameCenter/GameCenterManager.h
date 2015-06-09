//
//  GameCenter.h
//  Unity-iPhone
//
//  Created by Yuya Ono on 2013/08/30.
//
//

#import <Foundation/Foundation.h>
#import <GameKit/GKLocalPlayer.h>
#import <GameKit/GameKit.h>

#define USING_UNITY_3_5 1

@interface GameCenterManager : NSObject<GKGameCenterControllerDelegate> {
    UIViewController* __unsafe_unretained uiViewController;
    UIViewController* __unsafe_unretained auiViewController;

}

@property(nonatomic,assign) UIViewController* uiViewController;
@property(nonatomic,assign) UIViewController* auiViewController;
@property(nonatomic, retain) NSMutableDictionary *achievementsDictionary;
@property(nonatomic, retain) NSString *udKeyId;

+ (GameCenterManager*) instance;

BOOL isGameCenterAvailable(void);

- (void) initGameCenter;

- (void) authenticateLocalPlayerWithVC;

- (void) authenticateLocalPlayer;

- (void) authenticateSuccess: (GKLocalPlayer*) localPlayer: (NSError *) error;
// FRIENDS ===========================

- (void) retrieveFriends;

- (void) loadPlayerData: (NSArray *) identifiers;

- (void) loadPlayerPhoto: (GKPlayer*) player;

// ACHIEVEMENTS ===========================
- (void) reportAchievement: (NSString*) identifier percentage: (float) percent;
//report acheivements
//- (void) reportAchievementIdentifier: (NSString*) identifier percentComplete: (float) percent;

// When your game loads achievement data, add the achievement objects to the dictionary:
- (void) loadAchievements;

// If the identifier does not exist as a key in the dictionary, create a new achievement object and add it to the dictionary:
- (GKAchievement*) getAchievementForIdentifier: (NSString*) identifier;

- (void) resetAchievements;

- (void) showLeaderboard;//: (NSString*) leaderboardID;

- (void) gameCenterViewControllerDidFinish:(GKGameCenterViewController *)gameCenterViewController;

@end
