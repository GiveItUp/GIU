using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Storage : MonoBehaviour 
{
    public static Storage Instance;
    public Stage testStage = null;
	public List<Stage> _worlds = new List<Stage>();
	public List<Stage> _easyChunks = new List<Stage>();
	public List<Stage> _normalChunks = new List<Stage>();
	public List<Stage> _hardChunks = new List<Stage>();

	public AnimationClip animShowGUI;
	public AnimationClip animHideGUI;

    void Awake()
    {
        Instance = this;
    }

	public Stage GetValidStage(int stageIndex)
	{
		if (stageIndex < CGame.LEVEL_COUNT && stageIndex < Storage.Instance._worlds.Count)
			return _worlds [stageIndex];
		return null;
	}

}
