using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCredits {

	private string title;
	public string Title
	{
		get { return title; }
	}
    public List<string> Names;

    public GameCredits(string _title, List<string> names)
    {
        title = _title;
        Names = names;
    }

    public static List<GameCredits> GetGameCredits()
    {
		List<GameCredits> ret = new List<GameCredits>();
		ret.Add(new GameCredits("", new List<string>() { 
			"****** Give It Up! ******" 
		}));
		
		ret.Add(new GameCredits("", new List<string>() { "" }));
		//ret.Add(new GameCredits("", new List<string>() { "" }));

		//------------------------------------------------------------

		ret.Add(new GameCredits("Developed by", new List<string>() { 
			"INVICTUS-GAMES Ltd" ,
			"EAST2WEST GAMES Ltd",
			"MONSTAR-LAB GAMES Ltd"
		}));
        ret.Add(new GameCredits("Producer", new List<string>() { 
			"Akos Divianszky" ,
			"QiangQiang",
		}));
		ret.Add(new GameCredits("Development", new List<string>() { 
			"Zoltan Kovacs",
			"Janos Barkoczi", 
			"Zsolt Szentpeteri",
			"Laszlo Ladik",
			"Laszlo Palfi",
			"Karoly Molnar",
			"Rein Lv",
			"Long Hui",
			"Cheng Fei",
			"Wei Shao Hua",
			"Zeng Fei",
			"Luo Jia"
		}));
		ret.Add(new GameCredits("QA", new List<string>() { 
			"Peter Hagen", 
			"Laszlo Kiss", 
			"Laszlo Karacs", 
			"Gyorgy Laszlo", 
			"Gabor Antal",
			"Gabor Raszava", 
			"Zhang Xin",
		}));
		ret.Add(new GameCredits("Special Thanks", new List<string>() { 
			"Krisztina Batonyi",
			"Kaika",
			"Wang Ke",
			"Dong Fenfen",
		}));

		ret.Add(new GameCredits("Customer service Title", new List<string>() { 
			"Customer service Email",
			"Customer service NO.",
			"Customer service WWW"
		}));

		ret.Add(new GameCredits("", new List<string>() { "" }));
		ret.Add(new GameCredits("", new List<string>() { "" }));
		ret.Add(new GameCredits("", new List<string>() { 
			"Copyright \u00a9 2014 Invictus Games Ltd. All rights reserved.",
		}));

        return ret;
    }
}
