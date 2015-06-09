//
//  GameCenter.m
//  Unity-iPhone
//
//  Created by Yuya Ono on 2013/08/30.
//
//

#import "GameCenterManager.h"
#import <GameKit/GKLocalPlayer.h>

@implementation GameCenterManager: NSObject

@synthesize uiViewController;
@synthesize auiViewController;

@synthesize achievementsDictionary;
@synthesize udKeyId;

void UnitySendMessage( const char * className, const char * methodName, const char * param );

BOOL isGameCenterAvailable(void){
    NSLog(@"ava");
	Class gcClass = (NSClassFromString(@"GKLocalPlayer"));
	
	NSString *reqSysVer = @"4.1";
	NSString *currSysVer = [[UIDevice currentDevice] systemVersion];
	BOOL osVersionSupported = ([currSysVer compare:reqSysVer options:NSNumericSearch] != NSOrderedAscending);
	return (gcClass && osVersionSupported);
}

+ (GameCenterManager*) instance
{
	static GameCenterManager *sharedSingleton;
	
	if( !sharedSingleton ){
		sharedSingleton = [[GameCenterManager alloc] init];
    }
	
	return sharedSingleton;
}

-(void) initGameCenter
{
    self.uiViewController = [[[UIApplication sharedApplication] keyWindow] rootViewController];
    self.auiViewController = [[[UIApplication sharedApplication] keyWindow] rootViewController];
    self.udKeyId = @"STRATAGIA_GCID";
    achievementsDictionary = [[NSMutableDictionary alloc] init];
}
- (void) authenticateLocalPlayerWithVC
{
    NSLog(@"try auth with VC");
    [[GKLocalPlayer localPlayer] authenticateWithCompletionHandler:nil];
}
- (void) authenticateLocalPlayer
{
    NSLog(@"try auth");
    GKLocalPlayer *localPlayer = [GKLocalPlayer localPlayer];
    const float version = [[[UIDevice currentDevice] systemVersion] floatValue];
    if(version >= 6){
        NSLog(@"version 6");

            localPlayer.authenticateHandler = ^(UIViewController *viewController, NSError *error){
            if (viewController != nil){
                NSLog(@"open authorization view");
                [self.uiViewController presentViewController:viewController animated:YES completion:nil];
                //UnitySendMessage( "GamecenterManager", "gamecenterLoginDialogOpen", "" );
                //[self showAuthenticationDialogWhenReasonable: viewController];
            }
            else if (localPlayer.isAuthenticated)
            {
                [self authenticateSuccess:localPlayer : error];
            }
            else
            {
                NSLog(@"error : %@", error);
                UnitySendMessage( "GamecenterManager", "gamecenterAuthenticateCancelled", ""  );
                //[self disableGameCenter];
            }
        };
    }else if(version >= 5){
        NSLog(@"version 5");        
        [localPlayer authenticateWithCompletionHandler:^(NSError *error) {
            if (localPlayer.isAuthenticated)
            {
                [self authenticateSuccess:localPlayer : error];
            }else{
                NSLog(@"error : %@", error);
                UnitySendMessage( "GamecenterManager", "gamecenterAuthenticateCancelled", ""  );
                //[self disableGameCenter];
            }
        }];
    }
    
}
- (void) authenticateSuccess: (GKLocalPlayer*) localPlayer: (NSError *) error
{
    NSUserDefaults *ud = [NSUserDefaults standardUserDefaults];
    //set default
    NSMutableDictionary *defaults = [NSMutableDictionary dictionary];
    [defaults setObject:@"" forKey:self.udKeyId];
    [ud registerDefaults:defaults];
    //compare current & previous localplayerId
    NSString *str = [ud stringForKey: self.udKeyId ];
    if( [str isEqualToString:localPlayer.playerID]){
        NSLog(@"is already authenticated");
        //send to unity
        UnitySendMessage( "GamecenterManager", "gamecenterIsAlreadyAuthenticated", [localPlayer.playerID UTF8String]  );
    }else{
        NSLog(@"authenticated with different player");
        //change NSUserDefault
        [ud setObject:localPlayer.playerID forKey: self.udKeyId];
        //send to unity
        UnitySendMessage( "GamecenterManager", "gamecenterAuthenticateSucceed", [localPlayer.playerID UTF8String]  );
        
    }
}

- (void) retrieveFriends
{
    GKLocalPlayer *lp = [GKLocalPlayer localPlayer];
    if (lp.authenticated)
    {
        NSLog(@"friends　reqqqqqqqqqqqqq");
        [lp loadFriendsWithCompletionHandler:^(NSArray *friends, NSError *error) {
            if (friends != nil && friends.count != 0)
            {
                
                                    NSLog(@"3 : %d", friends.count);
                for(NSString *str in friends)
                {
                    NSLog(@"friends : %@", str);
                }
                
                [self loadPlayerData: friends];
            }
            if (error != nil)
            {
                NSLog(@"error: %@", error);
            }
        }];
    }
}



