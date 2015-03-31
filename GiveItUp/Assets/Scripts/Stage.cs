using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage : MonoBehaviour 
{
    public List<int> stage = new List<int>
        {
            0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,7,0,0,0,7,0,0,7,0,0,0,0,0,0,1,1,1,0,0,0,1,0,0,0,1,0,0,0,7,0,0,0,0,1,1,1,8,1,1,8,1,1,2,3,3,2,1,0,0,0,0,0,7,0,7,0,7,0,7,0,0,7,0,0,0,0,0,0,1,0,1,0,1,8,1,0,0,0,1,1,7,1,1,2,2,2,3,10,3,3,2,2,1,0,0,0,7,0,7,0,7,0,7,0,7,0,1,1,8,1,0,0,0,0,0,0,1,1,1,5,1,1,1,0,0,0,0,0,0,0,1,1,2,2,2,2,0,0,0,4,0,0,5,0,1,1,2,1,1,6,0,0,0,0,1,1,2,2,2,2,1,1,1,2,1,7,1,2,7,2,3,3,3,2,2,2,2,3,2,2,2,1,0,1,1,1,2,2,6,1,1,1,1,1,1,7,0,0,0,1,2,3,3,3,2,1,0,1,2,3,3,3,7,2,2,2,2,2,2,3,3,2,7,2,7,2,3,2,2,1,2,1,0,0,0,7,0,7,0,0,7,0,0,0,0,0,1,2,2,3,3,3,3,3,3,3,3,3,2,1,2,1,0,0,0,0,0,11,0,0,0,0,0,0,0,0,0,0,0,0,1
        };
        
        
    public void GenerateFromChunks( int minLength, int maxLength )
    {
    	List<Stage> eChunks = Storage.Instance._easyChunks;
    	List<Stage> nChunks = Storage.Instance._normalChunks;
    	List<Stage> hChunks = Storage.Instance._hardChunks;
    	List<Stage> allChunks = new List<Stage>();
    	
    	allChunks.AddRange(eChunks);
    	allChunks.AddRange(nChunks);
    	allChunks.AddRange(hChunks);
    
    	int length = Random.Range( minLength, maxLength ) - 30;
    	
    	int maxEasyLength = (length / 100) * 10;
    	int maxNormalLength = (length / 100) * 30;
    	
    	stage = new List<int>();
    	
    	for (int i=0; i<10; i++)
    	{
    		stage.Add(0);
    	}
    	
    	#if UNITY_EDITOR
    	string stageNames = "";
		#endif
		
		Random.seed = (int)(System.DateTime.Now.Date.Ticks);
		
    	while ( stage.Count < length )
    	{
    		Stage stg;
    		bool isEasy = false;
    		bool isNormal = false;
    		do
    		{
				stg = allChunks[Random.Range( 0, allChunks.Count )];
				
				isEasy = eChunks.Contains(stg);
				isNormal = nChunks.Contains(stg);
				
			}
			while ( (maxEasyLength < 0 && isEasy) || (maxNormalLength < 0 && isNormal) );
			
			if ( isEasy )
			{
				maxEasyLength -= stg.stage.Count;
			}
			if (isNormal)
			{
				maxNormalLength -= stg.stage.Count;
			}
			
			
			#if UNITY_EDITOR
			stageNames += stg.name + ",  ";
			#endif
    		
    		stage.AddRange( stg.stage );
    	}
    	
    	Random.seed = (int)(System.DateTime.Now.Ticks);
    	
    	#if UNITY_EDITOR
    	string path = Application.dataPath + "/genLevels.txt";
    	if (System.IO.File.Exists(path))
    	{
    		using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
    		{
    			sw.WriteLine(stageNames);
    		}
    	}
    	#endif
    	
		for (int i=0; i<20; i++)
		{
			stage.Add(0);
		}
    }
    
    
    public void Save ( string ppKey )
    {
    	string data = "";
    	
    	foreach (int i in stage)
    	{
    		data += i.ToString();
    		data += ",";
    	}
    	
    	data = data.Remove( data.Length - 1, 1 );
    	
    	PlayerPrefs.SetString( ppKey, data);
    }
    
    public void Load ( string ppKey )
    {
    	string data = PlayerPrefs.GetString( ppKey, null );
    	
    	if (data == null) // ennek nem kene bekovetkeznie sose
    	{
    		Debug.Log("HA EZT LATOD AKKOR SZOLJ NEKEM!");
    		GenerateFromChunks( 100, 200);
    		return;
    	}
    	
    	stage = new List<int>();
    	
		char[] sep = { ',' };
    	string[] ss = data.Split( sep );
    	
    	
    	foreach (string s in ss)
    	{
    		stage.Add( System.Int32.Parse( s ));
    	}
    }
    
}
