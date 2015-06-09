//
//  FacebookBinding.m
//  Facebook
//
//  Created by Mike on 9/13/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "GameCenterManager.h"
#import <GameKit/GKLocalPlayer.h>
//#import "JSON.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil
void _gamecenterInit()
{
    [[GameCenterManager instance] initGameCenter];
}

void _gamecenterAuthenticateWithVC( )
{
	[[GameCenterManager instance] authenticateLocalPlayerWithVC];
}

void _gamecenterAuthenticate( )
{
	[[GameCenterManager instance] authenticateLocalPlayer];
}

bool _gamecenterIsAuthenticated()
{
	return [[GKLocalPlayer localPlayer] isAuthenticated];
}

bool _gamecenterIsAvailable()
{
	//return [[GameCenterManager instance] isGameCenterAvailable];
}

void _gamecenterReportAcheivements(const char * identifier , const char * percent )
{
    
    
    NSString *identifierString = GetStringParam ( identifier );
    NSString *percentString = GetStringParam ( percent );
    NSLog(@"%@",identifierString);
    
    [[GameCenterManager instance] reportAchievement: identifierString percentage: [percentString floatValue]];
    //[[GameCenterManager instance] reportAchievementIdentifier: identifierString percentComplete: *percent ];
    //reportAchievementIdentifier: (NSString*) identifier percentComplete: (float*) percent;
}

void _gamecenterLoadAcheivements(){
    [[GameCenterManager instance] loadAchievements];
}

void _gamecenterResetAcheivements(){
    [[GameCenterManager instance] resetAchievements];
}

void _gamecenterShowLeaderboard(){
    [[GameCenterManager instance] showLeaderboard];
}

void _gamecenterRetrieveFriends(){
    NSLog(@"friend bd");
    [[GameCenterManager instance] retrieveFriends];
}