- (void) loadPlayerData: (NSArray *) identifiers
{
    [GKPlayer loadPlayersForIdentifiers:identifiers withCompletionHandler:^(NSArray *players, NSError *error) {
        if (error != nil)
        {
            NSLog(@"error : %@", error);
            // Handle the error.
        }
        //players is NSArray of <GKPlayer> object
        if (players != nil)
        {
            NSString *name = @"";
            NSString *combinedString = @"";
            
            for(GKPlayer *gkp in players)
            {
                NSLog(@"friends : %@", gkp.displayName);
                name = [NSString stringWithFormat:@"/%@,%@", gkp.displayName, gkp.playerID];
                combinedString = [combinedString stringByAppendingString: name];
            }
            NSLog(@"comb : %@",combinedString);
            //send to unity
            UnitySendMessage( "GamecenterManager", "gamecenterReceiveFriends", [combinedString UTF8String]  );
        }
    }];
}

- (void) loadPlayerPhoto: (GKPlayer*) player
{
    [player loadPhotoForSize:GKPhotoSizeSmall withCompletionHandler:^(UIImage *photo, NSError *error) {
        if (photo != nil)
        {
            [self storePhoto: photo forPlayer: player];
        }
        if (error != nil)
        {
            // Handle the error if necessary.
        }
    }];
}

- (void) reportAchievement: (NSString*) identifier percentage: (float) percent;
{
    GKAchievement *achievement = [[GKAchievement alloc] initWithIdentifier: identifier];
    if (achievement)
    {
        achievement.percentComplete = percent;
        [achievement reportAchievementWithCompletionHandler:^(NSError *error)
         {
             if (error != nil)
             {
                 NSLog(@"Error in reporting achievements: %@", error);
             }else{
                 NSLog(@"report complete");
             }             
             
         }];
    }

}

/*
- (void) reportAchievementIdentifier: (NSString*) identifier percentComplete: (float) percent
{
    
    //NSString *identifier = @"goldearned1000";
    NSLog(@"ok");
    GKAchievement *achievement = [[GKAchievement alloc] initWithIdentifier: identifier];
    if (achievement)
    {
        NSLog(@"ok2");
        achievement.percentComplete = percent;
        [achievement reportAchievementWithCompletionHandler:^(NSError *error)
         {
             if (error != nil)
             {
                 NSLog(@"Error in reporting achievements: %@", error);
             }else{
                 NSLog(@"report complete");             
             }
             
             
         }];
    }
     
}
*/
    
- (void) loadAchievements
{
    [GKAchievement loadAchievementsWithCompletionHandler:^(NSArray *achievements, NSError *error)
     {
         NSString *combinedString;
         NSString *name;
         if (error != nil){
             NSLog(@"error : %@",error);
         }
         if(achievements != nil)
         {
             NSLog(@"%d",achievements.count);
             for (GKAchievement* achievement in achievements)
             {
                 [achievementsDictionary setObject: achievement forKey: achievement.identifier];
                 NSLog(@"%@",achievement.identifier);
                 name = [NSString stringWithFormat:@"/%@,%f", achievement.identifier, achievement.percentComplete];
                 combinedString = [combinedString stringByAppendingString: name];
             }
             //send to unity
             UnitySendMessage( "GamecenterManager", "gamecenterReceiveAcheivements", [combinedString UTF8String]  );
         }
     }];
}

- (GKAchievement*) getAchievementForIdentifier: (NSString*) identifier
{
    GKAchievement *achievement = [achievementsDictionary objectForKey:identifier];
    if (achievement == nil)
    {
        achievement = [[GKAchievement alloc] initWithIdentifier:identifier];
        [achievementsDictionary setObject:achievement forKey:achievement.identifier];
    }
    return achievement;
}

- (void) resetAchievements
{
    // Clear all locally saved achievement objects.
    achievementsDictionary = [[NSMutableDictionary alloc] init];
    // Clear all progress saved on Game Center.
    [GKAchievement resetAchievementsWithCompletionHandler:^(NSError *error)
     {
         if (error != nil){
             // handle the error.
             NSLog(@"error : %@", error);
         }else{
             NSLog(@"reset complete");
         }
     }];
}


- (void) showLeaderboard//: (NSString*) leaderboardID
{
    /*
    NSLog(@"leader x1");
    GKAchievementViewController *achievements = [[GKAchievementViewController alloc] init];
    NSLog(@"leader x2");
    if (achievements){
        NSLog(@"leader x3");
        achievements.achievementDelegate = self;
        NSLog(@"leader x4");
        //        [self.viewController presentModalViewController: achievements animated: YES];
        [self.uiViewController presentViewController:achievements animated:YES completion:NULL]; 
    }
    */
    GKGameCenterViewController *gameCenterController = [[GKGameCenterViewController alloc] init];
    if (gameCenterController != nil)
    {
        NSLog(@"leader x");
        gameCenterController.gameCenterDelegate = self;
        gameCenterController.viewState = GKGameCenterViewControllerStateAchievements;
        [self.auiViewController presentViewController: gameCenterController animated: YES completion:nil];
    }
     
}



- (void)gameCenterViewControllerDidFinish:(GKGameCenterViewController *)gameCenterViewController
{
    [self.auiViewController dismissViewControllerAnimated:YES completion:nil];
}


@end
