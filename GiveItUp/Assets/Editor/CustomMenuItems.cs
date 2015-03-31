using UnityEngine;
using UnityEditor;


public class CustomMenuItem : ScriptableObject
{

	[UnityEditor.MenuItem("Tools/Clear PlayerPrefs")]
	static void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}
