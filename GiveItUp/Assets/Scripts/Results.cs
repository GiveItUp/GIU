using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Results {

	public int Score;
	public int BestScore;

	public bool IsNewBest = false; 
	
	public int StageIndex;
	public Stage Stage;

	public Results() { }


	public static string GetResultString(int score)
	{
		//score = 99;
		if(score <= 30)
		{
			return messageStrings_1[Random.Range(0, messageStrings_1.Count)];
		}
		else if(score <= 60)
		{
			return messageStrings_2[Random.Range(0, messageStrings_2.Count)];
		}
		else if(score <= 90)
		{
			return messageStrings_3[Random.Range(0, messageStrings_3.Count)];
		}
		else if(score <= 99)
		{
			return messageStrings_4[Random.Range(0, messageStrings_4.Count)];
		}
		else
		{
			return messageStrings_5[Random.Range(0, messageStrings_5.Count)];
		}
	}
	
	
	private static List<string> messageStrings_1 = new List<string>() 
	{ 
		"You'd better play something else.",
		"You call it a result?",
		"I'm bored to be splashed.",
		"Come on!_Watch TV instead!",
		"Are you kiddin'?",
	};
	private static List<string> messageStrings_2 = new List<string>() 
	{ 
		"I see you're getting intuitive.", 
		"Neither bad nor good.",
		"What a progress!",
		"I'm glad you still hold on.",
		"Go ahead!_You can do much better!",
	};
	private static List<string> messageStrings_3 = new List<string>() 
	{ 
		"At last!",
		"You seem to be a gamer...",
		"I see you keep practicing.",
		"Not so hard,_is it?",
		"Just a little practice and you can finish it.",
	};
	private static List<string> messageStrings_4 = new List<string>() 
	{ 
		"Whoo, it's almost done!",
		"You must be angry._Try again!",
		"Just a little more and it's done!",
		"You don't give up,_do you.",
		"Not just try,_do it!",
	};
	
	private static List<string> messageStrings_5 = new List<string>() { 
		"That's it!", 
		"Good job!", 
		"At last!_Someone got this far!", 
		"One hundred percent!!_Believe it?", 
		"That's the way it's done!", 
	};
}
