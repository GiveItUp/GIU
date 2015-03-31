using UnityEngine;
using System.Collections;

public class SwitchingColumn : Column {

	[HideInInspector]
	public bool animDir = false;
	public float animSpeed = 1;

	public Animation switchAnimation;
	
	public int jumpsPerSwitch = 1;
	
	public override void PlayAnimation()
	{
		
	}

	//[HideInInspector]
	public int numJumps = 0;
	public override void OnJumped()
	{
		if ( numJumps % jumpsPerSwitch == 0)
		{
			IncreaseState();
		
			if (currentState == 1)
			{
				switchAnimation[switchAnimation.clip.name].speed = switchAnimation[switchAnimation.clip.name].length / (1f/(CGame.gamelogic.ball.speed * CGame.gamelogic.ball.GAME_SPEED_SCALE)) * animSpeed;
				switchAnimation[switchAnimation.clip.name].time = 0;
				switchAnimation.Play();
			}
			else
			{
				switchAnimation[switchAnimation.clip.name].speed = -switchAnimation[switchAnimation.clip.name].length / (1f/(CGame.gamelogic.ball.speed * CGame.gamelogic.ball.GAME_SPEED_SCALE)) * animSpeed;
				switchAnimation[switchAnimation.clip.name].time = switchAnimation[switchAnimation.clip.name].length;
				switchAnimation.Play();
			}
			
			animDir = !animDir;
		}
		
		numJumps++;
	}
	
}
